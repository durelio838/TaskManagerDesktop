using System.Collections.Generic;
using System.ComponentModel;

namespace TaskManagerDesktop.Models
{
    public class Priority : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public int Level { get; set; }
        public string Color { get; set; } = "#808080";

        public event PropertyChangedEventHandler? PropertyChanged;

        public static List<Priority> GetDefaultPriorities() => new()
        {
            new Priority { Id = 1, Name = "Критический", Level = 1, Color = "#D32F2F" },
            new Priority { Id = 2, Name = "Высокий",     Level = 2, Color = "#F57C00" },
            new Priority { Id = 3, Name = "Средний",     Level = 3, Color = "#FBC02D" },
            new Priority { Id = 4, Name = "Низкий",      Level = 4, Color = "#388E3C" }
        };
    }
}
