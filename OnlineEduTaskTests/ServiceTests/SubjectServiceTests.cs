using Moq;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;
using Xunit;

namespace OnlineEduTaskTests.ServiceTests;

public class SubjectServiceTests
{
    private readonly Mock<ISubjectRepository> _mockRepository;
    private readonly SubjectService _service;

    public SubjectServiceTests()
    {
        _mockRepository = new Mock<ISubjectRepository>();
        _service = new SubjectService(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new SubjectService(null));
    }

    [Fact]
    public void GetAllSubjects_ReturnsAllSubjects()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 },
            new Subject { SubjectId = 2, SubjectCode = "CS102", SubjectName = "Database", Credits = 4 }
        };
        _mockRepository.Setup(r => r.GetAllSubjects()).Returns(subjects);

        // Act
        var result = _service.GetAllSubjects();

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetAllSubjects(), Times.Once);
    }

    [Fact]
    public void GetSubjectById_ReturnsSubject_WhenValidId()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 };
        _mockRepository.Setup(r => r.GetSubjectById(1)).Returns(subject);

        // Act
        var result = _service.GetSubjectById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Programming", result.SubjectName);
        _mockRepository.Verify(r => r.GetSubjectById(1), Times.Once);
    }

    [Fact]
    public void GetSubjectById_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetSubjectById(0));
        Assert.Throws<ArgumentException>(() => _service.GetSubjectById(-1));
    }

    [Fact]
    public void GetSubjectByCode_ReturnsSubject_WhenValidCode()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 };
        _mockRepository.Setup(r => r.GetSubjectByCode("CS101")).Returns(subject);

        // Act
        var result = _service.GetSubjectByCode("CS101");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("CS101", result.SubjectCode);
        _mockRepository.Verify(r => r.GetSubjectByCode("CS101"), Times.Once);
    }

    [Fact]
    public void GetSubjectByCode_ThrowsException_WhenCodeIsNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetSubjectByCode(null));
        Assert.Throws<ArgumentException>(() => _service.GetSubjectByCode(""));
        Assert.Throws<ArgumentException>(() => _service.GetSubjectByCode("   "));
    }

    [Fact]
    public void GetSubjectsByClassId_ReturnsSubjects_WhenValidClassId()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 },
            new Subject { SubjectId = 2, SubjectCode = "CS102", SubjectName = "Database", Credits = 4 }
        };
        _mockRepository.Setup(r => r.GetSubjectsByClassId(1)).Returns(subjects);

        // Act
        var result = _service.GetSubjectsByClassId(1);

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetSubjectsByClassId(1), Times.Once);
    }

    [Fact]
    public void GetSubjectsByClassId_ThrowsException_WhenClassIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetSubjectsByClassId(0));
        Assert.Throws<ArgumentException>(() => _service.GetSubjectsByClassId(-1));
    }

    [Fact]
    public void AddSubject_CallsRepository_WhenValidSubject()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 };

        // Act
        _service.AddSubject(subject);

        // Assert
        _mockRepository.Verify(r => r.AddSubject(subject), Times.Once);
    }

    [Fact]
    public void AddSubject_ThrowsException_WhenSubjectIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.AddSubject(null));
    }

    [Fact]
    public void AddSubject_ThrowsException_WhenSubjectCodeIsEmpty()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "", SubjectName = "Programming", Credits = 3 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddSubject(subject));
    }

    [Fact]
    public void AddSubject_ThrowsException_WhenSubjectNameIsEmpty()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "", Credits = 3 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddSubject(subject));
    }

    [Fact]
    public void AddSubject_ThrowsException_WhenCreditsIsZeroOrNegative()
    {
        // Arrange
        var subject1 = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 0 };
        var subject2 = new Subject { SubjectId = 2, SubjectCode = "CS102", SubjectName = "Database", Credits = -1 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddSubject(subject1));
        Assert.Throws<ArgumentException>(() => _service.AddSubject(subject2));
    }

    [Fact]
    public void UpdateSubject_CallsRepository_WhenValidSubject()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Updated", Credits = 4 };

        // Act
        _service.UpdateSubject(subject);

        // Assert
        _mockRepository.Verify(r => r.UpdateSubject(subject), Times.Once);
    }

    [Fact]
    public void UpdateSubject_ThrowsException_WhenSubjectIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.UpdateSubject(null));
    }

    [Fact]
    public void UpdateSubject_ThrowsException_WhenSubjectIdIsInvalid()
    {
        // Arrange
        var subject = new Subject { SubjectId = 0, SubjectCode = "CS101", SubjectName = "Programming", Credits = 3 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.UpdateSubject(subject));
    }

    [Fact]
    public void UpdateSubject_ThrowsException_WhenCreditsIsInvalid()
    {
        // Arrange
        var subject = new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "Programming", Credits = 0 };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.UpdateSubject(subject));
    }

    [Fact]
    public void DeleteSubject_CallsRepository_WhenValidId()
    {
        // Act
        _service.DeleteSubject(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteSubject(1), Times.Once);
    }

    [Fact]
    public void DeleteSubject_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.DeleteSubject(0));
        Assert.Throws<ArgumentException>(() => _service.DeleteSubject(-1));
    }

    [Fact]
    public void SubjectExists_ReturnsTrue_WhenSubjectExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.SubjectExists(1)).Returns(true);

        // Act
        var result = _service.SubjectExists(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.SubjectExists(1), Times.Once);
    }

    [Fact]
    public void SubjectExists_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.SubjectExists(0));
    }

    [Fact]
    public void SubjectCodeExists_ReturnsTrue_WhenCodeExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.SubjectCodeExists("CS101")).Returns(true);

        // Act
        var result = _service.SubjectCodeExists("CS101");

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.SubjectCodeExists("CS101"), Times.Once);
    }

    [Fact]
    public void SubjectCodeExists_ThrowsException_WhenCodeIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.SubjectCodeExists(""));
    }

    [Fact]
    public void GetTotalSubjectCount_ReturnsCorrectCount()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "P1", Credits = 3 },
            new Subject { SubjectId = 2, SubjectCode = "CS102", SubjectName = "P2", Credits = 4 },
            new Subject { SubjectId = 3, SubjectCode = "CS103", SubjectName = "P3", Credits = 2 }
        };
        _mockRepository.Setup(r => r.GetAllSubjects()).Returns(subjects);

        // Act
        var result = _service.GetTotalSubjectCount();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetTotalCredits_ReturnsCorrectSum()
    {
        // Arrange
        var subjects = new List<Subject>
        {
            new Subject { SubjectId = 1, SubjectCode = "CS101", SubjectName = "P1", Credits = 3 },
            new Subject { SubjectId = 2, SubjectCode = "CS102", SubjectName = "P2", Credits = 4 },
            new Subject { SubjectId = 3, SubjectCode = "CS103", SubjectName = "P3", Credits = 2 }
        };
        _mockRepository.Setup(r => r.GetAllSubjects()).Returns(subjects);

        // Act
        var result = _service.GetTotalCredits();

        // Assert
        Assert.Equal(9, result); // 3 + 4 + 2
    }
}
