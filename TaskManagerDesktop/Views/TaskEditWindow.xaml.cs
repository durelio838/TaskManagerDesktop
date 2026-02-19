using System.Windows;
using TaskManagerDesktop.Models;
using TaskManagerDesktop.ViewModels;

namespace TaskManagerDesktop.Views
{
    public partial class TaskEditWindow : Window
    {
        public TaskEditViewModel ViewModel { get; }

        public TaskEditWindow(TaskEditViewModel viewModel)
        {
            InitializeComponent();
            ViewModel = viewModel;
            DataContext = ViewModel;
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(ViewModel.Task?.Title))
            {
                MessageBox.Show("Введите название задачи.", "Ошибка",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            DialogResult = true;
            Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }
    }
}
