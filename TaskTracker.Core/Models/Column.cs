
using System.Collections.ObjectModel;

namespace TaskTracker.Core.Models;

public class Column
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int Order { get; set; }

    public ObservableCollection<Task> Tasks { get; set; } = new ObservableCollection<Task>();
}