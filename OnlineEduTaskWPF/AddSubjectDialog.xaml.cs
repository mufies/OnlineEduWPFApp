using System;
using System.Windows;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class AddSubjectDialog : Window
    {
        private readonly SubjectService _subjectService;

        public bool IsSuccess { get; private set; }

        public AddSubjectDialog()
        {
            InitializeComponent();
            _subjectService = new SubjectService(new SubjectRepository());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtSubjectCode.Text))
                {
                    ShowError("Subject Code is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtSubjectName.Text))
                {
                    ShowError("Subject Name is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtCredits.Text))
                {
                    ShowError("Credits is required");
                    return;
                }

                if (!int.TryParse(txtCredits.Text, out int credits) || credits <= 0)
                {
                    ShowError("Credits must be a positive number");
                    return;
                }

                // Create Subject (SubjectId will be auto-generated)
                var subject = new Subject
                {
                    SubjectCode = txtSubjectCode.Text.Trim(),
                    SubjectName = txtSubjectName.Text.Trim(),
                    Credits = credits
                };

                _subjectService.AddSubject(subject);

                IsSuccess = true;
                MessageBox.Show("Subject added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error adding subject: {ex.Message}");
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            IsSuccess = false;
            this.Close();
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
