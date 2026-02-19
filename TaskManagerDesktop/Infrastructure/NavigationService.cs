using System;
using System.Windows;
using TaskManagerDesktop.Models;
using TaskManagerDesktop.ViewModels;
using TaskManagerDesktop.Views;

namespace TaskManagerDesktop.Infrastructure
{
    public class NavigationService
    {
        private Window? _mainWindow;

        public void ShowMainWindow()
        {
            if (_mainWindow == null)
            {
                var vm = new MainViewModel(this);
                _mainWindow = new MainWindow { DataContext = vm };
            }
            _mainWindow.Show();
        }

        public TaskItem? ShowAddTaskDialog()
        {
            var vm = new TaskEditViewModel();
            var window = new TaskEditWindow(vm) { Owner = _mainWindow };
            return window.ShowDialog() == true ? vm.Task : null;
        }

        public TaskItem? ShowEditTaskDialog(TaskItem task)
        {
            if (task == null) throw new ArgumentNullException(nameof(task));
            var vm = new TaskEditViewModel(task);
            var window = new TaskEditWindow(vm) { Owner = _mainWindow };
            return window.ShowDialog() == true ? vm.Task : null;
        }

        public void Shutdown()
        {
            _mainWindow?.Close();
            Application.Current.Shutdown();
        }
    }
}
