using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class StudentDAO
    {
        private static StudentDAO instance = null;
        private static readonly object instanceLock = new object();
        private StudentManagementDbContext _testContext = null;

        private StudentDAO() { }

        // For testing purposes only - Reset singleton and set test context
        public static void ResetForTesting(StudentManagementDbContext testContext = null)
        {
            lock (instanceLock)
            {
                instance = new StudentDAO();
                if (testContext != null)
                {
                    instance._testContext = testContext;
                }
            }
        }

        // For testing purposes only
        public void SetContext(StudentManagementDbContext context)
        {
            _testContext = context;
        }

        private StudentManagementDbContext GetContext()
        {
            return _testContext ?? new StudentManagementDbContext();
        }

        public static StudentDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StudentDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Student> GetAllStudents()
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                return context.Students.Include(s => s.Class).ToList();
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }

        public Student GetStudentById(int studentId)
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                return context.Students
                    .Include(s => s.Class)
                    .Include(s => s.Tasks)
                    .FirstOrDefault(s => s.StudentId == studentId);
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }

        public Student GetStudentByCode(string studentCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students
                    .Include(s => s.Class)
                    .FirstOrDefault(s => s.StudentCode == studentCode);
            }
        }

        public Student GetStudentByEmail(string email)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students
                    .Include(s => s.Class)
                    .FirstOrDefault(s => s.Email == email);
            }
        }

        public List<Student> GetStudentsByClassId(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students
                    .Include(s => s.Class)
                    .Where(s => s.ClassId == classId)
                    .ToList();
            }
        }

        public void AddStudent(Student student)
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }

        public void UpdateStudent(Student student)
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                var existingStudent = context.Students.Find(student.StudentId);
                if (existingStudent != null)
                {
                    existingStudent.StudentCode = student.StudentCode;
                    existingStudent.FullName = student.FullName;
                    existingStudent.Email = student.Email;
                    existingStudent.ClassId = student.ClassId;
                    context.SaveChanges();
                }
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }

        public void DeleteStudent(int studentId)
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                var student = context.Students.Find(studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }

        public bool StudentExists(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students.Any(s => s.StudentId == studentId);
            }
        }

        public bool StudentCodeExists(string studentCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students.Any(s => s.StudentCode == studentCode);
            }
        }

        public List<Student> SearchStudents(string keyword)
        {
            var context = GetContext();
            var shouldDispose = _testContext == null;
            try
            {
                return context.Students
                    .Include(s => s.Class)
                    .Where(s => s.FullName.Contains(keyword) || 
                                s.Email.Contains(keyword) || 
                                (s.StudentCode != null && s.StudentCode.Contains(keyword)))
                    .ToList();
            }
            finally
            {
                if (shouldDispose) context.Dispose();
            }
        }
    }
}
