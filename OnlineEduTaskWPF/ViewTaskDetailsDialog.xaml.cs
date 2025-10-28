using System;
using System.Windows;
using System.Windows.Media;

namespace OnlineEduTaskWPF
{
    public partial class ViewTaskDetailsDialog : Window
    {
        public ViewTaskDetailsDialog(StudentTaskViewModel task)
        {
            InitializeComponent();
            LoadTaskDetails(task);
        }

        private void LoadTaskDetails(StudentTaskViewModel task)
        {
            // Basic Info
            txtTitle.Text = task.TaskTitle;
            txtSubject.Text = task.SubjectName;
            txtDescription.Text = task.TaskDescription;
            txtDueDate.Text = task.DueDate.ToString("dd/MM/yyyy HH:mm");
            txtMaxScore.Text = task.MaxScore.ToString();

            // Status
            txtStatus.Text = task.Status;
            borderStatus.Background = new SolidColorBrush(
                (Color)ColorConverter.ConvertFromString(task.StatusColor));

            // Submission Content
            if (task.IsSubmitted)
            {
                pnlSubmission.Visibility = Visibility.Visible;
                txtSubmittedDate.Text = $"Submitted on: {task.SubmittedDate?.ToString("dd/MM/yyyy HH:mm")}";
                txtSubmissionContent.Text = task.SubmissionContent ?? "No content";
            }

            // Grade and Feedback
            if (task.Score.HasValue)
            {
                pnlGrade.Visibility = Visibility.Visible;
                txtScore.Text = task.Score.Value.ToString("0.0");
                txtScoreMax.Text = task.MaxScore.ToString();

                if (!string.IsNullOrWhiteSpace(task.Feedback))
                {
                    pnlFeedback.Visibility = Visibility.Visible;
                    txtFeedback.Text = task.Feedback;
                }
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}
