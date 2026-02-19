using System;
using System.Collections.ObjectModel;
using TaskManagerDesktop.Infrastructure;
using TaskManagerDesktop.Models;

namespace TaskManagerDesktop.ViewModels
{
    public class MainViewModel : System.ComponentModel.INotifyPropertyChanged
    {
        private readonly NavigationService _navigation;

        public ObservableCollection<TaskItem> Tasks { get; } = new();

        public event System.ComponentModel.PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged(string name)
            => PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(name));

        public MainViewModel() { }

        public MainViewModel(NavigationService navigation)
        {
            _navigation = navigation;
        }
    }
}
