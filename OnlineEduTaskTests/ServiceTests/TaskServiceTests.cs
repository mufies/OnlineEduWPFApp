using Moq;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;
using Xunit;
using TaskEntity = StudentManagementBusinessObject.Task;

namespace OnlineEduTaskTests.ServiceTests;

public class TaskServiceTests
{
    private readonly Mock<ITaskRepository> _mockRepository;
    private readonly TaskService _service;

    public TaskServiceTests()
    {
        _mockRepository = new Mock<ITaskRepository>();
        _service = new TaskService(_mockRepository.Object);
    }

    [Fact]
    public void GetAllTasks_ReturnsAllTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new TaskEntity { TaskId = 1, Title = "Task 1", Description = "Desc 1", CreatedByTeacherId = 1 },
            new TaskEntity { TaskId = 2, Title = "Task 2", Description = "Desc 2", CreatedByTeacherId = 1 }
        };
        _mockRepository.Setup(r => r.GetAllTasks()).Returns(tasks);

        // Act
        var result = _service.GetAllTasks();

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetAllTasks(), Times.Once);
    }

    [Fact]
    public void GetTaskById_ReturnsTask()
    {
        // Arrange
        var task = new TaskEntity { TaskId = 1, Title = "Test Task", Description = "Test Desc", CreatedByTeacherId = 1 };
        _mockRepository.Setup(r => r.GetTaskById(1)).Returns(task);

        // Act
        var result = _service.GetTaskById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Task", result.Title);
        _mockRepository.Verify(r => r.GetTaskById(1), Times.Once);
    }

    [Fact]
    public void GetTasksByTeacherId_ReturnsTasksForTeacher()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new TaskEntity { TaskId = 1, Title = "Task 1", CreatedByTeacherId = 5 },
            new TaskEntity { TaskId = 2, Title = "Task 2", CreatedByTeacherId = 5 }
        };
        _mockRepository.Setup(r => r.GetTasksByTeacherId(5)).Returns(tasks);

        // Act
        var result = _service.GetTasksByTeacherId(5);

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetTasksByTeacherId(5), Times.Once);
    }

    [Fact]
    public void GetRecentTasks_ReturnsRecentTasks()
    {
        // Arrange
        var tasks = new List<TaskEntity>
        {
            new TaskEntity { TaskId = 1, Title = "Recent Task 1" },
            new TaskEntity { TaskId = 2, Title = "Recent Task 2" }
        };
        _mockRepository.Setup(r => r.GetRecentTasks(10)).Returns(tasks);

        // Act
        var result = _service.GetRecentTasks(10);

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetRecentTasks(10), Times.Once);
    }

    [Fact]
    public void AddTask_SetsCreatedDateAndCallsRepository()
    {
        // Arrange
        var task = new TaskEntity { TaskId = 1, Title = "New Task", Description = "New Desc", CreatedByTeacherId = 1 };
        var beforeAdd = DateTime.Now.AddSeconds(-1);

        // Act
        _service.AddTask(task);

        // Assert
        Assert.True(task.CreatedDate >= beforeAdd);
        Assert.True(task.CreatedDate <= DateTime.Now.AddSeconds(1));
        _mockRepository.Verify(r => r.AddTask(task), Times.Once);
    }

    [Fact]
    public void AddTask_ThrowsException_WhenTaskIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.AddTask(null));
    }

    [Fact]
    public void UpdateTask_CallsRepository()
    {
        // Arrange
        var task = new TaskEntity { TaskId = 1, Title = "Updated Task", Description = "Updated", CreatedByTeacherId = 1 };

        // Act
        _service.UpdateTask(task);

        // Assert
        _mockRepository.Verify(r => r.UpdateTask(task), Times.Once);
    }

    [Fact]
    public void DeleteTask_CallsRepository()
    {
        // Act
        _service.DeleteTask(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteTask(1), Times.Once);
    }

    [Fact]
    public void TaskExists_ReturnsTrue_WhenTaskExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.TaskExists(1)).Returns(true);

        // Act
        var result = _service.TaskExists(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.TaskExists(1), Times.Once);
    }

    [Fact]
    public void TaskExists_ReturnsFalse_WhenTaskDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.TaskExists(999)).Returns(false);

        // Act
        var result = _service.TaskExists(999);

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetTotalCount_ReturnsCorrectCount()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetTotalCount()).Returns(25);

        // Act
        var result = _service.GetTotalCount();

        // Assert
        Assert.Equal(25, result);
        _mockRepository.Verify(r => r.GetTotalCount(), Times.Once);
    }

    [Fact]
    public void GetTaskCountByTeacher_ReturnsCorrectCount()
    {
        // Arrange
        _mockRepository.Setup(r => r.GetTaskCountByTeacher(5)).Returns(10);

        // Act
        var result = _service.GetTaskCountByTeacher(5);

        // Assert
        Assert.Equal(10, result);
        _mockRepository.Verify(r => r.GetTaskCountByTeacher(5), Times.Once);
    }
}
