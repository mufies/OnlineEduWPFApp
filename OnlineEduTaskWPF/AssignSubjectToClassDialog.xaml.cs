using System;
using System.Linq;
using System.Windows;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class AssignSubjectToClassDialog : Window
    {
        private readonly StudentClassService _classService;
        private readonly SubjectService _subjectService;
        private readonly ClassSubjectService _classSubjectService;
        private readonly UserAccountService _userService;

        public bool IsSuccess { get; private set; }

        public AssignSubjectToClassDialog()
        {
            InitializeComponent();
            
            _classService = new StudentClassService(new StudentClassRepository());
            _subjectService = new SubjectService(new SubjectRepository());
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _userService = new UserAccountService(new UserAccountRepository());
            
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                // Load classes
                var classes = _classService.GetAllClasses();
                cmbClass.ItemsSource = classes;

                // Load subjects
                var subjects = _subjectService.GetAllSubjects();
                cmbSubject.ItemsSource = subjects;

                // Load teachers
                var teachers = _userService.GetUsersByRole("Teacher");
                cmbTeacher.ItemsSource = teachers;
            }
            catch (Exception ex)
            {
                ShowError($"Error loading data: {ex.Message}");
            }
        }

        private void btnAssign_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Validate input
                if (cmbClass.SelectedValue == null)
                {
                    ShowError("Please select a class");
                    return;
                }

                if (cmbSubject.SelectedValue == null)
                {
                    ShowError("Please select a subject");
                    return;
                }

                int classId = (int)cmbClass.SelectedValue;
                int subjectId = (int)cmbSubject.SelectedValue;
                int? teacherId = cmbTeacher.SelectedValue as int?;

                // Check if assignment already exists
                var existingAssignments = _classSubjectService.GetClassSubjectsByClassId(classId);
                if (existingAssignments.Any(cs => cs.SubjectId == subjectId))
                {
                    ShowError("This subject is already assigned to this class");
                    return;
                }

                // Create ClassSubject assignment
                var classSubject = new ClassSubject
                {
                    ClassId = classId,
                    SubjectId = subjectId,
                    TeacherId = teacherId
                };

                _classSubjectService.AddClassSubject(classSubject);

                IsSuccess = true;
                MessageBox.Show("Subject assigned to class successfully!", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                this.Close();
            }
            catch (Exception ex)
            {
                ShowError($"Error assigning subject: {ex.Message}");
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
