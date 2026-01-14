using System.Windows;
using TaskTracker.Data;

namespace TaskTracker.Desktop;

public partial class CalendarWindow : Window
{
    public CalendarWindow()
    {
        InitializeComponent();
        using var context = new ApplicationDbContext();
        var tasks = context.Tasks
            .Where(t => t.DueDate.HasValue)
            .OrderBy(t => t.DueDate)
            .ToList();
        TasksListView.ItemsSource = tasks;
    }
}