using System;
using System.Windows;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class GradeSubmissionDialog : Window
    {
        private readonly StudentTaskService _studentTaskService;
        private readonly int _studentTaskId;
        private int _maxScore;

        public GradeSubmissionDialog(int studentTaskId, string studentName, string taskTitle)
        {
            InitializeComponent();
            
            _studentTaskService = new StudentTaskService(
                new StudentTaskRepository(),
                new ClassTaskRepository(),
                new StudentRepository());
            
            _studentTaskId = studentTaskId;
            txtStudentName.Text = $"Student: {studentName}";
            txtTaskTitle.Text = $"Task: {taskTitle}";
            
            LoadSubmissionDetails();
        }

        private void LoadSubmissionDetails()
        {
            try
            {
                var studentTasks = _studentTaskService.GetAllStudentTasks();
                var task = studentTasks.Find(st => st.StudentTaskId == _studentTaskId);

                if (task == null)
                {
                    MessageBox.Show("Submission not found", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    this.Close();
                    return;
                }

                // Display submission content
                txtSubmissionContent.Text = task.SubmissionContent ?? "No submission content";
                
                // Set max score
                _maxScore = task.ClassTask?.MaxScore ?? 0;
                txtMaxScore.Text = _maxScore.ToString();

                // If already graded, show existing score and feedback
                if (task.Score.HasValue)
                {
                    txtScore.Text = task.Score.Value.ToString("0.0");
                    txtFeedback.Text = task.TeacherFeedback ?? "";
                    btnSave.Content = "Update Grade";
                }
                else
                {
                    txtScore.Text = "";
                    txtFeedback.Text = "";
                }

                txtScore.Focus();
            }
            catch (Exception ex)
            {
                ShowError($"Error loading submission: {ex.Message}");
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtScore.Text))
                {
                    ShowError("Please enter a score");
                    txtScore.Focus();
                    return;
                }

                if (!double.TryParse(txtScore.Text, out double score) || score < 0 || score > _maxScore)
                {
                    ShowError($"Please enter a valid score between 0 and {_maxScore}");
                    txtScore.Focus();
                    return;
                }

                // Grade the submission
                _studentTaskService.GradeStudentTask(
                    _studentTaskId,
                    (int)score,
                    txtFeedback.Text.Trim());

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error saving grade: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
