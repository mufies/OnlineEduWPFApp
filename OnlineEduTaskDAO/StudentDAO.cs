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

        private StudentDAO() { }

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
            using (var context = new StudentManagementDbContext())
            {
                return context.Students.Include(s => s.Class).ToList();
            }
        }

        public Student GetStudentById(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students
                    .Include(s => s.Class)
                    .Include(s => s.Tasks)
                    .FirstOrDefault(s => s.StudentId == studentId);
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
            using (var context = new StudentManagementDbContext())
            {
                context.Students.Add(student);
                context.SaveChanges();
            }
        }

        public void UpdateStudent(Student student)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Entry(student).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteStudent(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var student = context.Students.Find(studentId);
                if (student != null)
                {
                    context.Students.Remove(student);
                    context.SaveChanges();
                }
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
    }
}
