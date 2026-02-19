using System;
using System.Collections.Generic;
using System.ComponentModel;
using TaskManagerDesktop.Models;

namespace TaskManagerDesktop.ViewModels
{
    public class TaskEditViewModel : INotifyPropertyChanged
    {
        public List<Priority> Priorities { get; } = Priority.GetDefaultPriorities();
        public List<Category> Categories { get; } = Category.GetDefaultCategories();

        private TaskItem _task = new();
        public TaskItem Task
        {
            get => _task;
            set { _task = value; OnPropertyChanged(nameof(Task)); }
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));

        public TaskEditViewModel()
        {
            _task = new TaskItem
            {
                DueDate = DateTime.Now.AddDays(1),
                Priority = Priorities[2],
                Category = Categories[0]
            };
        }

        public TaskEditViewModel(TaskItem task)
        {
            _task = task;
        }
    }
}
