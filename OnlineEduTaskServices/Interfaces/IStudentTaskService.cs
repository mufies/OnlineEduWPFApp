using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Interfaces
{
    public interface IStudentTaskService
    {
        List<StudentTask> GetAllStudentTasks();
        StudentTask GetStudentTaskById(int studentTaskId);
        List<StudentTask> GetStudentTasksByStudentId(int studentId);
        List<StudentTask> GetStudentTasksByClassTaskId(int classTaskId);
        List<StudentTask> GetSubmittedStudentTasks();
        List<StudentTask> GetPendingStudentTasks();
        List<StudentTask> GetPendingStudentTasksByStudentId(int studentId);
        List<StudentTask> GetGradedStudentTasks();
        List<StudentTask> GetUngradedStudentTasks();
        List<StudentTask> GetOverdueStudentTasksByStudentId(int studentId);
        void AddStudentTask(StudentTask studentTask);
        void UpdateStudentTask(StudentTask studentTask);
        void DeleteStudentTask(int studentTaskId);
        void SubmitStudentTask(int studentTaskId, string submissionContent, string submissionFilePath = null);
        void GradeStudentTask(int studentTaskId, int score, string feedback = null);
        void CreateStudentTasksForClass(int classTaskId);
        int GetTotalCount();
        double GetSubmissionRate(int classTaskId);
        bool StudentTaskExists(int studentTaskId);
        bool HasSubmission(int classTaskId, int studentId);
    }
}
