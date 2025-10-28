using System;
using System.Collections.Generic;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Repositories
{
    public class StudentTaskRepository : IStudentTaskRepository
    {
        public List<StudentTask> GetAllStudentTasks() => StudentTaskDAO.Instance.GetAllStudentTasks();

        public StudentTask GetStudentTaskById(int studentTaskId)
        {
            if (studentTaskId <= 0)
                throw new ArgumentException("StudentTask ID must be greater than 0");
            return StudentTaskDAO.Instance.GetStudentTaskById(studentTaskId);
        }

        public List<StudentTask> GetStudentTasksByStudentId(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");
            return StudentTaskDAO.Instance.GetStudentTasksByStudentId(studentId);
        }

        public List<StudentTask> GetStudentTasksByClassTaskId(int classTaskId)
        {
            if (classTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            return StudentTaskDAO.Instance.GetStudentTasksByClassTaskId(classTaskId);
        }

        public List<StudentTask> GetSubmittedStudentTasks() => StudentTaskDAO.Instance.GetSubmittedStudentTasks();

        public List<StudentTask> GetPendingStudentTasks() => StudentTaskDAO.Instance.GetPendingStudentTasks();

        public List<StudentTask> GetPendingStudentTasksByStudentId(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");
            return StudentTaskDAO.Instance.GetPendingStudentTasksByStudentId(studentId);
        }

        public List<StudentTask> GetGradedStudentTasks() => StudentTaskDAO.Instance.GetGradedStudentTasks();

        public List<StudentTask> GetUngradedStudentTasks() => StudentTaskDAO.Instance.GetUngradedStudentTasks();

        public List<StudentTask> GetOverdueStudentTasksByStudentId(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");
            return StudentTaskDAO.Instance.GetOverdueStudentTasksByStudentId(studentId);
        }

        public void AddStudentTask(StudentTask studentTask)
        {
            if (studentTask == null)
                throw new ArgumentNullException(nameof(studentTask));
            if (studentTask.ClassTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            if (studentTask.StudentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");
            if (HasSubmission(studentTask.ClassTaskId, studentTask.StudentId))
                throw new InvalidOperationException("Student already has a submission for this class task");
            
            StudentTaskDAO.Instance.AddStudentTask(studentTask);
        }

        public void UpdateStudentTask(StudentTask studentTask)
        {
            if (studentTask == null)
                throw new ArgumentNullException(nameof(studentTask));
            if (studentTask.StudentTaskId <= 0)
                throw new ArgumentException("StudentTask ID must be greater than 0");
            
            StudentTaskDAO.Instance.UpdateStudentTask(studentTask);
        }

        public void DeleteStudentTask(int studentTaskId)
        {
            if (studentTaskId <= 0)
                throw new ArgumentException("StudentTask ID must be greater than 0");
            StudentTaskDAO.Instance.DeleteStudentTask(studentTaskId);
        }

        public void SubmitStudentTask(int studentTaskId, string submissionContent, string submissionFilePath = null)
        {
            if (studentTaskId <= 0)
                throw new ArgumentException("StudentTask ID must be greater than 0");
            if (string.IsNullOrWhiteSpace(submissionContent) && string.IsNullOrWhiteSpace(submissionFilePath))
                throw new ArgumentException("Either submission content or file path must be provided");
            
            StudentTaskDAO.Instance.SubmitStudentTask(studentTaskId, submissionContent, submissionFilePath);
        }

        public void GradeStudentTask(int studentTaskId, int score, string feedback = null)
        {
            if (studentTaskId <= 0)
                throw new ArgumentException("StudentTask ID must be greater than 0");
            if (score < 0)
                throw new ArgumentException("Score cannot be negative");
            
            // Check if task is submitted
            var studentTask = GetStudentTaskById(studentTaskId);
            if (studentTask == null)
                throw new InvalidOperationException("StudentTask not found");
            if (!studentTask.IsSubmitted)
                throw new InvalidOperationException("Cannot grade a task that hasn't been submitted");
            
            // Validate score doesn't exceed max score
            if (studentTask.ClassTask != null && score > studentTask.ClassTask.MaxScore)
                throw new ArgumentException($"Score cannot exceed maximum score of {studentTask.ClassTask.MaxScore}");
            
            StudentTaskDAO.Instance.GradeStudentTask(studentTaskId, score, feedback);
        }

        public int GetTotalCount() => StudentTaskDAO.Instance.GetTotalCount();

        public double GetSubmissionRate(int classTaskId)
        {
            if (classTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            return StudentTaskDAO.Instance.GetSubmissionRate(classTaskId);
        }

        public bool StudentTaskExists(int studentTaskId)
        {
            if (studentTaskId <= 0)
                return false;
            return StudentTaskDAO.Instance.StudentTaskExists(studentTaskId);
        }

        public bool HasSubmission(int classTaskId, int studentId)
        {
            if (classTaskId <= 0 || studentId <= 0)
                return false;
            return StudentTaskDAO.Instance.HasSubmission(classTaskId, studentId);
        }
    }
}
