using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class StudentTasksPage : Page
    {
        private readonly StudentTaskService _studentTaskService;
        private readonly StudentService _studentService;
        private int _currentStudentId;
        private UserAccount _currentUser;
        private List<StudentTaskViewModel> _allTasks = new List<StudentTaskViewModel>();
        private string _currentFilter = "All";

        public StudentTasksPage() : this(null)
        {
        }

        public StudentTasksPage(UserAccount user)
        {
            InitializeComponent();
            _studentTaskService = new StudentTaskService(
                new StudentTaskRepository(),
                new ClassTaskRepository(),
                new StudentRepository());
            _studentService = new StudentService(new StudentRepository());
            _currentUser = user;

            this.Loaded += StudentTasksPage_Loaded;
        }

        private void StudentTasksPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentStudent();
            LoadStudentTasks();
            SetupFilterButtons();
        }

        private void LoadCurrentStudent()
        {
            try
            {
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

                if (_currentUser.StudentId == null)
                {
                    MessageBox.Show("Current user is not a student", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _currentStudentId = _currentUser.StudentId.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading student info: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadStudentTasks()
        {
            try
            {
                // Get all student tasks
                var studentTasks = _studentTaskService.GetStudentTasksByStudentId(_currentStudentId);

                _allTasks = studentTasks.Select(st => new StudentTaskViewModel
                {
                    StudentTaskId = st.StudentTaskId,
                    TaskTitle = st.ClassTask?.Task?.Title ?? "N/A",
                    TaskDescription = st.ClassTask?.Task?.Description ?? "N/A",
                    SubjectName = st.ClassTask?.Subject?.SubjectName ?? "N/A",
                    DueDate = st.ClassTask?.DueDate ?? DateTime.Now,
                    MaxScore = st.ClassTask?.MaxScore ?? 0,
                    IsSubmitted = st.IsSubmitted,
                    SubmissionContent = st.SubmissionContent,
                    SubmittedDate = st.SubmittedDate,
                    Score = st.Score,
                    Feedback = st.TeacherFeedback,
                    Status = GetTaskStatus(st)
                }).OrderBy(t => t.DueDate).ToList();

                ApplyFilter(_currentFilter);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading tasks: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetTaskStatus(StudentTask task)
        {
            if (task.Score.HasValue)
                return "Graded";
            if (task.IsSubmitted)
                return "Submitted";
            if (task.ClassTask?.DueDate < DateTime.Now)
                return "Overdue";
            return "Pending";
        }

        private void SetupFilterButtons()
        {
            btnAllTasks.Click += (s, e) => FilterTasks("All");
            btnPending.Click += (s, e) => FilterTasks("Pending");
            btnSubmitted.Click += (s, e) => FilterTasks("Submitted");
            btnGraded.Click += (s, e) => FilterTasks("Graded");
        }

        private void FilterTasks(string filter)
        {
            _currentFilter = filter;
            ApplyFilter(filter);
            UpdateFilterButtonStyles(filter);
        }

        private void ApplyFilter(string filter)
        {
            if (_allTasks == null) return;

            IEnumerable<StudentTaskViewModel> filtered = _allTasks;

            switch (filter)
            {
                case "Pending":
                    filtered = _allTasks.Where(t => t.Status == "Pending" || t.Status == "Overdue");
                    break;
                case "Submitted":
                    filtered = _allTasks.Where(t => t.Status == "Submitted");
                    break;
                case "Graded":
                    filtered = _allTasks.Where(t => t.Status == "Graded");
                    break;
            }

            lstTasks.ItemsSource = filtered.ToList();
        }

        private void UpdateFilterButtonStyles(string activeFilter)
        {
            var activeColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#2196F3"));
            var inactiveColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#E0E0E0"));
            var activeTextColor = Brushes.White;
            var inactiveTextColor = new SolidColorBrush((Color)ColorConverter.ConvertFromString("#333"));

            btnAllTasks.Background = activeFilter == "All" ? activeColor : inactiveColor;
            btnAllTasks.Foreground = activeFilter == "All" ? activeTextColor : inactiveTextColor;

            btnPending.Background = activeFilter == "Pending" ? activeColor : inactiveColor;
            btnPending.Foreground = activeFilter == "Pending" ? activeTextColor : inactiveTextColor;

            btnSubmitted.Background = activeFilter == "Submitted" ? activeColor : inactiveColor;
            btnSubmitted.Foreground = activeFilter == "Submitted" ? activeTextColor : inactiveTextColor;

            btnGraded.Background = activeFilter == "Graded" ? activeColor : inactiveColor;
            btnGraded.Foreground = activeFilter == "Graded" ? activeTextColor : inactiveTextColor;
        }

        private void SubmitTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var task = button?.DataContext as StudentTaskViewModel;

                if (task == null) return;

                if (task.IsSubmitted)
                {
                    MessageBox.Show("This task has already been submitted", "Information", MessageBoxButton.OK, MessageBoxImage.Information);
                    return;
                }

                // Check if task is overdue
                if (task.DueDate < DateTime.Now)
                {
                    MessageBox.Show("This task is overdue and cannot be submitted", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                    return;
                }

                // Show submit dialog
                var dialog = new SubmitTaskDialog(task.StudentTaskId, task.TaskTitle);
                if (dialog.ShowDialog() == true)
                {
                    // Reload tasks
                    LoadStudentTasks();
                    MessageBox.Show("Task submitted successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error submitting task: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void ViewDetails_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var button = sender as Button;
                var task = button?.DataContext as StudentTaskViewModel;

                if (task == null) return;

                var dialog = new ViewTaskDetailsDialog(task);
                dialog.ShowDialog();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error viewing details: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }

    public class StudentTaskViewModel
    {
        public int StudentTaskId { get; set; }
        public string TaskTitle { get; set; } = string.Empty;
        public string TaskDescription { get; set; } = string.Empty;
        public string SubjectName { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public int MaxScore { get; set; }
        public bool IsSubmitted { get; set; }
        public string? SubmissionContent { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public double? Score { get; set; }
        public string? Feedback { get; set; }
        public string Status { get; set; } = string.Empty;

        public string StatusColor
        {
            get
            {
                return Status switch
                {
                    "Graded" => "#4CAF50",
                    "Submitted" => "#2196F3",
                    "Overdue" => "#F44336",
                    _ => "#FF9800"
                };
            }
        }

        public string ActionButtonText
        {
            get
            {
                return Status switch
                {
                    "Graded" => "View Score",
                    "Submitted" => "View Submission",
                    _ => "Submit"
                };
            }
        }

        public bool CanSubmit => Status == "Pending";
    }
}
