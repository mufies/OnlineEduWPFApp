using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class StudentClassDAO
    {
        private static StudentClassDAO instance = null;
        private static readonly object instanceLock = new object();

        private StudentClassDAO() { }

        public static StudentClassDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StudentClassDAO();
                    }
                    return instance;
                }
            }
        }

        public List<StudentClass> GetAllClasses()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentClasses
                    .Include(c => c.Students)
                    .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                    .ToList();
            }
        }

        public StudentClass GetClassById(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentClasses
                    .Include(c => c.Students)
                    .Include(c => c.ClassSubjects)
                    .ThenInclude(cs => cs.Subject)
                    .FirstOrDefault(c => c.ClassId == classId);
            }
        }

        public StudentClass GetClassByCode(string classCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentClasses
                    .Include(c => c.Students)
                    .FirstOrDefault(c => c.ClassCode == classCode);
            }
        }

        public void AddClass(StudentClass studentClass)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.StudentClasses.Add(studentClass);
                context.SaveChanges();
            }
        }

        public void UpdateClass(StudentClass studentClass)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Entry(studentClass).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteClass(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var studentClass = context.StudentClasses.Find(classId);
                if (studentClass != null)
                {
                    context.StudentClasses.Remove(studentClass);
                    context.SaveChanges();
                }
            }
        }

        public bool ClassExists(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentClasses.Any(c => c.ClassId == classId);
            }
        }

        public bool ClassCodeExists(string classCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentClasses.Any(c => c.ClassCode == classCode);
            }
        }

        public int GetStudentCountInClass(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Students.Count(s => s.ClassId == classId);
            }
        }
    }
}
