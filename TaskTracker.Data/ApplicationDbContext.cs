using System.IO;
using Microsoft.EntityFrameworkCore;
using TaskTracker.Core.Models;

namespace TaskTracker.Data;

public class ApplicationDbContext : DbContext
{
    public DbSet<TaskTracker.Core.Models.Task> Tasks { get; set; } = null!;
    public DbSet<Column> Columns { get; set; } = null!;
    // public DbSet<Board> Boards { get; set; } = null!; // раскомментировать, если создали Board

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Путь к файлу базы данных — рядом с .exe
        string dbPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "tasktracker.db");
        optionsBuilder.UseSqlite($"Data Source={dbPath}");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Настройка колонок по умолчанию
        modelBuilder.Entity<Column>().HasData(
            new Column { Id = 1, Name = "To Do", Order = 0 },
            new Column { Id = 2, Name = "In Progress", Order = 1 },
            new Column { Id = 3, Name = "Done", Order = 2 }
        );

        base.OnModelCreating(modelBuilder);
    }
}