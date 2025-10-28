using System;
using System.Collections.Generic;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;
using TaskEntity = StudentManagementBusinessObject.Task;

namespace OnlineEduTaskRepository.Repositories
{
    public class TaskRepository : ITaskRepository
    {
        public List<TaskEntity> GetAllTasks() => TaskDAO.Instance.GetAllTasks();

        public TaskEntity GetTaskById(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Task ID must be greater than 0");
            return TaskDAO.Instance.GetTaskById(taskId);
        }

        public List<TaskEntity> GetTasksByTeacherId(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Teacher ID must be greater than 0");
            return TaskDAO.Instance.GetTasksByTeacherId(teacherId);
        }

        public List<TaskEntity> GetRecentTasks(int count = 10)
        {
            if (count <= 0)
                throw new ArgumentException("Count must be greater than 0");
            return TaskDAO.Instance.GetRecentTasks(count);
        }

        public void AddTask(TaskEntity task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty");
            if (task.CreatedByTeacherId <= 0)
                throw new ArgumentException("Teacher ID must be greater than 0");
            
            TaskDAO.Instance.AddTask(task);
        }

        public void UpdateTask(TaskEntity task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            if (task.TaskId <= 0)
                throw new ArgumentException("Task ID must be greater than 0");
            if (string.IsNullOrWhiteSpace(task.Title))
                throw new ArgumentException("Task title cannot be empty");
            
            TaskDAO.Instance.UpdateTask(task);
        }

        public void DeleteTask(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Task ID must be greater than 0");
            TaskDAO.Instance.DeleteTask(taskId);
        }

        public bool TaskExists(int taskId)
        {
            if (taskId <= 0)
                return false;
            return TaskDAO.Instance.TaskExists(taskId);
        }

        public int GetTotalCount() => TaskDAO.Instance.GetTotalCount();

        public int GetTaskCountByTeacher(int teacherId)
        {
            if (teacherId <= 0)
                throw new ArgumentException("Teacher ID must be greater than 0");
            return TaskDAO.Instance.GetTaskCountByTeacher(teacherId);
        }
    }
}
