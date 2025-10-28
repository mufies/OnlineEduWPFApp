using System.Windows;
using System.Windows.Controls;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class ManageTeachersPage : Page
    {
        private readonly UserAccountService _userService;

        public ManageTeachersPage()
        {
            InitializeComponent();
            _userService = new UserAccountService(new UserAccountRepository());
            LoadTeachers();
        }

        private void LoadTeachers()
        {
            try
            {
                var teachers = _userService.GetUsersByRole("Teacher");
                dgTeachers.ItemsSource = teachers;
                
                if (teachers.Count == 0)
                {
                    MessageBox.Show("No teachers found in the database.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading teachers: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddTeacher_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddTeacherDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
            
            if (dialog.IsSuccess)
            {
                LoadTeachers(); // Reload the list
            }
        }
    }
}
