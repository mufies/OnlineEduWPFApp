using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class ClassTaskDAO
    {
        private static ClassTaskDAO instance = null;
        private static readonly object instanceLock = new object();

        private ClassTaskDAO() { }

        public static ClassTaskDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ClassTaskDAO();
                    }
                    return instance;
                }
            }
        }

        // Get all ClassTasks
        public List<ClassTask> GetAllClassTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .ToList();
            }
        }

        // Get ClassTask by ID
        public ClassTask GetClassTaskById(int classTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .FirstOrDefault(ct => ct.ClassTaskId == classTaskId);
            }
        }

        // Get ClassTasks by ClassId
        public List<ClassTask> GetClassTasksByClassId(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .Where(ct => ct.ClassId == classId)
                    .ToList();
            }
        }

        // Get ClassTasks by SubjectId
        public List<ClassTask> GetClassTasksBySubjectId(int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .Where(ct => ct.SubjectId == subjectId)
                    .ToList();
            }
        }

        // Get ClassTasks by TaskId
        public List<ClassTask> GetClassTasksByTaskId(int taskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .Where(ct => ct.TaskId == taskId)
                    .ToList();
            }
        }

        // Get overdue ClassTasks
        public List<ClassTask> GetOverdueClassTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                var now = DateTime.Now;
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .Where(ct => ct.DueDate.HasValue && ct.DueDate.Value < now)
                    .ToList();
            }
        }

        // Get upcoming ClassTasks (due within next 7 days)
        public List<ClassTask> GetUpcomingClassTasks(int days = 7)
        {
            using (var context = new StudentManagementDbContext())
            {
                var now = DateTime.Now;
                var futureDate = now.AddDays(days);
                return context.ClassTasks
                    .Include(ct => ct.Task)
                    .Include(ct => ct.StudentClass)
                    .Include(ct => ct.Subject)
                    .Include(ct => ct.StudentTasks)
                    .Where(ct => ct.DueDate.HasValue && ct.DueDate.Value >= now && ct.DueDate.Value <= futureDate)
                    .ToList();
            }
        }

        // Add ClassTask
        public void AddClassTask(ClassTask classTask)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.ClassTasks.Add(classTask);
                context.SaveChanges();
            }
        }

        // Update ClassTask
        public void UpdateClassTask(ClassTask classTask)
        {
            using (var context = new StudentManagementDbContext())
            {
                var existingClassTask = context.ClassTasks.Find(classTask.ClassTaskId);
                if (existingClassTask != null)
                {
                    context.Entry(existingClassTask).CurrentValues.SetValues(classTask);
                    context.SaveChanges();
                }
            }
        }

        // Delete ClassTask
        public void DeleteClassTask(int classTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var classTask = context.ClassTasks.Find(classTaskId);
                if (classTask != null)
                {
                    context.ClassTasks.Remove(classTask);
                    context.SaveChanges();
                }
            }
        }

        // Get total count
        public int GetTotalCount()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks.Count();
            }
        }

        // Check if ClassTask exists
        public bool ClassTaskExists(int classTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassTasks.Any(ct => ct.ClassTaskId == classTaskId);
            }
        }
    }
}
