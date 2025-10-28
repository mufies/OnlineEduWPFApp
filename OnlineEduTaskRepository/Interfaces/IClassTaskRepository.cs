using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface IClassTaskRepository
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
        int GetTotalCount();
        bool ClassTaskExists(int classTaskId);
    }
}
