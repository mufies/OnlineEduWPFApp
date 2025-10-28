using System.Windows.Controls;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class AdminDashboardPage : Page
    {
        private readonly StudentService _studentService;
        private readonly UserAccountService _userService;
        private readonly StudentClassService _classService;

        public AdminDashboardPage()
        {
            InitializeComponent();
            
            _studentService = new StudentService(new StudentRepository());
            _userService = new UserAccountService(new UserAccountRepository());
            _classService = new StudentClassService(new StudentClassRepository());
            
            LoadStatistics();
        }

        private void LoadStatistics()
        {
            try
            {
                var students = _studentService.GetAllStudents();
                txtTotalStudents.Text = students.Count.ToString();
                
                var teachers = _userService.GetUsersByRole("Teacher");
                txtTotalTeachers.Text = teachers.Count.ToString();
                
                var classes = _classService.GetAllClasses();
                txtTotalClasses.Text = classes.Count.ToString();
            }
            catch
            {
                txtTotalStudents.Text = "0";
                txtTotalTeachers.Text = "0";
                txtTotalClasses.Text = "0";
            }
        }
    }
}
