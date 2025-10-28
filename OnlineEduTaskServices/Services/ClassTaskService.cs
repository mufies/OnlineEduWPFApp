using System;
using System.Collections.Generic;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Services
{
    public class ClassTaskService : IClassTaskService
    {
        private readonly IClassTaskRepository _classTaskRepository;
        private readonly IStudentTaskService _studentTaskService;

        public ClassTaskService(IClassTaskRepository classTaskRepository, IStudentTaskService studentTaskService)
        {
            _classTaskRepository = classTaskRepository;
            _studentTaskService = studentTaskService;
        }

        public List<ClassTask> GetAllClassTasks() => _classTaskRepository.GetAllClassTasks();

        public ClassTask GetClassTaskById(int classTaskId) => _classTaskRepository.GetClassTaskById(classTaskId);

        public List<ClassTask> GetClassTasksByClassId(int classId) => _classTaskRepository.GetClassTasksByClassId(classId);

        public List<ClassTask> GetClassTasksBySubjectId(int subjectId) => _classTaskRepository.GetClassTasksBySubjectId(subjectId);

        public List<ClassTask> GetClassTasksByTaskId(int taskId) => _classTaskRepository.GetClassTasksByTaskId(taskId);

        public List<ClassTask> GetOverdueClassTasks() => _classTaskRepository.GetOverdueClassTasks();

        public List<ClassTask> GetUpcomingClassTasks(int days = 7) => _classTaskRepository.GetUpcomingClassTasks(days);

        public void AddClassTask(ClassTask classTask)
        {
            if (classTask == null)
                throw new ArgumentNullException(nameof(classTask));
            
            classTask.AssignedDate = DateTime.Now;
            _classTaskRepository.AddClassTask(classTask);
            
            // Automatically create StudentTask records for all students in the class
            _studentTaskService.CreateStudentTasksForClass(classTask.ClassTaskId);
        }

        public void UpdateClassTask(ClassTask classTask) => _classTaskRepository.UpdateClassTask(classTask);

        public void DeleteClassTask(int classTaskId) => _classTaskRepository.DeleteClassTask(classTaskId);

        public void AssignTaskToClass(int taskId, int classId, int subjectId, DateTime? dueDate, int maxScore)
        {
            var classTask = new ClassTask
            {
                TaskId = taskId,
                ClassId = classId,
                SubjectId = subjectId,
                AssignedDate = DateTime.Now,
                DueDate = dueDate,
                MaxScore = maxScore
            };
            
            AddClassTask(classTask);
        }

        public int GetTotalCount() => _classTaskRepository.GetTotalCount();

        public bool ClassTaskExists(int classTaskId) => _classTaskRepository.ClassTaskExists(classTaskId);
    }
}
