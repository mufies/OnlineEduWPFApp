using Moq;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;
using Xunit;

namespace OnlineEduTaskTests.ServiceTests;

public class StudentServiceTests
{
    private readonly Mock<IStudentRepository> _mockRepository;
    private readonly StudentService _service;

    public StudentServiceTests()
    {
        _mockRepository = new Mock<IStudentRepository>();
        _service = new StudentService(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new StudentService(null));
    }

    [Fact]
    public void GetAllStudents_ReturnsAllStudents()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentCode = "ST001", FullName = "Student 1", Email = "s1@test.com", ClassId = 1 },
            new Student { StudentId = 2, StudentCode = "ST002", FullName = "Student 2", Email = "s2@test.com", ClassId = 1 }
        };
        _mockRepository.Setup(r => r.GetAllStudents()).Returns(students);

        // Act
        var result = _service.GetAllStudents();

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetAllStudents(), Times.Once);
    }

    [Fact]
    public void GetStudentById_ReturnsStudent_WhenValidId()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "Test Student", Email = "test@test.com", ClassId = 1 };
        _mockRepository.Setup(r => r.GetStudentById(1)).Returns(student);

        // Act
        var result = _service.GetStudentById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Student", result.FullName);
        _mockRepository.Verify(r => r.GetStudentById(1), Times.Once);
    }

    [Fact]
    public void GetStudentById_ThrowsException_WhenIdIsZeroOrNegative()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetStudentById(0));
        Assert.Throws<ArgumentException>(() => _service.GetStudentById(-1));
    }

    [Fact]
    public void GetStudentByCode_ReturnsStudent_WhenValidCode()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "Test Student", Email = "test@test.com", ClassId = 1 };
        _mockRepository.Setup(r => r.GetStudentByCode("ST001")).Returns(student);

        // Act
        var result = _service.GetStudentByCode("ST001");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("ST001", result.StudentCode);
        _mockRepository.Verify(r => r.GetStudentByCode("ST001"), Times.Once);
    }

    [Fact]
    public void GetStudentByCode_ThrowsException_WhenCodeIsNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetStudentByCode(null));
        Assert.Throws<ArgumentException>(() => _service.GetStudentByCode(""));
        Assert.Throws<ArgumentException>(() => _service.GetStudentByCode("   "));
    }

    [Fact]
    public void GetStudentByEmail_ReturnsStudent_WhenValidEmail()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "Test", Email = "test@test.com", ClassId = 1 };
        _mockRepository.Setup(r => r.GetStudentByEmail("test@test.com")).Returns(student);

        // Act
        var result = _service.GetStudentByEmail("test@test.com");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("test@test.com", result.Email);
        _mockRepository.Verify(r => r.GetStudentByEmail("test@test.com"), Times.Once);
    }

    [Fact]
    public void AddStudent_CallsRepository_WhenValidStudent()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "New Student", Email = "new@test.com", ClassId = 1 };

        // Act
        _service.AddStudent(student);

        // Assert
        _mockRepository.Verify(r => r.AddStudent(student), Times.Once);
    }

    [Fact]
    public void AddStudent_ThrowsException_WhenStudentIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.AddStudent(null));
    }

    [Fact]
    public void AddStudent_ThrowsException_WhenStudentCodeIsEmpty()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "", FullName = "Test", Email = "test@test.com", ClassId = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddStudent(student));
    }

    [Fact]
    public void AddStudent_ThrowsException_WhenFullNameIsEmpty()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "", Email = "test@test.com", ClassId = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddStudent(student));
    }

    [Fact]
    public void AddStudent_ThrowsException_WhenEmailIsEmpty()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "Test", Email = "", ClassId = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddStudent(student));
    }

    [Fact]
    public void UpdateStudent_CallsRepository_WhenValidStudent()
    {
        // Arrange
        var student = new Student { StudentId = 1, StudentCode = "ST001", FullName = "Updated", Email = "updated@test.com", ClassId = 1 };

        // Act
        _service.UpdateStudent(student);

        // Assert
        _mockRepository.Verify(r => r.UpdateStudent(student), Times.Once);
    }

    [Fact]
    public void UpdateStudent_ThrowsException_WhenStudentIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.UpdateStudent(null));
    }

    [Fact]
    public void UpdateStudent_ThrowsException_WhenStudentIdIsInvalid()
    {
        // Arrange
        var student = new Student { StudentId = 0, StudentCode = "ST001", FullName = "Test", Email = "test@test.com", ClassId = 1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.UpdateStudent(student));
    }

    [Fact]
    public void DeleteStudent_CallsRepository_WhenValidId()
    {
        // Act
        _service.DeleteStudent(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteStudent(1), Times.Once);
    }

    [Fact]
    public void DeleteStudent_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.DeleteStudent(0));
        Assert.Throws<ArgumentException>(() => _service.DeleteStudent(-1));
    }

    [Fact]
    public void StudentExists_ReturnsTrue_WhenStudentExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.StudentExists(1)).Returns(true);

        // Act
        var result = _service.StudentExists(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.StudentExists(1), Times.Once);
    }

    [Fact]
    public void StudentExists_ReturnsFalse_WhenStudentDoesNotExist()
    {
        // Arrange
        _mockRepository.Setup(r => r.StudentExists(999)).Returns(false);

        // Act
        var result = _service.StudentExists(999);

        // Assert
        Assert.False(result);
        _mockRepository.Verify(r => r.StudentExists(999), Times.Once);
    }

    [Fact]
    public void GetTotalStudentCount_ReturnsCorrectCount()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentCode = "ST001", FullName = "S1", Email = "s1@test.com", ClassId = 1 },
            new Student { StudentId = 2, StudentCode = "ST002", FullName = "S2", Email = "s2@test.com", ClassId = 1 },
            new Student { StudentId = 3, StudentCode = "ST003", FullName = "S3", Email = "s3@test.com", ClassId = 1 }
        };
        _mockRepository.Setup(r => r.GetAllStudents()).Returns(students);

        // Act
        var result = _service.GetTotalStudentCount();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetStudentsByClassId_ReturnsStudents_WhenValidClassId()
    {
        // Arrange
        var students = new List<Student>
        {
            new Student { StudentId = 1, StudentCode = "ST001", FullName = "S1", Email = "s1@test.com", ClassId = 1 },
            new Student { StudentId = 2, StudentCode = "ST002", FullName = "S2", Email = "s2@test.com", ClassId = 1 }
        };
        _mockRepository.Setup(r => r.GetStudentsByClassId(1)).Returns(students);

        // Act
        var result = _service.GetStudentsByClassId(1);

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetStudentsByClassId(1), Times.Once);
    }

    [Fact]
    public void GetStudentsByClassId_ThrowsException_WhenClassIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetStudentsByClassId(0));
        Assert.Throws<ArgumentException>(() => _service.GetStudentsByClassId(-1));
    }

    [Fact]
    public void StudentCodeExists_ReturnsTrue_WhenCodeExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.StudentCodeExists("ST001")).Returns(true);

        // Act
        var result = _service.StudentCodeExists("ST001");

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.StudentCodeExists("ST001"), Times.Once);
    }

    [Fact]
    public void StudentCodeExists_ThrowsException_WhenCodeIsNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.StudentCodeExists(null));
        Assert.Throws<ArgumentException>(() => _service.StudentCodeExists(""));
    }
}
