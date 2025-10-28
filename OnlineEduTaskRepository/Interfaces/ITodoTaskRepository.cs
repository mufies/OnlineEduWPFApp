using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface ITodoTaskRepository
    {
        List<StudentManagementBusinessObject.Task> GetAllTasks();
        StudentManagementBusinessObject.Task GetTaskById(int taskId);
        List<StudentManagementBusinessObject.Task> GetTasksByStudentId(int studentId);
        List<StudentManagementBusinessObject.Task> GetCompletedTasks(int studentId);
        List<StudentManagementBusinessObject.Task> GetPendingTasks(int studentId);
        List<StudentManagementBusinessObject.Task> GetOverdueTasks(int studentId);
        void AddTask(StudentManagementBusinessObject.Task task);
        void UpdateTask(StudentManagementBusinessObject.Task task);
        void DeleteTask(int taskId);
        void MarkTaskAsCompleted(int taskId);
        void MarkTaskAsPending(int taskId);
        bool TaskExists(int taskId);
        int GetTaskCountByStudent(int studentId);
        int GetCompletedTaskCountByStudent(int studentId);
        int GetPendingTaskCountByStudent(int studentId);
    }
}
