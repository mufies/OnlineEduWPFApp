using StudentManagementBusinessObject;
using OnlineEduTaskDAO;

namespace OnlineEduTaskWPF
{
    public static class DatabaseSeeder
    {
        public static void SeedData()
        {
            try
            {
                // Check if data already exists
                var existingUsers = UserAccountDAO.Instance.GetAllUsers();
                if (existingUsers.Count > 0)
                {
                    return; // Data already seeded
                }

                Console.WriteLine("Seeding database...");

                // 1. Create Classes first
                var class1 = new StudentClass
                {
                    ClassId = 1,
                    ClassCode = "SE1801",
                    ClassName = "Software Engineering 18.01"
                };
                StudentClassDAO.Instance.AddClass(class1);

                var class2 = new StudentClass
                {
                    ClassId = 2,
                    ClassCode = "SE1802",
                    ClassName = "Software Engineering 18.02"
                };
                StudentClassDAO.Instance.AddClass(class2);

                // 2. Create Students
                var student1 = new Student
                {
                    StudentId = 1,
                    StudentCode = "SE180001",
                    FullName = "Trần Thị Học",
                    Email = "student1@school.com",
                    ClassId = 1
                };
                StudentDAO.Instance.AddStudent(student1);

                var student2 = new Student
                {
                    StudentId = 2,
                    StudentCode = "SE180002",
                    FullName = "Lê Văn Thông",
                    Email = "student2@school.com",
                    ClassId = 1
                };
                StudentDAO.Instance.AddStudent(student2);

                // 3. Create User Accounts
                var adminAccount = new UserAccount
                {
                    UserAccountId = 1,
                    Username = "admin",
                    Password = "123456",
                    Role = "Admin",
                    StudentId = null,
                    TeacherId = null
                };
                UserAccountDAO.Instance.AddUser(adminAccount);

                var teacher1 = new UserAccount
                {
                    UserAccountId = 2,
                    Username = "teacher1",
                    Password = "123456",
                    Role = "Teacher",
                    StudentId = null,
                    TeacherId = 1
                };
                UserAccountDAO.Instance.AddUser(teacher1);

                var studentAccount = new UserAccount
                {
                    UserAccountId = 3,
                    Username = "student1",
                    Password = "123456",
                    Role = "Student",
                    StudentId = 1,
                    TeacherId = null
                };
                UserAccountDAO.Instance.AddUser(studentAccount);

                // 4. Create Subjects
                var subject1 = new Subject
                {
                    SubjectId = 1,
                    SubjectCode = "PRN212",
                    SubjectName = "PRN212 - Basic Cross-Platform Application Programming with .NET",
                    Credits = 3
                };
                SubjectDAO.Instance.AddSubject(subject1);

                var subject2 = new Subject
                {
                    SubjectId = 2,
                    SubjectCode = "PRJ301",
                    SubjectName = "PRJ301 - Java Web Application Development",
                    Credits = 3
                };
                SubjectDAO.Instance.AddSubject(subject2);

                var subject3 = new Subject
                {
                    SubjectId = 3,
                    SubjectCode = "SWP391",
                    SubjectName = "SWP391 - Software Development Project",
                    Credits = 3
                };
                SubjectDAO.Instance.AddSubject(subject3);

                // 5. Create ClassSubjects (assign subjects to classes with teacher)
                var classSubject1 = new ClassSubject
                {
                    ClassId = 1,
                    SubjectId = 1,
                    TeacherId = 1  // Teacher 1 teaches PRN212 to SE1801
                };
                ClassSubjectDAO.Instance.AddClassSubject(classSubject1);

                var classSubject2 = new ClassSubject
                {
                    ClassId = 1,
                    SubjectId = 2,
                    TeacherId = 1  // Teacher 1 teaches PRJ301 to SE1801
                };
                ClassSubjectDAO.Instance.AddClassSubject(classSubject2);

                var classSubject3 = new ClassSubject
                {
                    ClassId = 2,
                    SubjectId = 3,
                    TeacherId = 1  // Teacher 1 teaches SWP391 to SE1802
                };
                ClassSubjectDAO.Instance.AddClassSubject(classSubject3);

                Console.WriteLine("Database seeded successfully!");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error seeding database: {ex.Message}");
            }
        }
    }
}
