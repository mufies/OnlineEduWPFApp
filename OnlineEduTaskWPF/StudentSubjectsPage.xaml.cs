using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class StudentSubjectsPage : Page
    {
        private readonly StudentService _studentService;
        private readonly ClassSubjectService _classSubjectService;
        private readonly SubjectService _subjectService;

        public StudentSubjectsPage()
        {
            InitializeComponent();
            
            _studentService = new StudentService(new StudentRepository());
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _subjectService = new SubjectService(new SubjectRepository());
            
            Loaded += StudentSubjectsPage_Loaded;
        }

        private void StudentSubjectsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadSubjects();
        }

        private void LoadSubjects()
        {
            try
            {
                // Get current user from MainWindow
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow?.CurrentUser == null || mainWindow.CurrentUser.StudentId == null)
                {
                    MessageBox.Show("Unable to get student information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Get student
                var student = _studentService.GetStudentById(mainWindow.CurrentUser.StudentId.Value);
                if (student == null)
                {
                    MessageBox.Show("Student not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Get subjects for student's class
                var classSubjects = _classSubjectService.GetClassSubjectsByClassId(student.ClassId);
                
                if (classSubjects == null || classSubjects.Count == 0)
                {
                    lstSubjects.ItemsSource = null;
                    MessageBox.Show("No subjects assigned to your class yet", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Get full subject details
                var subjects = classSubjects
                    .Select(cs => _subjectService.GetSubjectById(cs.SubjectId))
                    .Where(s => s != null)
                    .ToList();

                lstSubjects.ItemsSource = subjects;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnViewTasks_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var subject = button?.Tag as Subject;
                if (subject == null)
                {
                    MessageBox.Show("Unable to get subject information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Navigate to StudentTasksPage
                var mainWindow = Window.GetWindow(this) as MainWindow;
                if (mainWindow != null)
                {
                    mainWindow.mainFrame.Navigate(new StudentTasksPage(mainWindow.CurrentUser));
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
