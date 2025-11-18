using Moq;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Services;
using StudentManagementBusinessObject;
using Xunit;

namespace OnlineEduTaskTests.ServiceTests;

public class UserAccountServiceTests
{
    private readonly Mock<IUserAccountRepository> _mockRepository;
    private readonly UserAccountService _service;

    public UserAccountServiceTests()
    {
        _mockRepository = new Mock<IUserAccountRepository>();
        _service = new UserAccountService(_mockRepository.Object);
    }

    [Fact]
    public void Constructor_ThrowsException_WhenRepositoryIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => new UserAccountService(null));
    }

    [Fact]
    public void GetAllUsers_ReturnsAllUsers()
    {
        // Arrange
        var users = new List<UserAccount>
        {
            new UserAccount { UserAccountId = 1, Username = "user1", Password = "pass1", Role = "Student" },
            new UserAccount { UserAccountId = 2, Username = "user2", Password = "pass2", Role = "Teacher" }
        };
        _mockRepository.Setup(r => r.GetAllUsers()).Returns(users);

        // Act
        var result = _service.GetAllUsers();

        // Assert
        Assert.Equal(2, result.Count);
        _mockRepository.Verify(r => r.GetAllUsers(), Times.Once);
    }

    [Fact]
    public void GetUserById_ReturnsUser_WhenValidId()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "testuser", Password = "pass", Role = "Admin" };
        _mockRepository.Setup(r => r.GetUserById(1)).Returns(user);

        // Act
        var result = _service.GetUserById(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.GetUserById(1), Times.Once);
    }

    [Fact]
    public void GetUserById_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetUserById(0));
        Assert.Throws<ArgumentException>(() => _service.GetUserById(-1));
    }

    [Fact]
    public void GetUserByUsername_ReturnsUser_WhenValidUsername()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "testuser", Password = "pass", Role = "Student" };
        _mockRepository.Setup(r => r.GetUserByUsername("testuser")).Returns(user);

        // Act
        var result = _service.GetUserByUsername("testuser");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.GetUserByUsername("testuser"), Times.Once);
    }

    [Fact]
    public void GetUserByUsername_ThrowsException_WhenUsernameIsNullOrEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetUserByUsername(null));
        Assert.Throws<ArgumentException>(() => _service.GetUserByUsername(""));
        Assert.Throws<ArgumentException>(() => _service.GetUserByUsername("   "));
    }

    [Fact]
    public void Login_ReturnsUser_WhenCredentialsAreValid()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "testuser", Password = "password123", Role = "Student" };
        _mockRepository.Setup(r => r.Login("testuser", "password123")).Returns(user);

        // Act
        var result = _service.Login("testuser", "password123");

        // Assert
        Assert.NotNull(result);
        Assert.Equal("testuser", result.Username);
        _mockRepository.Verify(r => r.Login("testuser", "password123"), Times.Once);
    }

    [Fact]
    public void Login_ThrowsException_WhenUsernameIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Login("", "password"));
    }

    [Fact]
    public void Login_ThrowsException_WhenPasswordIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.Login("user", ""));
    }

    [Fact]
    public void Login_ThrowsException_WhenCredentialsAreInvalid()
    {
        // Arrange
        _mockRepository.Setup(r => r.Login("wronguser", "wrongpass")).Returns((UserAccount)null);

        // Act & Assert
        Assert.Throws<InvalidOperationException>(() => _service.Login("wronguser", "wrongpass"));
    }

    [Fact]
    public void ValidateCredentials_ReturnsTrue_WhenCredentialsAreValid()
    {
        // Arrange
        _mockRepository.Setup(r => r.ValidateCredentials("user", "pass")).Returns(true);

        // Act
        var result = _service.ValidateCredentials("user", "pass");

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.ValidateCredentials("user", "pass"), Times.Once);
    }

    [Fact]
    public void ValidateCredentials_ReturnsFalse_WhenCredentialsAreInvalid()
    {
        // Arrange
        _mockRepository.Setup(r => r.ValidateCredentials("user", "wrongpass")).Returns(false);

        // Act
        var result = _service.ValidateCredentials("user", "wrongpass");

        // Assert
        Assert.False(result);
    }

    [Fact]
    public void GetUsersByRole_ReturnsFilteredUsers()
    {
        // Arrange
        var students = new List<UserAccount>
        {
            new UserAccount { UserAccountId = 1, Username = "student1", Password = "pass1", Role = "Student" },
            new UserAccount { UserAccountId = 2, Username = "student2", Password = "pass2", Role = "Student" }
        };
        _mockRepository.Setup(r => r.GetUsersByRole("Student")).Returns(students);

        // Act
        var result = _service.GetUsersByRole("Student");

        // Assert
        Assert.Equal(2, result.Count);
        Assert.All(result, u => Assert.Equal("Student", u.Role));
        _mockRepository.Verify(r => r.GetUsersByRole("Student"), Times.Once);
    }

    [Fact]
    public void GetUsersByRole_ThrowsException_WhenRoleIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.GetUsersByRole(""));
    }

    [Fact]
    public void AddUser_CallsRepository_WhenValidUser()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "newuser", Password = "newpass", Role = "Student" };

        // Act
        _service.AddUser(user);

        // Assert
        _mockRepository.Verify(r => r.AddUser(user), Times.Once);
    }

    [Fact]
    public void AddUser_ThrowsException_WhenUserIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.AddUser(null));
    }

    [Fact]
    public void AddUser_ThrowsException_WhenUsernameIsEmpty()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "", Password = "pass", Role = "Student" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddUser(user));
    }

    [Fact]
    public void AddUser_ThrowsException_WhenPasswordIsEmpty()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "user", Password = "", Role = "Student" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddUser(user));
    }

    [Fact]
    public void AddUser_ThrowsException_WhenRoleIsEmpty()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "user", Password = "pass", Role = "" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.AddUser(user));
    }

    [Fact]
    public void UpdateUser_CallsRepository_WhenValidUser()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 1, Username = "updated", Password = "pass", Role = "Teacher" };

        // Act
        _service.UpdateUser(user);

        // Assert
        _mockRepository.Verify(r => r.UpdateUser(user), Times.Once);
    }

    [Fact]
    public void UpdateUser_ThrowsException_WhenUserIsNull()
    {
        // Act & Assert
        Assert.Throws<ArgumentNullException>(() => _service.UpdateUser(null));
    }

    [Fact]
    public void UpdateUser_ThrowsException_WhenUserIdIsInvalid()
    {
        // Arrange
        var user = new UserAccount { UserAccountId = 0, Username = "user", Password = "pass", Role = "Student" };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.UpdateUser(user));
    }

    [Fact]
    public void DeleteUser_CallsRepository_WhenValidId()
    {
        // Act
        _service.DeleteUser(1);

        // Assert
        _mockRepository.Verify(r => r.DeleteUser(1), Times.Once);
    }

    [Fact]
    public void DeleteUser_ThrowsException_WhenIdIsInvalid()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.DeleteUser(0));
    }

    [Fact]
    public void ChangePassword_CallsRepository_WhenValid()
    {
        // Act
        _service.ChangePassword(1, "newpassword");

        // Assert
        _mockRepository.Verify(r => r.ChangePassword(1, "newpassword"), Times.Once);
    }

    [Fact]
    public void ChangePassword_ThrowsException_WhenPasswordIsEmpty()
    {
        // Act & Assert
        Assert.Throws<ArgumentException>(() => _service.ChangePassword(1, ""));
    }

    [Fact]
    public void UserExists_ReturnsTrue_WhenUserExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.UserExists(1)).Returns(true);

        // Act
        var result = _service.UserExists(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.UserExists(1), Times.Once);
    }

    [Fact]
    public void UsernameExists_ReturnsTrue_WhenUsernameExists()
    {
        // Arrange
        _mockRepository.Setup(r => r.UsernameExists("existinguser")).Returns(true);

        // Act
        var result = _service.UsernameExists("existinguser");

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.UsernameExists("existinguser"), Times.Once);
    }

    [Fact]
    public void GetTotalUserCount_ReturnsCorrectCount()
    {
        // Arrange
        var users = new List<UserAccount>
        {
            new UserAccount { UserAccountId = 1, Username = "u1", Password = "p1", Role = "Student" },
            new UserAccount { UserAccountId = 2, Username = "u2", Password = "p2", Role = "Teacher" },
            new UserAccount { UserAccountId = 3, Username = "u3", Password = "p3", Role = "Admin" }
        };
        _mockRepository.Setup(r => r.GetAllUsers()).Returns(users);

        // Act
        var result = _service.GetTotalUserCount();

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetUserCountByRole_ReturnsCorrectCount()
    {
        // Arrange
        var students = new List<UserAccount>
        {
            new UserAccount { UserAccountId = 1, Username = "s1", Password = "p1", Role = "Student" },
            new UserAccount { UserAccountId = 2, Username = "s2", Password = "p2", Role = "Student" }
        };
        _mockRepository.Setup(r => r.GetUsersByRole("Student")).Returns(students);

        // Act
        var result = _service.GetUserCountByRole("Student");

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void IsAdmin_ReturnsTrue_WhenUserIsAdmin()
    {
        // Arrange
        _mockRepository.Setup(r => r.IsAdmin(1)).Returns(true);

        // Act
        var result = _service.IsAdmin(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.IsAdmin(1), Times.Once);
    }

    [Fact]
    public void IsStudent_ReturnsTrue_WhenUserIsStudent()
    {
        // Arrange
        _mockRepository.Setup(r => r.IsStudent(1)).Returns(true);

        // Act
        var result = _service.IsStudent(1);

        // Assert
        Assert.True(result);
        _mockRepository.Verify(r => r.IsStudent(1), Times.Once);
    }
}
