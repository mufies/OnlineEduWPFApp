using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;

namespace OnlineEduTaskServices.Services
{
    public class StudentClassService : IStudentClassService
    {
        private readonly IStudentClassRepository _studentClassRepository;

        public StudentClassService(IStudentClassRepository studentClassRepository)
        {
            _studentClassRepository = studentClassRepository ?? throw new ArgumentNullException(nameof(studentClassRepository));
        }

        public List<StudentClass> GetAllClasses()
        {
            return _studentClassRepository.GetAllClasses();
        }

        public StudentClass GetClassById(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _studentClassRepository.GetClassById(classId);
        }

        public StudentClass GetClassByCode(string classCode)
        {
            if (string.IsNullOrWhiteSpace(classCode))
                throw new ArgumentException("Class code is required");

            return _studentClassRepository.GetClassByCode(classCode);
        }

        public void AddClass(StudentClass studentClass)
        {
            if (studentClass == null)
                throw new ArgumentNullException(nameof(studentClass));

            if (string.IsNullOrWhiteSpace(studentClass.ClassCode))
                throw new ArgumentException("Class code is required");

            if (string.IsNullOrWhiteSpace(studentClass.ClassName))
                throw new ArgumentException("Class name is required");

            _studentClassRepository.AddClass(studentClass);
        }

        public void UpdateClass(StudentClass studentClass)
        {
            if (studentClass == null)
                throw new ArgumentNullException(nameof(studentClass));

            if (studentClass.ClassId <= 0)
                throw new ArgumentException("Valid class ID is required");

            _studentClassRepository.UpdateClass(studentClass);
        }

        public void DeleteClass(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            _studentClassRepository.DeleteClass(classId);
        }

        public bool ClassExists(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _studentClassRepository.ClassExists(classId);
        }

        public bool ClassCodeExists(string classCode)
        {
            if (string.IsNullOrWhiteSpace(classCode))
                throw new ArgumentException("Class code is required");

            return _studentClassRepository.ClassCodeExists(classCode);
        }

        public int GetStudentCountInClass(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _studentClassRepository.GetStudentCountInClass(classId);
        }

        public int GetTotalClassCount()
        {
            return GetAllClasses().Count;
        }
    }
}
