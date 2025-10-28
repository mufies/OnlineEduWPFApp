using System.Windows;
using System.Windows.Controls;
using StudentManagementBusinessObject;

namespace OnlineEduTaskWPF
{
    public partial class MainWindow : Window
    {
        private UserAccount currentUser;
        public UserAccount CurrentUser => currentUser;

        public MainWindow(UserAccount user)
        {
            InitializeComponent();
            currentUser = user;
            LoadUserInterface();
        }

        private void LoadUserInterface()
        {
            txtUserName.Text = currentUser.Username;
            txtUserRole.Text = currentUser.Role;

            switch (currentUser.Role)
            {
                case "Student":
                    LoadStudentMenu();
                    mainFrame.Navigate(new StudentSubjectsPage());
                    break;
                case "Teacher":
                    LoadTeacherMenu();
                    mainFrame.Navigate(new TeacherClassesPage(currentUser));
                    break;
                case "Admin":
                    LoadAdminMenu();
                    mainFrame.Navigate(new AdminDashboardPage());
                    break;
            }
        }

        private void LoadStudentMenu()
        {
            AddMenuButton("📚 Môn học của tôi", (s, e) => 
                mainFrame.Navigate(new StudentSubjectsPage()));
            AddMenuButton("📝 Bài tập của tôi", (s, e) => 
                mainFrame.Navigate(new StudentTasksPage(currentUser)));
        }

        private void LoadTeacherMenu()
        {
            AddMenuButton("👥 Lớp học của tôi", (s, e) => 
                mainFrame.Navigate(new TeacherClassesPage(currentUser)));
            AddMenuButton("📖 Môn học giảng dạy", (s, e) => 
                mainFrame.Navigate(new TeacherSubjectsPage(currentUser)));
            AddMenuButton("➕ Tạo bài tập mới", (s, e) => 
                mainFrame.Navigate(new CreateTaskPage(currentUser)));
            AddMenuButton("✅ Kiểm tra nộp bài", (s, e) => 
                mainFrame.Navigate(new CheckSubmissionsPage(currentUser)));
        }

        private void LoadAdminMenu()
        {
            AddMenuButton("📊 Dashboard", (s, e) => 
                mainFrame.Navigate(new AdminDashboardPage()));
            AddMenuButton("👨‍🎓 Quản lý học sinh", (s, e) => 
                mainFrame.Navigate(new ManageStudentsPage()));
            AddMenuButton("👨‍🏫 Quản lý giáo viên", (s, e) => 
                mainFrame.Navigate(new ManageTeachersPage()));
            AddMenuButton("🏫 Quản lý lớp học", (s, e) => 
                mainFrame.Navigate(new ManageClassesPage()));
            AddMenuButton("📚 Quản lý môn học", (s, e) => 
                mainFrame.Navigate(new ManageSubjectsPage()));
        }

        private void AddMenuButton(string content, RoutedEventHandler clickHandler)
        {
            var btn = new Button
            {
                Content = content,
                Style = (Style)FindResource("SideMenuButton")
            };
            btn.Click += clickHandler;
            menuPanel.Children.Add(btn);
        }

        private void btnLogout_Click(object sender, RoutedEventArgs e)
        {
            var result = MessageBox.Show("Bạn có chắc muốn đăng xuất?", 
                "Xác nhận", MessageBoxButton.YesNo, MessageBoxImage.Question);
            
            if (result == MessageBoxResult.Yes)
            {
                new LoginWindow().Show();
                this.Close();
            }
        }
    }
}
