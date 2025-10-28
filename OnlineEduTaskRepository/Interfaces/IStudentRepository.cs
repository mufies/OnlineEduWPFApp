using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface IStudentRepository
    {
        List<Student> GetAllStudents();
        Student GetStudentById(int studentId);
        Student GetStudentByCode(string studentCode);
        Student GetStudentByEmail(string email);
        List<Student> GetStudentsByClassId(int classId);
        void AddStudent(Student student);
        void UpdateStudent(Student student);
        void DeleteStudent(int studentId);
        bool StudentExists(int studentId);
        bool StudentCodeExists(string studentCode);
    }
}
