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
    public partial class TeacherSubjectsPage : Page
    {
        private readonly ClassSubjectService _classSubjectService;
        private readonly SubjectService _subjectService;
        private int _currentTeacherId;
        private UserAccount _currentUser;

        public TeacherSubjectsPage() : this(null)
        {
        }

        public TeacherSubjectsPage(UserAccount user)
        {
            InitializeComponent();
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _subjectService = new SubjectService(new SubjectRepository());
            _currentUser = user;
            this.Loaded += TeacherSubjectsPage_Loaded;
        }

        private void TeacherSubjectsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTeacherSubjects();
        }

        private void LoadTeacherSubjects()
        {
            try
            {
                // Get current teacher ID
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

                // Get all ClassSubjects for this teacher and extract unique subjects
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                var teacherSubjects = allClassSubjects
                    .Where(cs => cs.TeacherId == _currentTeacherId)
                    .Select(cs => cs.Subject)
                    .Where(s => s != null)
                    .GroupBy(s => s.SubjectId)
                    .Select(g => g.First())
                    .Select(s => new SubjectViewModel
                    {
                        SubjectId = s.SubjectId,
                        SubjectCode = s.SubjectCode,
                        SubjectName = s.SubjectName,
                        Credits = s.Credits,
                        ClassCount = allClassSubjects.Count(cs => cs.SubjectId == s.SubjectId && cs.TeacherId == _currentTeacherId)
                    })
                    .OrderBy(s => s.SubjectCode)
                    .ToList();

                dgSubjects.ItemsSource = teacherSubjects;

                if (!teacherSubjects.Any())
                {
                    MessageBox.Show("You are not assigned to teach any subjects yet.", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class SubjectViewModel
    {
        public int SubjectId { get; set; }
        public string SubjectCode { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public int Credits { get; set; }
        public int ClassCount { get; set; }
    }
}
