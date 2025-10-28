using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface IStudentClassRepository
    {
        List<StudentClass> GetAllClasses();
        StudentClass GetClassById(int classId);
        StudentClass GetClassByCode(string classCode);
        void AddClass(StudentClass studentClass);
        void UpdateClass(StudentClass studentClass);
        void DeleteClass(int classId);
        bool ClassExists(int classId);
        bool ClassCodeExists(string classCode);
        int GetStudentCountInClass(int classId);
    }
}
