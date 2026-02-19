using System.Collections.Generic;
using System.ComponentModel;

namespace TaskManagerDesktop.Models
{
    public class Category : INotifyPropertyChanged
    {
        public int Id { get; set; }
        public string Name { get; set; } = "";
        public string Icon { get; set; } = "";

        public event PropertyChangedEventHandler? PropertyChanged;

        public static List<Category> GetDefaultCategories() => new()
        {
            new Category { Id = 1, Name = "Работа",   Icon = "Work"    },
            new Category { Id = 2, Name = "Личное",   Icon = "Home"    },
            new Category { Id = 3, Name = "Учеба",    Icon = "Study"   },
            new Category { Id = 4, Name = "Здоровье", Icon = "Health"  },
            new Category { Id = 5, Name = "Финансы",  Icon = "Finance" },
            new Category { Id = 6, Name = "Семья",    Icon = "Family"  },
            new Category { Id = 7, Name = "Отдых",    Icon = "Rest"    }
        };
    }
}
