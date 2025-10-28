using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class StudentTaskDAO
    {
        private static StudentTaskDAO instance = null;
        private static readonly object instanceLock = new object();

        private StudentTaskDAO() { }

        public static StudentTaskDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new StudentTaskDAO();
                    }
                    return instance;
                }
            }
        }

        // Get all StudentTasks
        public List<StudentTask> GetAllStudentTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .ToList();
            }
        }

        // Get StudentTask by ID
        public StudentTask GetStudentTaskById(int studentTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .FirstOrDefault(st => st.StudentTaskId == studentTaskId);
            }
        }

        // Get StudentTasks by StudentId
        public List<StudentTask> GetStudentTasksByStudentId(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.StudentId == studentId)
                    .ToList();
            }
        }

        // Get StudentTasks by ClassTaskId
        public List<StudentTask> GetStudentTasksByClassTaskId(int classTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.ClassTaskId == classTaskId)
                    .ToList();
            }
        }

        // Get submitted StudentTasks
        public List<StudentTask> GetSubmittedStudentTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.IsSubmitted)
                    .ToList();
            }
        }

        // Get pending StudentTasks (not submitted)
        public List<StudentTask> GetPendingStudentTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => !st.IsSubmitted)
                    .ToList();
            }
        }

        // Get pending StudentTasks by StudentId
        public List<StudentTask> GetPendingStudentTasksByStudentId(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.StudentId == studentId && !st.IsSubmitted)
                    .ToList();
            }
        }

        // Get graded StudentTasks
        public List<StudentTask> GetGradedStudentTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.Score.HasValue)
                    .ToList();
            }
        }

        // Get ungraded StudentTasks (submitted but not graded)
        public List<StudentTask> GetUngradedStudentTasks()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.IsSubmitted && !st.Score.HasValue)
                    .ToList();
            }
        }

        // Get overdue StudentTasks (not submitted and past due date)
        public List<StudentTask> GetOverdueStudentTasksByStudentId(int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var now = DateTime.Now;
                return context.StudentTasks
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Task)
                    .Include(st => st.ClassTask)
                        .ThenInclude(ct => ct.Subject)
                    .Include(st => st.Student)
                    .Where(st => st.StudentId == studentId 
                        && !st.IsSubmitted 
                        && st.ClassTask.DueDate.HasValue 
                        && st.ClassTask.DueDate.Value < now)
                    .ToList();
            }
        }

        // Add StudentTask
        public void AddStudentTask(StudentTask studentTask)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.StudentTasks.Add(studentTask);
                context.SaveChanges();
            }
        }

        // Update StudentTask
        public void UpdateStudentTask(StudentTask studentTask)
        {
            using (var context = new StudentManagementDbContext())
            {
                var existingStudentTask = context.StudentTasks.Find(studentTask.StudentTaskId);
                if (existingStudentTask != null)
                {
                    context.Entry(existingStudentTask).CurrentValues.SetValues(studentTask);
                    context.SaveChanges();
                }
            }
        }

        // Delete StudentTask
        public void DeleteStudentTask(int studentTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var studentTask = context.StudentTasks.Find(studentTaskId);
                if (studentTask != null)
                {
                    context.StudentTasks.Remove(studentTask);
                    context.SaveChanges();
                }
            }
        }

        // Submit StudentTask
        public void SubmitStudentTask(int studentTaskId, string submissionContent, string submissionFilePath = null)
        {
            using (var context = new StudentManagementDbContext())
            {
                var studentTask = context.StudentTasks.Find(studentTaskId);
                if (studentTask != null)
                {
                    studentTask.IsSubmitted = true;
                    studentTask.SubmittedDate = DateTime.Now;
                    studentTask.SubmissionContent = submissionContent;
                    studentTask.SubmissionFilePath = submissionFilePath;
                    context.SaveChanges();
                }
            }
        }

        // Grade StudentTask
        public void GradeStudentTask(int studentTaskId, int score, string feedback = null)
        {
            using (var context = new StudentManagementDbContext())
            {
                var studentTask = context.StudentTasks.Find(studentTaskId);
                if (studentTask != null)
                {
                    studentTask.Score = score;
                    studentTask.TeacherFeedback = feedback;
                    studentTask.GradedDate = DateTime.Now;
                    context.SaveChanges();
                }
            }
        }

        // Get total count
        public int GetTotalCount()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks.Count();
            }
        }

        // Get submission rate for a ClassTask
        public double GetSubmissionRate(int classTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var total = context.StudentTasks.Count(st => st.ClassTaskId == classTaskId);
                if (total == 0) return 0;
                var submitted = context.StudentTasks.Count(st => st.ClassTaskId == classTaskId && st.IsSubmitted);
                return (double)submitted / total * 100;
            }
        }

        // Check if StudentTask exists
        public bool StudentTaskExists(int studentTaskId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks.Any(st => st.StudentTaskId == studentTaskId);
            }
        }

        // Check if student already has submission for this ClassTask
        public bool HasSubmission(int classTaskId, int studentId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.StudentTasks.Any(st => st.ClassTaskId == classTaskId && st.StudentId == studentId);
            }
        }
    }
}
