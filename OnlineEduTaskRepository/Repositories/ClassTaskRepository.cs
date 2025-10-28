using System;
using System.Collections.Generic;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Repositories
{
    public class ClassTaskRepository : IClassTaskRepository
    {
        public List<ClassTask> GetAllClassTasks() => ClassTaskDAO.Instance.GetAllClassTasks();

        public ClassTask GetClassTaskById(int classTaskId)
        {
            if (classTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            return ClassTaskDAO.Instance.GetClassTaskById(classTaskId);
        }

        public List<ClassTask> GetClassTasksByClassId(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");
            return ClassTaskDAO.Instance.GetClassTasksByClassId(classId);
        }

        public List<ClassTask> GetClassTasksBySubjectId(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");
            return ClassTaskDAO.Instance.GetClassTasksBySubjectId(subjectId);
        }

        public List<ClassTask> GetClassTasksByTaskId(int taskId)
        {
            if (taskId <= 0)
                throw new ArgumentException("Task ID must be greater than 0");
            return ClassTaskDAO.Instance.GetClassTasksByTaskId(taskId);
        }

        public List<ClassTask> GetOverdueClassTasks() => ClassTaskDAO.Instance.GetOverdueClassTasks();

        public List<ClassTask> GetUpcomingClassTasks(int days = 7)
        {
            if (days <= 0)
                throw new ArgumentException("Days must be greater than 0");
            return ClassTaskDAO.Instance.GetUpcomingClassTasks(days);
        }

        public void AddClassTask(ClassTask classTask)
        {
            if (classTask == null)
                throw new ArgumentNullException(nameof(classTask));
            if (classTask.TaskId <= 0)
                throw new ArgumentException("Task ID must be greater than 0");
            if (classTask.ClassId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");
            if (classTask.SubjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");
            if (classTask.MaxScore < 0)
                throw new ArgumentException("Max score cannot be negative");
            if (classTask.DueDate.HasValue && classTask.DueDate.Value < classTask.AssignedDate)
                throw new ArgumentException("Due date cannot be before assigned date");
            
            ClassTaskDAO.Instance.AddClassTask(classTask);
        }

        public void UpdateClassTask(ClassTask classTask)
        {
            if (classTask == null)
                throw new ArgumentNullException(nameof(classTask));
            if (classTask.ClassTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            if (classTask.MaxScore < 0)
                throw new ArgumentException("Max score cannot be negative");
            
            ClassTaskDAO.Instance.UpdateClassTask(classTask);
        }

        public void DeleteClassTask(int classTaskId)
        {
            if (classTaskId <= 0)
                throw new ArgumentException("ClassTask ID must be greater than 0");
            ClassTaskDAO.Instance.DeleteClassTask(classTaskId);
        }

        public int GetTotalCount() => ClassTaskDAO.Instance.GetTotalCount();

        public bool ClassTaskExists(int classTaskId)
        {
            if (classTaskId <= 0)
                return false;
            return ClassTaskDAO.Instance.ClassTaskExists(classTaskId);
        }
    }
}
