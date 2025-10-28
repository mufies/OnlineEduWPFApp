using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class TeacherClassesPage : Page
    {
        private readonly ClassSubjectService _classSubjectService;
        private int _currentTeacherId;
        private UserAccount _currentUser;

        public TeacherClassesPage() : this(null)
        {
        }

        public TeacherClassesPage(UserAccount user)
        {
            InitializeComponent();
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _currentUser = user;
            this.Loaded += TeacherClassesPage_Loaded;
        }

        private void TeacherClassesPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTeacherClasses();
        }

        private void LoadTeacherClasses()
        {
            try
            {
                // Get current teacher ID from passed user or MainWindow
                if (_currentUser == null)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow?.CurrentUser == null)
                    {
                        MessageBox.Show("Unable to get current user information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _currentUser = mainWindow.CurrentUser;
                }

                if (_currentUser.TeacherId == null)
                {
                    MessageBox.Show("Current user is not a teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _currentTeacherId = _currentUser.TeacherId.Value;

                // Get all ClassSubjects for this teacher
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                var teacherClassSubjects = allClassSubjects
                    .Where(cs => cs.TeacherId == _currentTeacherId)
                    .Select(cs => new
                    {
                        ClassCode = cs.StudentClass?.ClassCode ?? "N/A",
                        ClassName = cs.StudentClass?.ClassName ?? "N/A",
                        SubjectCode = cs.Subject?.SubjectCode ?? "N/A",
                        SubjectName = cs.Subject?.SubjectName ?? "N/A",
                        Credits = cs.Subject?.Credits ?? 0
                    })
                    .OrderBy(x => x.ClassCode)
                    .ThenBy(x => x.SubjectCode)
                    .ToList();

                dgClasses.ItemsSource = teacherClassSubjects;

                if (!teacherClassSubjects.Any())
                {
                    MessageBox.Show("You are not assigned to teach any classes yet.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading classes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
