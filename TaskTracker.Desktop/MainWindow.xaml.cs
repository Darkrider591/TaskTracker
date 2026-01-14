using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using TaskTracker.Data;
using TaskTracker.Core.Models;
using Microsoft.EntityFrameworkCore;
using System.Windows.Controls;

namespace TaskTracker.Desktop;

public partial class MainWindow : Window
{
    private readonly ApplicationDbContext _context;
    private TaskTracker.Core.Models.Task? _draggedTask;
    private ListBox? _sourceListBox;

    // Свойства для статистики (привязка в XAML)
    public int ToDoCount { get; set; }
    public int InProgressCount { get; set; }
    public int DoneCount { get; set; }
    public int OverdueCount { get; set; }

    public int UrgentImportantCount { get; set; }
    public int NotUrgentImportantCount { get; set; }
    public int UrgentNotImportantCount { get; set; }
    public int NotUrgentNotImportantCount { get; set; }

    public MainWindow()
    {
        InitializeComponent();
        _context = new ApplicationDbContext();
        LoadData();
        UpdateStats(); // Инициализация статистики
    }

    private void LoadData()
    {
        var columns = _context.Columns.Include(c => c.Tasks).ToList();

        foreach (var column in columns)
        {
            switch (column.Name)
            {
                case "To Do":
                    ToDoList.ItemsSource = column.Tasks;
                    break;
                case "In Progress":
                    InProgressList.ItemsSource = column.Tasks;
                    break;
                case "Done":
                    DoneList.ItemsSource = column.Tasks;
                    break;
            }
        }
    }

    private void UpdateStats()
    {
        var tasks = _context.Tasks.ToList();

        ToDoCount = tasks.Count(t => t.ColumnId == 1);
        InProgressCount = tasks.Count(t => t.ColumnId == 2);
        DoneCount = tasks.Count(t => t.ColumnId == 3);
        OverdueCount = tasks.Count(t => t.DueDate.HasValue && t.DueDate.Value.Date < DateTime.Now.Date);

        UrgentImportantCount = tasks.Count(t => t.Quadrant == Quadrant.UrgentImportant);
        NotUrgentImportantCount = tasks.Count(t => t.Quadrant == Quadrant.NotUrgentImportant);
        UrgentNotImportantCount = tasks.Count(t => t.Quadrant == Quadrant.UrgentNotImportant);
        NotUrgentNotImportantCount = tasks.Count(t => t.Quadrant == Quadrant.NotUrgentNotImportant);

        DataContext = this; // Обновление привязок в XAML
    }

    private void AddTask_Click(object sender, RoutedEventArgs e)
    {
        var addWindow = new AddTaskWindow();
        addWindow.Owner = this;
        if (addWindow.ShowDialog() == true && addWindow.NewTask != null)
        {
            _context.Tasks.Add(addWindow.NewTask);
            _context.SaveChanges();

            LoadData();
            UpdateStats(); // Обновить статистику
        }
    }

    private void ShowCalendar_Click(object sender, RoutedEventArgs e)
    {
        var calendarWindow = new CalendarWindow();
        calendarWindow.Owner = this;
        calendarWindow.ShowDialog();
    }

    private void TaskList_PreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox == null) return;

        var item = listBox.InputHitTest(e.GetPosition(listBox)) as FrameworkElement;
        if (item == null) return;

        var task = item.DataContext as TaskTracker.Core.Models.Task;
        if (task == null) return;

        _draggedTask = task;
        _sourceListBox = listBox;
    }

    private void TaskList_PreviewMouseMove(object sender, MouseEventArgs e)
    {
        if (_draggedTask == null || e.LeftButton != MouseButtonState.Pressed) return;

        DragDrop.DoDragDrop(_sourceListBox, _draggedTask, DragDropEffects.Move);
        _draggedTask = null;
        _sourceListBox = null;
    }

    private void TaskList_DragEnter(object sender, DragEventArgs e)
    {
        if (!e.Data.GetDataPresent(typeof(TaskTracker.Core.Models.Task))) return;

        e.Effects = DragDropEffects.Move;
        e.Handled = true;
    }

    private void TaskList_Drop(object sender, DragEventArgs e)
    {
        if (_draggedTask == null) return;

        var targetListBox = sender as ListBox;
        if (targetListBox == null) return;

        int newColumnId = 0;
        if (targetListBox == ToDoList) newColumnId = 1;
        else if (targetListBox == InProgressList) newColumnId = 2;
        else if (targetListBox == DoneList) newColumnId = 3;

        _draggedTask.ColumnId = newColumnId;
        _context.SaveChanges();

        LoadData();
        UpdateStats(); // Обновить статистику
    }

    private void TaskList_MouseDoubleClick(object sender, MouseButtonEventArgs e)
    {
        var listBox = sender as ListBox;
        if (listBox?.SelectedItem == null) return;

        var task = listBox.SelectedItem as TaskTracker.Core.Models.Task;
        if (task == null) return;

        var detailsWindow = new TaskDetailsWindow(task);
        detailsWindow.Owner = this;
        if (detailsWindow.ShowDialog() == true)
        {
            if (detailsWindow.IsDeleted)
            {
                _context.Tasks.Remove(task);
                _context.SaveChanges();
            }
            else
            {
                _context.SaveChanges();
            }

            LoadData();
            UpdateStats(); // Обновить статистику
        }
    }
}