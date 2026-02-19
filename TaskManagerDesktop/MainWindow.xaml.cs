using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using TaskManagerDesktop.Models;
using TaskManagerDesktop.ViewModels;
using TaskManagerDesktop.Views;

namespace TaskManagerDesktop
{
    public partial class MainWindow : Window
    {
        private MainViewModel _viewModel;
        private ICollectionView _tasksView;

        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            this.DataContext = _viewModel;
            Loaded += MainWindow_Loaded;
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            InitializeSampleData();
            SetupCollectionView();
            UpdateStatistics();
            StatusDateText.Text = $"Дата: {DateTime.Now:dd.MM.yyyy}";
        }

        private void InitializeSampleData()
        {
            _viewModel.Tasks.Clear();

            var priorities = Priority.GetDefaultPriorities();
            var categories = Category.GetDefaultCategories();

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 1,
                Title = "Завершить проект",
                Description = "Доделать WPF приложение TaskManager с полным функционалом",
                Priority = priorities.First(p => p.Level == 1),
                Category = categories.First(c => c.Name == "Работа"),
                DueDate = DateTime.Now.AddDays(3),
                IsCompleted = false,
                CreatedDate = DateTime.Now.AddDays(-5)
            });

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 2,
                Title = "Сдать отчет",
                Description = "Подготовить квартальный отчет по продажам",
                Priority = priorities.First(p => p.Level == 1),
                Category = categories.First(c => c.Name == "Работа"),
                DueDate = DateTime.Now.AddDays(-1),
                IsCompleted = false,
                CreatedDate = DateTime.Now.AddDays(-10)
            });

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 3,
                Title = "Купить продукты",
                Description = "Молоко, хлеб, яйца, овощи",
                Priority = priorities.First(p => p.Level == 2),
                Category = categories.First(c => c.Name == "Личное"),
                DueDate = DateTime.Now.AddDays(1),
                IsCompleted = false,
                CreatedDate = DateTime.Now.AddDays(-2)
            });

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 4,
                Title = "Изучить MVVM паттерн",
                Description = "Прочитать статьи и посмотреть видео о паттерне MVVM",
                Priority = priorities.First(p => p.Level == 2),
                Category = categories.First(c => c.Name == "Учеба"),
                DueDate = DateTime.Now.AddDays(7),
                IsCompleted = false,
                CreatedDate = DateTime.Now.AddDays(-3)
            });

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 5,
                Title = "Записаться к врачу",
                Description = "Пройти ежегодный медосмотр",
                Priority = priorities.First(p => p.Level == 3),
                Category = categories.First(c => c.Name == "Здоровье"),
                DueDate = DateTime.Now.AddDays(14),
                IsCompleted = false,
                CreatedDate = DateTime.Now.AddDays(-1)
            });

            _viewModel.Tasks.Add(new TaskItem
            {
                Id = 6,
                Title = "Настроить CI/CD",
                Description = "Настроить автоматическое развертывание проекта",
                Priority = priorities.First(p => p.Level == 3),
                Category = categories.First(c => c.Name == "Работа"),
                DueDate = DateTime.Now.AddDays(10),
                IsCompleted = true,
                CreatedDate = DateTime.Now.AddDays(-15)
            });
        }

        private void SetupCollectionView()
        {
            _tasksView = CollectionViewSource.GetDefaultView(_viewModel.Tasks);
            _tasksView.Filter = FilterTask;
            TasksDataGrid.ItemsSource = _tasksView;
        }

        private bool FilterTask(object obj)
        {
            if (obj is not TaskItem task) return false;

            if (!string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                string search = SearchTextBox.Text.ToLower();
                bool matchesSearch = task.Title.ToLower().Contains(search) ||
                                    (task.Description?.ToLower().Contains(search) ?? false);
                if (!matchesSearch) return false;
            }

            var selectedFilter = (FilterComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (selectedFilter)
            {
                case "Активные":
                    if (task.IsCompleted) return false;
                    break;
                case "Выполненные":
                    if (!task.IsCompleted) return false;
                    break;
                case "Высокий приоритет":
                    if (task.Priority?.Level != 1) return false;
                    break;
                case "Просроченные":
                    if (task.DueDate >= DateTime.Now || task.IsCompleted) return false;
                    break;
            }

            return true;
        }

        private void ApplyFiltersAndSort()
        {
            _tasksView?.Refresh();
            ApplySorting();
            UpdateStatistics();
        }

        private void ApplySorting()
        {
            if (_tasksView == null) return;
            _tasksView.SortDescriptions.Clear();

            var selectedSort = (SortComboBox.SelectedItem as ComboBoxItem)?.Content?.ToString();
            switch (selectedSort)
            {
                case "По приоритету":
                    _tasksView.SortDescriptions.Add(new SortDescription("Priority.Level", ListSortDirection.Ascending));
                    break;
                case "По сроку":
                    _tasksView.SortDescriptions.Add(new SortDescription("DueDate", ListSortDirection.Ascending));
                    break;
                case "По названию":
                    _tasksView.SortDescriptions.Add(new SortDescription("Title", ListSortDirection.Ascending));
                    break;
                case "По категории":
                    _tasksView.SortDescriptions.Add(new SortDescription("Category.Name", ListSortDirection.Ascending));
                    break;
                default:
                    _tasksView.SortDescriptions.Add(new SortDescription("CreatedDate", ListSortDirection.Descending));
                    break;
            }
        }

        private void GroupByPriorityCheckBox_Click(object sender, RoutedEventArgs e)
        {
            if (_tasksView == null) return;
            _tasksView.GroupDescriptions.Clear();
            if (GroupByPriorityCheckBox.IsChecked == true)
                _tasksView.GroupDescriptions.Add(new PropertyGroupDescription("Priority.Name"));
        }

        private void UpdateStatistics()
        {
            try
            {
                TotalTasksText.Text = $"Всего задач: {_viewModel.Tasks.Count}";
                CompletedTasksText.Text = $"Выполнено: {_viewModel.Tasks.Count(t => t.IsCompleted)}";
            }
            catch
            {
                TotalTasksText.Text = "Всего задач: 0";
                CompletedTasksText.Text = "Выполнено: 0";
            }
        }

        private void SearchTextBox_TextChanged(object sender, TextChangedEventArgs e)
            => ApplyFiltersAndSort();

        private void FilterComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_tasksView != null) ApplyFiltersAndSort();
        }

        private void SortComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_tasksView != null) ApplyFiltersAndSort();
        }

        private void TasksDataGrid_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
            {
                DetailTitle.Text = selectedTask.Title;
                DetailDescription.Text = selectedTask.Description ?? "-";
                DetailPriority.Text = selectedTask.Priority?.Name ?? "-";
                try
                {
                    var color = (Color)ColorConverter.ConvertFromString(selectedTask.Priority?.Color ?? "#808080");
                    DetailPriorityColor.Fill = new SolidColorBrush(color);
                }
                catch
                {
                    DetailPriorityColor.Fill = new SolidColorBrush(Colors.Gray);
                }
                DetailCategory.Text = selectedTask.Category?.Name ?? "-";
                DetailDueDate.Text = selectedTask.DueDate.ToString("dd.MM.yyyy");
                DetailStatus.Text = selectedTask.StatusDisplay;
                DetailCreatedDate.Text = selectedTask.CreatedDate.ToString("dd.MM.yyyy HH:mm");
            }
            else
            {
                DetailTitle.Text = "-";
                DetailDescription.Text = "-";
                DetailPriority.Text = "-";
                DetailPriorityColor.Fill = new SolidColorBrush(Colors.Gray);
                DetailCategory.Text = "-";
                DetailDueDate.Text = "-";
                DetailStatus.Text = "-";
                DetailCreatedDate.Text = "-";
            }
        }

        private void AddTaskButton_Click(object sender, RoutedEventArgs e)
        {
            var vm = new TaskEditViewModel();
            var editWindow = new TaskEditWindow(vm);
            if (editWindow.ShowDialog() == true)
            {
                var newTask = vm.Task;
                newTask.Id = _viewModel.Tasks.Any() ? _viewModel.Tasks.Max(t => t.Id) + 1 : 1;
                newTask.CreatedDate = DateTime.Now;
                _viewModel.Tasks.Add(newTask);
                ApplyFiltersAndSort();
                MessageBox.Show("Задача успешно добавлена!", "Успех",
                    MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        private void EditTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
            {
                var vm = new TaskEditViewModel(selectedTask);
                var editWindow = new TaskEditWindow(vm);
                if (editWindow.ShowDialog() == true)
                {
                    var index = _viewModel.Tasks.IndexOf(selectedTask);
                    _viewModel.Tasks[index] = vm.Task;
                    ApplyFiltersAndSort();
                    MessageBox.Show("Задача успешно обновлена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для редактирования", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void DeleteTaskButton_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
            {
                var result = MessageBox.Show($"Удалить задачу \"{selectedTask.Title}\"?",
                    "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _viewModel.Tasks.Remove(selectedTask);
                    ApplyFiltersAndSort();
                    MessageBox.Show("Задача удалена!", "Успех",
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Выберите задачу для удаления", "Внимание",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            ApplyFiltersAndSort();
            StatusText.Text = "Обновлено";
        }

        private void AddTaskMenuItem_Click(object sender, RoutedEventArgs e)
            => AddTaskButton_Click(sender, e);

        private void EditTaskMenuItem_Click(object sender, RoutedEventArgs e)
            => EditTaskButton_Click(sender, e);

        private void DeleteTaskMenuItem_Click(object sender, RoutedEventArgs e)
            => DeleteTaskButton_Click(sender, e);

        private void MarkCompletedMenuItem_Click(object sender, RoutedEventArgs e)
        {
            if (TasksDataGrid.SelectedItem is TaskItem selectedTask)
            {
                selectedTask.IsCompleted = !selectedTask.IsCompleted;
                _tasksView?.Refresh();
                UpdateStatistics();
            }
        }

        private void ExitMenuItem_Click(object sender, RoutedEventArgs e)
            => Application.Current.Shutdown();
    }
}
