using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;

namespace OnlineEduTaskRepository.Repositories
{
    public class StudentClassRepository : IStudentClassRepository
    {
        private readonly StudentClassDAO _studentClassDAO;

        public StudentClassRepository()
        {
            _studentClassDAO = StudentClassDAO.Instance;
        }

        public List<StudentClass> GetAllClasses()
        {
            return _studentClassDAO.GetAllClasses();
        }

        public StudentClass GetClassById(int classId)
        {
            return _studentClassDAO.GetClassById(classId);
        }

        public StudentClass GetClassByCode(string classCode)
        {
            return _studentClassDAO.GetClassByCode(classCode);
        }

        public void AddClass(StudentClass studentClass)
        {
            if (studentClass == null)
                throw new ArgumentNullException(nameof(studentClass));

            if (string.IsNullOrWhiteSpace(studentClass.ClassCode))
                throw new ArgumentException("Class code is required");

            if (string.IsNullOrWhiteSpace(studentClass.ClassName))
                throw new ArgumentException("Class name is required");

            if (ClassCodeExists(studentClass.ClassCode))
                throw new InvalidOperationException($"Class with code {studentClass.ClassCode} already exists");

            _studentClassDAO.AddClass(studentClass);
        }

        public void UpdateClass(StudentClass studentClass)
        {
            if (studentClass == null)
                throw new ArgumentNullException(nameof(studentClass));

            if (!ClassExists(studentClass.ClassId))
                throw new InvalidOperationException($"Class with ID {studentClass.ClassId} does not exist");

            _studentClassDAO.UpdateClass(studentClass);
        }

        public void DeleteClass(int classId)
        {
            if (!ClassExists(classId))
                throw new InvalidOperationException($"Class with ID {classId} does not exist");

            int studentCount = GetStudentCountInClass(classId);
            if (studentCount > 0)
                throw new InvalidOperationException($"Cannot delete class with {studentCount} students. Remove students first.");

            _studentClassDAO.DeleteClass(classId);
        }

        public bool ClassExists(int classId)
        {
            return _studentClassDAO.ClassExists(classId);
        }

        public bool ClassCodeExists(string classCode)
        {
            return _studentClassDAO.ClassCodeExists(classCode);
        }

        public int GetStudentCountInClass(int classId)
        {
            return _studentClassDAO.GetStudentCountInClass(classId);
        }
    }
}
