using System;
using System.Windows;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class SubmitTaskDialog : Window
    {
        private readonly StudentTaskService _studentTaskService;
        private readonly int _studentTaskId;

        public SubmitTaskDialog(int studentTaskId, string taskTitle)
        {
            InitializeComponent();
            
            _studentTaskService = new StudentTaskService(
                new StudentTaskRepository(),
                new ClassTaskRepository(),
                new StudentRepository());
            
            _studentTaskId = studentTaskId;
            txtTaskTitle.Text = taskTitle;
            txtSubmission.Focus();
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hide previous error
                txtError.Visibility = Visibility.Collapsed;

                // Validate input
                if (string.IsNullOrWhiteSpace(txtSubmission.Text))
                {
                    ShowError("Please enter your submission content");
                    txtSubmission.Focus();
                    return;
                }

                // Confirm submission
                var result = MessageBox.Show(
                    "Are you sure you want to submit? You cannot edit after submission.",
                    "Confirm Submission",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (result == MessageBoxResult.No)
                    return;

                // Submit task
                _studentTaskService.SubmitStudentTask(
                    _studentTaskId,
                    txtSubmission.Text.Trim());

                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error submitting task: {ex.Message}");
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
