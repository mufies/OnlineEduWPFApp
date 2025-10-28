using System;
using System.Collections.Generic;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Services
{
    public class StudentTaskService : IStudentTaskService
    {
        private readonly IStudentTaskRepository _studentTaskRepository;
        private readonly IClassTaskRepository _classTaskRepository;
        private readonly IStudentRepository _studentRepository;

        public StudentTaskService(
            IStudentTaskRepository studentTaskRepository,
            IClassTaskRepository classTaskRepository,
            IStudentRepository studentRepository)
        {
            _studentTaskRepository = studentTaskRepository;
            _classTaskRepository = classTaskRepository;
            _studentRepository = studentRepository;
        }

        public List<StudentTask> GetAllStudentTasks() => _studentTaskRepository.GetAllStudentTasks();

        public StudentTask GetStudentTaskById(int studentTaskId) => _studentTaskRepository.GetStudentTaskById(studentTaskId);

        public List<StudentTask> GetStudentTasksByStudentId(int studentId) => _studentTaskRepository.GetStudentTasksByStudentId(studentId);

        public List<StudentTask> GetStudentTasksByClassTaskId(int classTaskId) => _studentTaskRepository.GetStudentTasksByClassTaskId(classTaskId);

        public List<StudentTask> GetSubmittedStudentTasks() => _studentTaskRepository.GetSubmittedStudentTasks();

        public List<StudentTask> GetPendingStudentTasks() => _studentTaskRepository.GetPendingStudentTasks();

        public List<StudentTask> GetPendingStudentTasksByStudentId(int studentId) => _studentTaskRepository.GetPendingStudentTasksByStudentId(studentId);

        public List<StudentTask> GetGradedStudentTasks() => _studentTaskRepository.GetGradedStudentTasks();

        public List<StudentTask> GetUngradedStudentTasks() => _studentTaskRepository.GetUngradedStudentTasks();

        public List<StudentTask> GetOverdueStudentTasksByStudentId(int studentId) => _studentTaskRepository.GetOverdueStudentTasksByStudentId(studentId);

        public void AddStudentTask(StudentTask studentTask)
        {
            if (studentTask == null)
                throw new ArgumentNullException(nameof(studentTask));
            
            studentTask.IsSubmitted = false;
            _studentTaskRepository.AddStudentTask(studentTask);
        }

        public void UpdateStudentTask(StudentTask studentTask) => _studentTaskRepository.UpdateStudentTask(studentTask);

        public void DeleteStudentTask(int studentTaskId) => _studentTaskRepository.DeleteStudentTask(studentTaskId);

        public void SubmitStudentTask(int studentTaskId, string submissionContent, string submissionFilePath = null)
        {
            _studentTaskRepository.SubmitStudentTask(studentTaskId, submissionContent, submissionFilePath);
        }

        public void GradeStudentTask(int studentTaskId, int score, string feedback = null)
        {
            _studentTaskRepository.GradeStudentTask(studentTaskId, score, feedback);
        }

        public void CreateStudentTasksForClass(int classTaskId)
        {
            var classTask = _classTaskRepository.GetClassTaskById(classTaskId);
            if (classTask == null)
                throw new InvalidOperationException("ClassTask not found");
            
            // Get all students in the class
            var students = _studentRepository.GetStudentsByClassId(classTask.ClassId);
            
            // Create a StudentTask for each student
            foreach (var student in students)
            {
                // Check if student already has this task
                if (!_studentTaskRepository.HasSubmission(classTaskId, student.StudentId))
                {
                    var studentTask = new StudentTask
                    {
                        ClassTaskId = classTaskId,
                        StudentId = student.StudentId,
                        IsSubmitted = false
                    };
                    
                    _studentTaskRepository.AddStudentTask(studentTask);
                }
            }
        }

        public int GetTotalCount() => _studentTaskRepository.GetTotalCount();

        public double GetSubmissionRate(int classTaskId) => _studentTaskRepository.GetSubmissionRate(classTaskId);

        public bool StudentTaskExists(int studentTaskId) => _studentTaskRepository.StudentTaskExists(studentTaskId);

        public bool HasSubmission(int classTaskId, int studentId) => _studentTaskRepository.HasSubmission(classTaskId, studentId);
    }
}
