using System.Windows;
using TaskTracker.Core.Models;

namespace TaskTracker.Desktop;

public partial class AddTaskWindow : Window
{
    // 👇 Используем полное имя модели!
    public TaskTracker.Core.Models.Task? NewTask { get; private set; }

    public AddTaskWindow()
    {
        InitializeComponent();
    }

    private void OnAddClick(object sender, RoutedEventArgs e)
    {
        if (string.IsNullOrWhiteSpace(TitleBox.Text))
        {
            MessageBox.Show("Введите заголовок задачи.");
            return;
        }

        NewTask = new TaskTracker.Core.Models.Task
        {
            Title = TitleBox.Text.Trim(),
            Description = DescriptionBox.Text.Trim(),
            ColumnId = 1 // По умолчанию — колонка "To Do"
        };

        // Установка срока выполнения
        if (DueDateBox.SelectedDate.HasValue)
        {
            NewTask.DueDate = DueDateBox.SelectedDate.Value;
        }

        // Определение квадранта
        if (UrgentYes.IsChecked == true && ImportantYes.IsChecked == true)
            NewTask.Quadrant = TaskTracker.Core.Models.Quadrant.UrgentImportant;
        else if (UrgentNo.IsChecked == true && ImportantYes.IsChecked == true)
            NewTask.Quadrant = TaskTracker.Core.Models.Quadrant.NotUrgentImportant;
        else if (UrgentYes.IsChecked == true && ImportantNo.IsChecked == true)
            NewTask.Quadrant = TaskTracker.Core.Models.Quadrant.UrgentNotImportant;
        else
            NewTask.Quadrant = TaskTracker.Core.Models.Quadrant.NotUrgentNotImportant;

        DialogResult = true;
        Close();
    }

    private void OnCancelClick(object sender, RoutedEventArgs e)
    {
        DialogResult = false;
        Close();
    }

    private void DescriptionBox_TextChanged(object sender, System.Windows.Controls.TextChangedEventArgs e)
    {

    }
}