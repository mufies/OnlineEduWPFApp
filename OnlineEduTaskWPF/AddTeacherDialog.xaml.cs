using System;
using System.Linq;
using System.Windows;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class AddTeacherDialog : Window
    {
        private readonly UserAccountService _userService;

        public bool IsSuccess { get; private set; }

        public AddTeacherDialog()
        {
            InitializeComponent();
            _userService = new UserAccountService(new UserAccountRepository());
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (string.IsNullOrWhiteSpace(txtTeacherId.Text))
                {
                    ShowError("Teacher ID is required");
                    return;
                }

                if (!int.TryParse(txtTeacherId.Text, out int teacherId))
                {
                    ShowError("Teacher ID must be a number");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtUsername.Text))
                {
                    ShowError("Username is required");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtPassword.Password))
                {
                    ShowError("Password is required");
                    return;
                }

                // Check if username exists
                if (_userService.UsernameExists(txtUsername.Text))
                {
                    ShowError("Username already exists");
                    return;
                }

                // Create UserAccount for Teacher (UserAccountId will be auto-generated)
                var userAccount = new UserAccount
                {
                    Username = txtUsername.Text.Trim(),
                    Password = txtPassword.Password,
                    Role = "Teacher",
                    StudentId = null,
                    TeacherId = teacherId
                };

                _userService.AddUser(userAccount);

                IsSuccess = true;
                MessageBox.Show("Teacher added successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error adding teacher: {ex.Message}");
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
