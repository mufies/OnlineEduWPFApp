using System.Windows;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;

namespace OnlineEduTaskWPF
{
    public partial class LoginWindow : Window
    {
        private readonly UserAccountService _userService;

        public UserAccount? LoggedInUser { get; private set; }

        public LoginWindow()
        {
            InitializeComponent();
            
            // Initialize service
            _userService = new UserAccountService(new UserAccountRepository());
            
            // Focus on username textbox
            txtUsername.Focus();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Hide previous error
                txtError.Visibility = Visibility.Collapsed;

                // Validate input
                string username = txtUsername.Text.Trim();
                string password = txtPassword.Password;

                if (string.IsNullOrEmpty(username))
                {
                    ShowError("Please enter username");
                    txtUsername.Focus();
                    return;
                }

                if (string.IsNullOrEmpty(password))
                {
                    ShowError("Please enter password");
                    txtPassword.Focus();
                    return;
                }

                // Attempt login
                var user = _userService.Login(username, password);

                if (user != null)
                {
                    LoggedInUser = user;
                    
                    // Open MainWindow with logged in user
                    var mainWindow = new MainWindow(user);
                    mainWindow.Show();
                    
                    // Close login window
                    this.Close();
                }
                else
                {
                    ShowError("Invalid username or password");
                    txtPassword.Clear();
                    txtPassword.Focus();
                }
            }
            catch (Exception ex)
            {
                ShowError($"Login error: {ex.Message}");
            }
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }

        // Handle Enter key press
        protected override void OnKeyDown(System.Windows.Input.KeyEventArgs e)
        {
            base.OnKeyDown(e);
            
            if (e.Key == System.Windows.Input.Key.Enter)
            {
                btnLogin_Click(this, new RoutedEventArgs());
            }
        }
    }
}
