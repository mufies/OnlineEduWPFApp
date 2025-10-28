using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class TaskDAO
    {
        private static TaskDAO instance = null;
        private static readonly object instanceLock = new object();

        private TaskDAO() { }

        public static TaskDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TaskDAO();
                    }
                    return instance;
                }
            }
        }

        // Get all Tasks
        public List<StudentManagementBusinessObject.Task> GetAllTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks
                    .Include(t => t.CreatedByTeacher)
                    .Include(t => t.ClassTasks)
                    .ToList();
            }
        }

        // Get Task by ID
        public StudentManagementBusinessObject.Task GetTaskById(int taskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks
                    .Include(t => t.CreatedByTeacher)
                    .Include(t => t.ClassTasks)
                    .FirstOrDefault(t => t.TaskId == taskId);
            }
        }

        // Get Tasks by Teacher ID
        public List<StudentManagementBusinessObject.Task> GetTasksByTeacherId(int teacherId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks
                    .Include(t => t.CreatedByTeacher)
                    .Include(t => t.ClassTasks)
                    .Where(t => t.CreatedByTeacherId == teacherId)
                    .OrderByDescending(t => t.CreatedDate)
                    .ToList();
            }
        }

        // Get recently created Tasks
        public List<StudentManagementBusinessObject.Task> GetRecentTasks(int count = 10)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks
                    .Include(t => t.CreatedByTeacher)
                    .Include(t => t.ClassTasks)
                    .OrderByDescending(t => t.CreatedDate)
                    .Take(count)
                    .ToList();
            }
        }

        // Add Task
        public void AddTask(StudentManagementBusinessObject.Task task)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Tasks.Add(task);
                context.SaveChanges();
            }
        }

        // Update Task
        public void UpdateTask(StudentManagementBusinessObject.Task task)
        {
            using (var context = new StudentManagementDbContext())
            {
                var existingTask = context.Tasks.Find(task.TaskId);
                if (existingTask != null)
                {
                    context.Entry(existingTask).CurrentValues.SetValues(task);
                    context.SaveChanges();
                }
            }
        }

        // Delete Task
        public void DeleteTask(int taskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var task = context.Tasks.Find(taskId);
                if (task != null)
                {
                    context.Tasks.Remove(task);
                    context.SaveChanges();
                }
            }
        }

        // Check if Task exists
        public bool TaskExists(int taskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks.Any(t => t.TaskId == taskId);
            }
        }

        // Get total count
        public int GetTotalCount()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks.Count();
            }
        }

        // Get count by teacher
        public int GetTaskCountByTeacher(int teacherId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Tasks.Count(t => t.CreatedByTeacherId == teacherId);
            }
        }
    }
}
