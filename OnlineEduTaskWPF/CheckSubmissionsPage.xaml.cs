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
    public partial class CheckSubmissionsPage : Page
    {
        private readonly ClassSubjectService _classSubjectService;
        private readonly ClassTaskService _classTaskService;
        private readonly StudentTaskService _studentTaskService;
        private int _currentTeacherId;
        private UserAccount _currentUser;
        private List<StudentClass> _teacherClasses = new List<StudentClass>();
        private List<Subject> _classSubjects = new List<Subject>();
        private List<ClassTask> _tasksList = new List<ClassTask>();
        private List<StudentTaskSubmissionViewModel> _allSubmissions = new List<StudentTaskSubmissionViewModel>();

        public CheckSubmissionsPage() : this(null)
        {
        }

        public CheckSubmissionsPage(UserAccount user)
        {
            InitializeComponent();
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _classTaskService = new ClassTaskService(new ClassTaskRepository(), 
                new StudentTaskService(new StudentTaskRepository(), new ClassTaskRepository(), new StudentRepository()));
            _studentTaskService = new StudentTaskService(new StudentTaskRepository(), 
                new ClassTaskRepository(), new StudentRepository());
            _currentUser = user;
            this.Loaded += CheckSubmissionsPage_Loaded;
        }

        private void CheckSubmissionsPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTeacherClasses();
            SetupFilterEvents();
        }

        private void SetupFilterEvents()
        {
            cmbClass.SelectionChanged += CmbClass_SelectionChanged;
            cmbSubject.SelectionChanged += CmbSubject_SelectionChanged;
            cmbTask.SelectionChanged += CmbTask_SelectionChanged;
        }

        private void LoadTeacherClasses()
        {
            try
            {
                if (_currentUser == null)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow?.CurrentUser == null)
                    {
                        ShowError("Unable to get current user information");
                        return;
                    }
                    _currentUser = mainWindow.CurrentUser;
                }

                if (_currentUser.TeacherId == null)
                {
                    ShowError("Current user is not a teacher");
                    return;
                }

                _currentTeacherId = _currentUser.TeacherId.Value;

                // Get all classes where teacher is assigned
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                _teacherClasses = allClassSubjects
                    .Where(cs => cs.TeacherId == _currentTeacherId)
                    .Select(cs => cs.StudentClass)
                    .Where(c => c != null)
                    .GroupBy(c => c.ClassId)
                    .Select(g => g.First())
                    .OrderBy(c => c.ClassCode)
                    .ToList();

                cmbClass.ItemsSource = _teacherClasses;
                cmbClass.DisplayMemberPath = "ClassName";
                cmbClass.SelectedValuePath = "ClassId";

                if (!_teacherClasses.Any())
                {
                    ShowError("You are not assigned to any classes");
                }
            }
            catch (Exception ex)
            {
                ShowError($"Error loading classes: {ex.Message}");
            }
        }

        private void CmbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbSubject.ItemsSource = null;
                cmbTask.ItemsSource = null;
                dgSubmissions.ItemsSource = null;

                if (cmbClass.SelectedValue == null) return;

                int classId = (int)cmbClass.SelectedValue;

                // Get subjects for this class
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                _classSubjects = allClassSubjects
                    .Where(cs => cs.ClassId == classId && cs.TeacherId == _currentTeacherId)
                    .Select(cs => cs.Subject)
                    .Where(s => s != null)
                    .OrderBy(s => s.SubjectCode)
                    .ToList();

                cmbSubject.ItemsSource = _classSubjects;
                cmbSubject.DisplayMemberPath = "SubjectName";
                cmbSubject.SelectedValuePath = "SubjectId";
            }
            catch (Exception ex)
            {
                ShowError($"Error loading subjects: {ex.Message}");
            }
        }

        private void CmbSubject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                cmbTask.ItemsSource = null;
                dgSubmissions.ItemsSource = null;

                if (cmbClass.SelectedValue == null || cmbSubject.SelectedValue == null) return;

                int classId = (int)cmbClass.SelectedValue;
                int subjectId = (int)cmbSubject.SelectedValue;

                // Get tasks for this class and subject
                var allClassTasks = _classTaskService.GetAllClassTasks();
                _tasksList = allClassTasks
                    .Where(ct => ct.ClassId == classId && ct.SubjectId == subjectId)
                    .OrderBy(ct => ct.DueDate)
                    .ToList();

                cmbTask.ItemsSource = _tasksList;
                cmbTask.DisplayMemberPath = "Task.Title";
                cmbTask.SelectedValuePath = "ClassTaskId";

                if (_tasksList.Any())
                    cmbTask.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                ShowError($"Error loading tasks: {ex.Message}");
            }
        }

        private void CmbTask_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                dgSubmissions.ItemsSource = null;

                if (cmbTask.SelectedValue == null) return;

                int classTaskId = (int)cmbTask.SelectedValue;

                // Get all student tasks for this class task
                var allStudentTasks = _studentTaskService.GetAllStudentTasks();
                var submissions = allStudentTasks
                    .Where(st => st.ClassTaskId == classTaskId)
                    .Select(st => new StudentTaskSubmissionViewModel
                    {
                        StudentTaskId = st.StudentTaskId,
                        StudentName = st.Student?.FullName ?? "N/A",
                        StudentCode = st.Student?.StudentCode ?? "N/A",
                        TaskTitle = st.ClassTask?.Task?.Title ?? "N/A",
                        IsSubmitted = st.IsSubmitted,
                        SubmittedDate = st.SubmittedDate,
                        SubmissionContent = st.SubmissionContent,
                        Score = st.Score,
                        MaxScore = st.ClassTask?.MaxScore ?? 0,
                        TeacherFeedback = st.TeacherFeedback,
                        IsGraded = st.Score.HasValue
                    })
                    .OrderBy(s => s.StudentCode)
                    .ToList();

                dgSubmissions.ItemsSource = submissions;
                _allSubmissions = submissions;

                // Show summary
                int submitted = submissions.Count(s => s.IsSubmitted);
                int graded = submissions.Count(s => s.IsGraded);
                int pending = submissions.Count - submitted;

                txtSummary.Text = $"Total: {submissions.Count} | Submitted: {submitted} | Graded: {graded} | Pending: {pending}";
                txtSummary.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                ShowError($"Error loading submissions: {ex.Message}");
            }
        }

        private void dgSubmissions_MouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                var item = dgSubmissions.SelectedItem as StudentTaskSubmissionViewModel;
                if (item == null) return;

                var dialog = new GradeSubmissionDialog(item.StudentTaskId, item.StudentName, item.TaskTitle);
                if (dialog.ShowDialog() == true)
                {
                    // Reload submissions
                    CmbTask_SelectionChanged(null, null);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ShowError(string message)
        {
            txtSummary.Text = $"❌ {message}";
            txtSummary.Foreground = new System.Windows.Media.SolidColorBrush((System.Windows.Media.Color)System.Windows.Media.ColorConverter.ConvertFromString("#F44336"));
            txtSummary.Visibility = Visibility.Visible;
        }
    }

    public class StudentTaskSubmissionViewModel
    {
        public int StudentTaskId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string StudentCode { get; set; } = string.Empty;
        public string TaskTitle { get; set; } = string.Empty;
        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? SubmissionContent { get; set; }
        public double? Score { get; set; }
        public int MaxScore { get; set; }
        public string? TeacherFeedback { get; set; }
        public bool IsGraded { get; set; }

        public string SubmissionStatus
        {
            get
            {
                if (IsGraded) return "✅ Graded";
                if (IsSubmitted) return "📝 Submitted";
                return "⏳ Pending";
            }
        }

        public string ScoreDisplay
        {
            get
            {
                if (Score.HasValue) return $"{Score}/{MaxScore}";
                return "—";
            }
        }
    }
}
