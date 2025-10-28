using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Interfaces
{
    public interface IClassTaskService
    {
        List<ClassTask> GetAllClassTasks();
        ClassTask GetClassTaskById(int classTaskId);
        List<ClassTask> GetClassTasksByClassId(int classId);
        List<ClassTask> GetClassTasksBySubjectId(int subjectId);
        List<ClassTask> GetClassTasksByTaskId(int taskId);
        List<ClassTask> GetOverdueClassTasks();
        List<ClassTask> GetUpcomingClassTasks(int days = 7);
        void AddClassTask(ClassTask classTask);
        void UpdateClassTask(ClassTask classTask);
        void DeleteClassTask(int classTaskId);
        void AssignTaskToClass(int taskId, int classId, int subjectId, DateTime? dueDate, int maxScore);
        int GetTotalCount();
        bool ClassTaskExists(int classTaskId);
    }
}
