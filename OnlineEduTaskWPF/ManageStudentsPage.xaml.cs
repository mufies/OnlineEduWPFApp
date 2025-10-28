using System.Windows;
using System.Windows.Controls;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class ManageStudentsPage : Page
    {
        private readonly StudentService _studentService;

        public ManageStudentsPage()
        {
            InitializeComponent();
            _studentService = new StudentService(new StudentRepository());
            LoadStudents();
        }

        private void LoadStudents()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                dgStudents.ItemsSource = students;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading students: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddStudent_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddStudentDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
            
            if (dialog.IsSuccess)
            {
                LoadStudents(); // Reload the list
            }
        }
    }
}
