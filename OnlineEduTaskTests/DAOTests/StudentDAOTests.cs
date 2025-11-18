using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;
using Xunit;

namespace OnlineEduTaskTests.DAOTests;

public class StudentDAOTests : IDisposable
{
    private readonly StudentManagementDbContext _context;

    public StudentDAOTests()
    {
        var options = new DbContextOptionsBuilder<StudentManagementDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new StudentManagementDbContext(options);
    }

    [Fact]
    public void GetAllStudents_ReturnsEmptyList_WhenNoStudentsExist()
    {
        // Act
        var result = _context.Students.ToList();

        // Assert
        Assert.NotNull(result);
        Assert.Empty(result);
    }

    [Fact]
    public void AddStudent_AddsStudentSuccessfully()
    {
        // Arrange
        var student = new Student
        {
            StudentId = 1,
            StudentCode = "ST001",
            FullName = "Test Student",
            Email = "test@example.com",
            ClassId = 1
        };

        // Act
        _context.Students.Add(student);
        _context.SaveChanges();
        
        var result = _context.Students.Find(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Test Student", result.FullName);
        Assert.Equal("test@example.com", result.Email);
    }

    [Fact]
    public void GetStudentById_ReturnsNull_WhenStudentDoesNotExist()
    {
        // Act
        var result = _context.Students.Find(999);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void UpdateStudent_UpdatesStudentSuccessfully()
    {
        // Arrange
        var student = new Student
        {
            StudentId = 1,
            StudentCode = "ST001",
            FullName = "Test Student",
            Email = "test@example.com",
            ClassId = 1
        };
        _context.Students.Add(student);
        _context.SaveChanges();

        // Act
        student.FullName = "Updated Name";
        student.Email = "updated@example.com";
        _context.SaveChanges();
        
        var result = _context.Students.Find(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal("Updated Name", result.FullName);
        Assert.Equal("updated@example.com", result.Email);
    }

    [Fact]
    public void DeleteStudent_RemovesStudentSuccessfully()
    {
        // Arrange
        var student = new Student
        {
            StudentId = 1,
            StudentCode = "ST001",
            FullName = "Test Student",
            Email = "test@example.com",
            ClassId = 1
        };
        _context.Students.Add(student);
        _context.SaveChanges();

        // Act
        _context.Students.Remove(student);
        _context.SaveChanges();
        
        var result = _context.Students.Find(1);

        // Assert
        Assert.Null(result);
    }

    [Fact]
    public void SearchStudents_FindsStudentsByName()
    {
        // Arrange
        _context.Students.Add(new Student
        {
            StudentId = 1,
            StudentCode = "ST001",
            FullName = "John Doe",
            Email = "john@example.com",
            ClassId = 1
        });
        _context.Students.Add(new Student
        {
            StudentId = 2,
            StudentCode = "ST002",
            FullName = "Jane Smith",
            Email = "jane@example.com",
            ClassId = 1
        });
        _context.SaveChanges();

        // Act
        var result = _context.Students.Where(s => s.FullName.Contains("John")).ToList();

        // Assert
        Assert.Single(result);
        Assert.Equal("John Doe", result.First().FullName);
    }

    [Fact]
    public void StudentExists_ReturnsTrue_WhenStudentExists()
    {
        // Arrange
        var student = new Student
        {
            StudentId = 1,
            StudentCode = "ST001",
            FullName = "Test Student",
            Email = "test@example.com",
            ClassId = 1
        };
        _context.Students.Add(student);
        _context.SaveChanges();

        // Act
        var result = _context.Students.Any(s => s.StudentId == 1);

        // Assert
        Assert.True(result);
    }

    [Fact]
    public void StudentExists_ReturnsFalse_WhenStudentDoesNotExist()
    {
        // Act
        var result = _context.Students.Any(s => s.StudentId == 999);

        // Assert
        Assert.False(result);
    }

    public void Dispose()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }
}
