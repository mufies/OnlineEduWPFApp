using Moq;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;
using Xunit;

namespace OnlineEduTaskTests.ServiceTests;

public class StudentTaskServiceTests
{
    private readonly Mock<IStudentTaskRepository> _mockStudentTaskRepo;
    private readonly Mock<IClassTaskRepository> _mockClassTaskRepo;
    private readonly Mock<IStudentRepository> _mockStudentRepo;
    private readonly StudentTaskService _service;

    public StudentTaskServiceTests()
    {
        _mockStudentTaskRepo = new Mock<IStudentTaskRepository>();
        _mockClassTaskRepo = new Mock<IClassTaskRepository>();
        _mockStudentRepo = new Mock<IStudentRepository>();
        _service = new StudentTaskService(
            _mockStudentTaskRepo.Object,
            _mockClassTaskRepo.Object,
            _mockStudentRepo.Object
        );
    }

    [Fact]
    public void GetAllStudentTasks_ReturnsAllTasks()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, StudentId = 1, ClassTaskId = 1, IsSubmitted = false },
            new StudentTask { StudentTaskId = 2, StudentId = 2, ClassTaskId = 1, IsSubmitted = true }
        };
        _mockStudentTaskRepo.Setup(r => r.GetAllStudentTasks()).Returns(tasks);

        // Act
        var result = _service.GetAllStudentTasks();

        // Assert
        Assert.Equal(2, result.Count);
        _mockStudentTaskRepo.Verify(r => r.GetAllStudentTasks(), Times.Once);
    }

    [Fact]
    public void GetStudentTaskById_ReturnsTask()
    {
        // Arrange
        var task = new StudentTask { StudentTaskId = 1, StudentId = 1, ClassTaskId = 1, IsSubmitted = false };
        _mockStudentTaskRepo.Setup(r => r.GetStudentTaskById(1)).Returns(task);

        // Act
        var result = _service.GetStudentTaskById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.StudentTaskId);
        _mockStudentTaskRepo.Verify(r => r.GetStudentTaskById(1), Times.Once);
    }

    [Fact]
    public void GetStudentTasksByStudentId_ReturnsTasksForStudent()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, StudentId = 5, ClassTaskId = 1 },
            new StudentTask { StudentTaskId = 2, StudentId = 5, ClassTaskId = 2 }
        };
        _mockStudentTaskRepo.Setup(r => r.GetStudentTasksByStudentId(5)).Returns(tasks);

        // Act
        var result = _service.GetStudentTasksByStudentId(5);

        // Assert
        Assert.Equal(2, result.Count);
        _mockStudentTaskRepo.Verify(r => r.GetStudentTasksByStudentId(5), Times.Once);
    }

    [Fact]
    public void GetStudentTasksByClassTaskId_ReturnsTasksForClass()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, StudentId = 1, ClassTaskId = 10 },
            new StudentTask { StudentTaskId = 2, StudentId = 2, ClassTaskId = 10 }
        };
        _mockStudentTaskRepo.Setup(r => r.GetStudentTasksByClassTaskId(10)).Returns(tasks);

        // Act
        var result = _service.GetStudentTasksByClassTaskId(10);

        // Assert
        Assert.Equal(2, result.Count);
        _mockStudentTaskRepo.Verify(r => r.GetStudentTasksByClassTaskId(10), Times.Once);
    }

    [Fact]
    public void GetSubmittedStudentTasks_ReturnsSubmittedTasks()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, IsSubmitted = true },
            new StudentTask { StudentTaskId = 2, IsSubmitted = true }
        };
        _mockStudentTaskRepo.Setup(r => r.GetSubmittedStudentTasks()).Returns(tasks);

        // Act
        var result = _service.GetSubmittedStudentTasks();

        // Assert
        Assert.Equal(2, result.Count);
        _mockStudentTaskRepo.Verify(r => r.GetSubmittedStudentTasks(), Times.Once);
    }

    [Fact]
    public void GetPendingStudentTasks_ReturnsPendingTasks()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, IsSubmitted = false }
        };
        _mockStudentTaskRepo.Setup(r => r.GetPendingStudentTasks()).Returns(tasks);

        // Act
        var result = _service.GetPendingStudentTasks();

        // Assert
        Assert.Single(result);
        _mockStudentTaskRepo.Verify(r => r.GetPendingStudentTasks(), Times.Once);
    }

    [Fact]
    public void GetGradedStudentTasks_ReturnsGradedTasks()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, Score = 85, IsSubmitted = true }
        };
        _mockStudentTaskRepo.Setup(r => r.GetGradedStudentTasks()).Returns(tasks);

        // Act
        var result = _service.GetGradedStudentTasks();

        // Assert
        Assert.Single(result);
        _mockStudentTaskRepo.Verify(r => r.GetGradedStudentTasks(), Times.Once);
    }

    [Fact]
    public void GetUngradedStudentTasks_ReturnsUngradedTasks()
    {
        // Arrange
        var tasks = new List<StudentTask>
        {
            new StudentTask { StudentTaskId = 1, Score = null, IsSubmitted = true }
        };
        _mockStudentTaskRepo.Setup(r => r.GetUngradedStudentTasks()).Returns(tasks);

        // Act
        var result = _service.GetUngradedStudentTasks();

        // Assert
        Assert.Single(result);
        _mockStudentTaskRepo.Verify(r => r.GetUngradedStudentTasks(), Times.Once);
    }

    [Fact]
    public void AddStudentTask_SetsIsSubmittedToFalseAndCallsRepository()
    {
        // Arrange
        var task = new StudentTask { StudentTaskId = 1, StudentId = 1, ClassTaskId = 1, IsSubmitted = true };

        // Act
        _service.AddStudentTask(task);

        // Assert
        Assert.False(task.IsSubmitted);
        _mockStudentTaskRepo.Verify(r => r.AddStudentTask(task), Times.Once);
    }

    [Fact]
    public void AddStudentTask_ThrowsException_WhenTaskIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.AddStudentTask(null));
    }

    [Fact]
    public void UpdateStudentTask_CallsRepository()
    {
        // Arrange
        var task = new StudentTask { StudentTaskId = 1, StudentId = 1, ClassTaskId = 1 };

        // Act
        _service.UpdateStudentTask(task);

        // Assert
        _mockStudentTaskRepo.Verify(r => r.UpdateStudentTask(task), Times.Once);
    }

    [Fact]
    public void DeleteStudentTask_CallsRepository()
    {
        // Act
        _service.DeleteStudentTask(1);

        // Assert
        _mockStudentTaskRepo.Verify(r => r.DeleteStudentTask(1), Times.Once);
    }

    [Fact]
    public void SubmitStudentTask_CallsRepository()
    {
        // Act
        _service.SubmitStudentTask(1, "My submission content", "path/to/file.pdf");

        // Assert
        _mockStudentTaskRepo.Verify(r => r.SubmitStudentTask(1, "My submission content", "path/to/file.pdf"), Times.Once);
    }

    [Fact]
    public void SubmitStudentTask_CallsRepositoryWithoutFilePath()
    {
        // Act
        _service.SubmitStudentTask(1, "My submission content");

        // Assert
        _mockStudentTaskRepo.Verify(r => r.SubmitStudentTask(1, "My submission content", null), Times.Once);
    }

    [Fact]
    public void GradeStudentTask_CallsRepository()
    {
        // Act
        _service.GradeStudentTask(1, 85, "Good work!");

        // Assert
        _mockStudentTaskRepo.Verify(r => r.GradeStudentTask(1, 85, "Good work!"), Times.Once);
    }

    [Fact]
    public void GradeStudentTask_CallsRepositoryWithoutFeedback()
    {
        // Act
        _service.GradeStudentTask(1, 90);

        // Assert
        _mockStudentTaskRepo.Verify(r => r.GradeStudentTask(1, 90, null), Times.Once);
    }

    [Fact]
    public void CreateStudentTasksForClass_CreatesTasksForAllStudents()
    {
        // Arrange
        var classTask = new ClassTask { ClassTaskId = 1, ClassId = 5, TaskId = 10 };
        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentCode = "ST001", FullName = "Student 1", Email = "s1@test.com", ClassId = 5 },
            new Student { StudentId = 2, StudentCode = "ST002", FullName = "Student 2", Email = "s2@test.com", ClassId = 5 }
        };

        _mockClassTaskRepo.Setup(r => r.GetClassTaskById(1)).Returns(classTask);
        _mockStudentRepo.Setup(r => r.GetStudentsByClassId(5)).Returns(students);
        _mockStudentTaskRepo.Setup(r => r.HasSubmission(1, It.IsAny<int>())).Returns(false);

        // Act
        _service.CreateStudentTasksForClass(1);

        // Assert
        _mockStudentTaskRepo.Verify(r => r.AddStudentTask(It.Is<StudentTask>(st => 
            st.ClassTaskId == 1 && st.IsSubmitted == false)), Times.Exactly(2));
    }

    [Fact]
    public void CreateStudentTasksForClass_ThrowsException_WhenClassTaskNotFound()
    {
        // Arrange
        _mockClassTaskRepo.Setup(r => r.GetClassTaskById(999)).Returns((ClassTask)null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.CreateStudentTasksForClass(999));
    }

    [Fact]
    public void CreateStudentTasksForClass_SkipsStudentsWhoAlreadyHaveTask()
    {
        // Arrange
        var classTask = new ClassTask { ClassTaskId = 1, ClassId = 5, TaskId = 10 };
        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentCode = "ST001", FullName = "Student 1", Email = "s1@test.com", ClassId = 5 },
            new Student { StudentId = 2, StudentCode = "ST002", FullName = "Student 2", Email = "s2@test.com", ClassId = 5 }
        };

        _mockClassTaskRepo.Setup(r => r.GetClassTaskById(1)).Returns(classTask);
        _mockStudentRepo.Setup(r => r.GetStudentsByClassId(5)).Returns(students);
        _mockStudentTaskRepo.Setup(r => r.HasSubmission(1, 1)).Returns(true); // Student 1 already has task
        _mockStudentTaskRepo.Setup(r => r.HasSubmission(1, 2)).Returns(false);

        // Act
        _service.CreateStudentTasksForClass(1);

        // Assert
        _mockStudentTaskRepo.Verify(r => r.AddStudentTask(It.IsAny<StudentTask>()), Times.Once); // Only one added
    }

    [Fact]
    public void GetTotalCount_ReturnsCorrectCount()
    {
        // Arrange
        _mockStudentTaskRepo.Setup(r => r.GetTotalCount()).Returns(50);

        // Act
        var result = _service.GetTotalCount();

        // Assert
        Assert.Equal(50, result);
        _mockStudentTaskRepo.Verify(r => r.GetTotalCount(), Times.Once);
    }

    [Fact]
    public void GetSubmissionRate_ReturnsCorrectRate()
    {
        // Arrange
        _mockStudentTaskRepo.Setup(r => r.GetSubmissionRate(1)).Returns(0.85);

        // Act
        var result = _service.GetSubmissionRate(1);

        // Assert
        Assert.Equal(0.85, result);
        _mockStudentTaskRepo.Verify(r => r.GetSubmissionRate(1), Times.Once);
    }

    [Fact]
    public void StudentTaskExists_ReturnsTrue_WhenTaskExists()
    {
        // Arrange
        _mockStudentTaskRepo.Setup(r => r.StudentTaskExists(1)).Returns(true);

        // Act
        var result = _service.StudentTaskExists(1);

        // Assert
        Assert.True(result);
        _mockStudentTaskRepo.Verify(r => r.StudentTaskExists(1), Times.Once);
    }

    [Fact]
    public void HasSubmission_ReturnsTrue_WhenSubmissionExists()
    {
        // Arrange
        _mockStudentTaskRepo.Setup(r => r.HasSubmission(1, 5)).Returns(true);

        // Act
        var result = _service.HasSubmission(1, 5);

        // Assert
        Assert.True(result);
        _mockStudentTaskRepo.Verify(r => r.HasSubmission(1, 5), Times.Once);
    }
}
