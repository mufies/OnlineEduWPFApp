using System;
using System.Collections.Generic;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;
using TaskEntity = StudentManagementBusinessObject.Task;

namespace OnlineEduTaskServices.Services
{
    public class TaskService : ITaskService
    {
        private readonly ITaskRepository _taskRepository;

        public TaskService(ITaskRepository taskRepository)
        {
            _taskRepository = taskRepository;
        }

        public List<TaskEntity> GetAllTasks() => _taskRepository.GetAllTasks();

        public TaskEntity GetTaskById(int taskId) => _taskRepository.GetTaskById(taskId);

        public List<TaskEntity> GetTasksByTeacherId(int teacherId) => _taskRepository.GetTasksByTeacherId(teacherId);

        public List<TaskEntity> GetRecentTasks(int count = 10) => _taskRepository.GetRecentTasks(count);

        public void AddTask(TaskEntity task)
        {
            if (task == null)
                throw new ArgumentNullException(nameof(task));
            
            task.CreatedDate = DateTime.Now;
            _taskRepository.AddTask(task);
        }

        public void UpdateTask(TaskEntity task) => _taskRepository.UpdateTask(task);

        public void DeleteTask(int taskId) => _taskRepository.DeleteTask(taskId);

        public bool TaskExists(int taskId) => _taskRepository.TaskExists(taskId);

        public int GetTotalCount() => _taskRepository.GetTotalCount();

        public int GetTaskCountByTeacher(int teacherId) => _taskRepository.GetTaskCountByTeacher(teacherId);
    }
}
