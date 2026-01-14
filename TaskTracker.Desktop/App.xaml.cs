using System.Linq;
using System.Windows;
using TaskTracker.Data;
using TaskTracker.Core.Models;

namespace TaskTracker.Desktop;

public partial class App : Application
{
    protected override void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        // Создаём и мигрируем базу данных
        using var context = new ApplicationDbContext();
        context.Database.EnsureCreated();

        // Можно добавить тестовые данные (опционально)
        if (!context.Columns.Any())
        {
            context.Columns.AddRange(
                new Column { Name = "To Do", Order = 0 },
                new Column { Name = "In Progress", Order = 1 },
                new Column { Name = "Done", Order = 2 }
            );
            context.SaveChanges();
        }
    }
}