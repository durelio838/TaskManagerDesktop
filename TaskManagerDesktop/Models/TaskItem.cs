using System;
using System.ComponentModel;
using System.Windows.Media;

namespace TaskManagerDesktop.Models
{
    public class TaskItem : INotifyPropertyChanged
    {
        private bool _isCompleted;

        public int Id { get; set; }
        public string Title { get; set; } = "";
        public string? Description { get; set; }
        public Priority? Priority { get; set; }
        public Category? Category { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;

        public bool IsCompleted
        {
            get => _isCompleted;
            set
            {
                _isCompleted = value;
                OnPropertyChanged(nameof(IsCompleted));
                OnPropertyChanged(nameof(StatusDisplay));
                OnPropertyChanged(nameof(StatusColor));
            }
        }

        public string StatusDisplay => IsCompleted ? "Выполнена" :
            DueDate < DateTime.Now ? "Просрочена" : "Активна";

        public Brush StatusColor => IsCompleted
            ? new SolidColorBrush(Colors.Green)
            : DueDate < DateTime.Now
                ? new SolidColorBrush(Colors.Red)
                : new SolidColorBrush(Colors.DodgerBlue);

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
    }
}
