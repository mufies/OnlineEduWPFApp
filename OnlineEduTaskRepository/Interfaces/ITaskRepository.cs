using System;
using System.Collections.Generic;
using TaskEntity = StudentManagementBusinessObject.Task;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface ITaskRepository
    {
        List<TaskEntity> GetAllTasks();
        TaskEntity GetTaskById(int taskId);
        List<TaskEntity> GetTasksByTeacherId(int teacherId);
        List<TaskEntity> GetRecentTasks(int count = 10);
        void AddTask(TaskEntity task);
        void UpdateTask(TaskEntity task);
        void DeleteTask(int taskId);
        bool TaskExists(int taskId);
        int GetTotalCount();
        int GetTaskCountByTeacher(int teacherId);
    }
}
