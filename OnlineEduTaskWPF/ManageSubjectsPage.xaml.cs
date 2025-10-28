using System.Windows;
using System.Windows.Controls;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class ManageSubjectsPage : Page
    {
        private readonly SubjectService _subjectService;

        public ManageSubjectsPage()
        {
            InitializeComponent();
            _subjectService = new SubjectService(new SubjectRepository());
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            try
            {
                var subjects = _subjectService.GetAllSubjects();
                dgSubjects.ItemsSource = subjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void AddSubject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AddSubjectDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
            
            if (dialog.IsSuccess)
            {
                LoadSubjects(); // Reload the list
            }
        }

        private void AssignSubject_Click(object sender, RoutedEventArgs e)
        {
            var dialog = new AssignSubjectToClassDialog();
            dialog.Owner = Window.GetWindow(this);
            dialog.ShowDialog();
        }
    }
}
