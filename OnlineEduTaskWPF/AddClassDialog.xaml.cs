using System;
using System.Windows;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class AddClassDialog : Window
    {
        private readonly StudentClassService _classService;

        public bool IsSuccess { get; private set; }

        public AddClassDialog()
        {
            InitializeComponent();
            _classService = new StudentClassService(new StudentClassRepository());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtClassCode.Text))
                {
                    ShowError("Class Code is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtClassName.Text))
                {
                    ShowError("Class Name is required");
                    return;
                }

                // Create Class (ClassId will be auto-generated)
                var studentClass = new StudentClass
                {
                    ClassCode = txtClassCode.Text.Trim(),
                    ClassName = txtClassName.Text.Trim()
                };

                _classService.AddClass(studentClass);

                IsSuccess = true;
                MessageBox.Show("Class added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error adding class: {ex.Message}");
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
