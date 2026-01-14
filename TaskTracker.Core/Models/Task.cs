using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace TaskTracker.Core.Models;

public class Task : INotifyPropertyChanged
{
    private string _title = string.Empty;
    private string _description = string.Empty;
    private string _status = "To Do";
    private DateTime? _dueDate;
    private Quadrant _quadrant = Quadrant.NotUrgentNotImportant;

    public int Id { get; set; }

    public string Title
    {
        get => _title;
        set
        {
            _title = value;
            OnPropertyChanged();
        }
    }

    public string Description
    {
        get => _description;
        set
        {
            _description = value;
            OnPropertyChanged();
        }
    }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public DateTime? DueDate
    {
        get => _dueDate;
        set
        {
            _dueDate = value;
            OnPropertyChanged();
        }
    }

    public string Status
    {
        get => _status;
        set
        {
            _status = value;
            OnPropertyChanged();
        }
    }

    public int ColumnId { get; set; }
    public Column? Column { get; set; }

    public Quadrant Quadrant
    {
        get => _quadrant;
        set
        {
            _quadrant = value;
            OnPropertyChanged();
        }
    }

    // Реализация INotifyPropertyChanged
    public event PropertyChangedEventHandler? PropertyChanged;

    protected virtual void OnPropertyChanged([CallerMemberName] string? propertyName = null)
    {
        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
}

// Перечисление для матрицы Эйзенхауэра
public enum Quadrant
{
    UrgentImportant,      // Срочно и важно → Делать немедленно
    NotUrgentImportant,   // Не срочно, но важно → Планировать
    UrgentNotImportant,   // Срочно, но не важно → Делегировать
    NotUrgentNotImportant // Не срочно и не важно → Удалить
}