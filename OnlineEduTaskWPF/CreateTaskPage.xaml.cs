using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Repositories;
using OnlineEduTaskServices.Services;

namespace OnlineEduTaskWPF
{
    public partial class CreateTaskPage : Page
    {
        private readonly TaskService _taskService;
        private readonly ClassTaskService _classTaskService;
        private readonly ClassSubjectService _classSubjectService;
        private readonly StudentTaskService _studentTaskService;
        private int _currentTeacherId;
        private UserAccount _currentUser;

        public CreateTaskPage() : this(null)
        {
        }

        public CreateTaskPage(UserAccount user)
        {
            InitializeComponent();
            _taskService = new TaskService(new TaskRepository());
            _studentTaskService = new StudentTaskService(
                new StudentTaskRepository(), 
                new ClassTaskRepository(), 
                new StudentRepository());
            _classTaskService = new ClassTaskService(new ClassTaskRepository(), _studentTaskService);
            _classSubjectService = new ClassSubjectService(new ClassSubjectRepository());
            _currentUser = user;
            
            this.Loaded += CreateTaskPage_Loaded;
        }

        private void CreateTaskPage_Loaded(object sender, RoutedEventArgs e)
        {
            LoadCurrentTeacher();
            LoadTeacherClassesAndSubjects();
        }

        private void LoadCurrentTeacher()
        {
            try
            {
                // Get current teacher ID from passed user or MainWindow
                if (_currentUser == null)
                {
                    var mainWindow = Application.Current.MainWindow as MainWindow;
                    if (mainWindow?.CurrentUser?.TeacherId == null)
                    {
                        MessageBox.Show("Unable to get current teacher information", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }
                    _currentUser = mainWindow.CurrentUser;
                }

                if (_currentUser.TeacherId == null)
                {
                    MessageBox.Show("Current user is not a teacher", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                _currentTeacherId = _currentUser.TeacherId.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading teacher info: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void LoadTeacherClassesAndSubjects()
        {
            try
            {
                // Get all ClassSubjects for this teacher
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                var teacherClassSubjects = allClassSubjects
                    .Where(cs => cs.TeacherId == _currentTeacherId)
                    .ToList();

                // Load unique classes
                var classes = teacherClassSubjects
                    .Select(cs => cs.StudentClass)
                    .Where(c => c != null)
                    .GroupBy(c => c.ClassId)
                    .Select(g => g.First())
                    .OrderBy(c => c.ClassCode)
                    .ToList();

                cmbClass.ItemsSource = classes;
                cmbClass.DisplayMemberPath = "ClassName";
                cmbClass.SelectedValuePath = "ClassId";
                cmbClass.SelectionChanged += CmbClass_SelectionChanged;

                // Set default due date to next week
                dpDueDate.SelectedDate = DateTime.Now.AddDays(7);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CmbClass_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            try
            {
                if (cmbClass.SelectedValue == null)
                {
                    cmbSubject.ItemsSource = null;
                    return;
                }

                int classId = (int)cmbClass.SelectedValue;

                // Get subjects for the selected class
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                var subjects = allClassSubjects
                    .Where(cs => cs.ClassId == classId && cs.TeacherId == _currentTeacherId)
                    .Select(cs => cs.Subject)
                    .Where(s => s != null)
                    .OrderBy(s => s.SubjectCode)
                    .ToList();

                cmbSubject.ItemsSource = subjects;
                cmbSubject.DisplayMemberPath = "SubjectName";
                cmbSubject.SelectedValuePath = "SubjectId";
                
                if (subjects.Any())
                    cmbSubject.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error loading subjects: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void CreateTask_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                // Clear previous error
                txtError.Visibility = Visibility.Collapsed;

                // Validate input
                if (string.IsNullOrWhiteSpace(txtTitle.Text))
                {
                    ShowError("Please enter task title");
                    return;
                }

                if (string.IsNullOrWhiteSpace(txtDescription.Text))
                {
                    ShowError("Please enter task description");
                    return;
                }

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

                if (dpDueDate.SelectedDate == null)
                {
                    ShowError("Please select a due date");
                    return;
                }

                // Get hour and minute from spinners
                if (!int.TryParse(txtDueHour.Text, out int hour) || hour < 0 || hour > 23)
                {
                    ShowError("Please enter a valid hour (0-23)");
                    return;
                }

                if (!int.TryParse(txtDueMinute.Text, out int minute) || minute < 0 || minute > 59)
                {
                    ShowError("Please enter a valid minute (0-59)");
                    return;
                }

                int classId = (int)cmbClass.SelectedValue;
                int subjectId = (int)cmbSubject.SelectedValue;
                DateTime dueDate = dpDueDate.SelectedDate.Value.AddHours(hour).AddMinutes(minute);

                if (!double.TryParse(txtMaxScore.Text, out double maxScore) || maxScore <= 0)
                {
                    ShowError("Please enter a valid max score (greater than 0)");
                    return;
                }

                // Verify teacher is assigned to this class-subject combination
                var allClassSubjects = _classSubjectService.GetAllClassSubjects();
                var classSubject = allClassSubjects.FirstOrDefault(cs => 
                    cs.ClassId == classId && 
                    cs.SubjectId == subjectId && 
                    cs.TeacherId == _currentTeacherId);

                if (classSubject == null)
                {
                    ShowError("You are not assigned to teach this subject to this class");
                    return;
                }

                // Step 1: Create Task
                var task = new StudentManagementBusinessObject.Task
                {
                    Title = txtTitle.Text.Trim(),
                    Description = txtDescription.Text.Trim(),
                    CreatedByTeacherId = _currentTeacherId,
                    CreatedDate = DateTime.Now
                };

                _taskService.AddTask(task);

                // Step 2: Create ClassTask (this will auto-create StudentTasks)
                var classTask = new ClassTask
                {
                    TaskId = task.TaskId,
                    ClassId = classId,
                    SubjectId = subjectId,
                    DueDate = dueDate,
                    MaxScore = (int)maxScore
                };

                _classTaskService.AddClassTask(classTask);

                MessageBox.Show(
                    $"Task '{task.Title}' created successfully and assigned to class!\nStudent tasks have been automatically created for all students in the class.", 
                    "Success", 
                    MessageBoxButton.OK, 
                    MessageBoxImage.Information);

                // Clear form
                ClearForm();
            }
            catch (Exception ex)
            {
                ShowError($"Error creating task: {ex.Message}");
            }
        }

        private void ClearForm()
        {
            txtTitle.Clear();
            txtDescription.Clear();
            cmbClass.SelectedIndex = -1;
            cmbSubject.SelectedIndex = -1;
            dpDueDate.SelectedDate = DateTime.Now.AddDays(7);
            txtMaxScore.Text = "10";
            txtDueHour.Text = "23";
            txtDueMinute.Text = "59";
            txtError.Visibility = Visibility.Collapsed;
        }

        private void ShowError(string message)
        {
            txtError.Text = message;
            txtError.Visibility = Visibility.Visible;
        }
    }
}
