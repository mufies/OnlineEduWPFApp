using System.Windows;
using System.Windows.Controls;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class ManageClassesPage : Page
    {
        private readonly StudentClassService _classService;

        public ManageClassesPage()
        {
            InitializeComponent();
            _classService = new StudentClassService(new StudentClassRepository());
            LoadClasses();
        }

        private void LoadClasses()
        {
            try
            {
                var classes = _classService.GetAllClasses();
                dgClasses.ItemsSource = classes;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading classes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddClass_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddClassDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
            
            if (dialog.IsSuccess)
            {
                LoadClasses(); // Reload the list
            }
        }
    }
}
