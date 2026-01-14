using System.Windows;
using TaskTracker.Core.Models;

namespace TaskTracker.Desktop;

public partial class TaskDetailsWindow : Window
{
    public TaskTracker.Core.Models.Task Task { get; private set; }
    public bool IsDeleted { get; private set; } = false;

    public TaskDetailsWindow(TaskTracker.Core.Models.Task task)
    {
        InitializeComponent();
        Task = task;
        TitleBox.Text = task.Title;
        DescriptionBox.Text = task.Description ?? "";
        DueDateBox.SelectedDate = task.DueDate;

        // Установка радиобаттонов
        switch (task.Quadrant)
        {
            case Quadrant.UrgentImportant:
                UrgentYes.IsChecked = true;
                ImportantYes.IsChecked = true;
                break;
            case Quadrant.NotUrgentImportant:
                UrgentNo.IsChecked = true;
                ImportantYes.IsChecked = true;
                break;
            case Quadrant.UrgentNotImportant:
                UrgentYes.IsChecked = true;
                ImportantNo.IsChecked = true;
                break;
            case Quadrant.NotUrgentNotImportant:
                UrgentNo.IsChecked = true;
                ImportantNo.IsChecked = true;
                break;
        }
    }

    private void SaveButton_Click(object sender, RoutedEventArgs e)
    {
        Task.Title = TitleBox.Text.Trim();
        Task.Description = DescriptionBox.Text.Trim();
        Task.DueDate = DueDateBox.SelectedDate;

        // Определение квадранта
        if (UrgentYes.IsChecked == true && ImportantYes.IsChecked == true)
            Task.Quadrant = Quadrant.UrgentImportant;
        else if (UrgentNo.IsChecked == true && ImportantYes.IsChecked == true)
            Task.Quadrant = Quadrant.NotUrgentImportant;
        else if (UrgentYes.IsChecked == true && ImportantNo.IsChecked == true)
            Task.Quadrant = Quadrant.UrgentNotImportant;
        else
            Task.Quadrant = Quadrant.NotUrgentNotImportant;

        DialogResult = true;
        Close();
    }

    private void DeleteButton_Click(object sender, RoutedEventArgs e)
    {
        var result = MessageBox.Show("Удалить задачу?", "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
        if (result == MessageBoxResult.Yes)
        {
            IsDeleted = true;
            DialogResult = true;
            Close();
        }
    }

    private void CloseButton_Click(object sender, RoutedEventArgs e)
    {
        Close();
    }
}