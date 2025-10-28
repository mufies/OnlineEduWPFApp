using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;

namespace OnlineEduTaskRepository.Repositories
{
    public class StudentRepository : IStudentRepository
    {
        private readonly StudentDAO _studentDAO;

        public StudentRepository()
        {
            _studentDAO = StudentDAO.Instance;
        }

        public List<Student> GetAllStudents()
        {
            return _studentDAO.GetAllStudents();
        }

        public Student GetStudentById(int studentId)
        {
            return _studentDAO.GetStudentById(studentId);
        }

        public Student GetStudentByCode(string studentCode)
        {
            return _studentDAO.GetStudentByCode(studentCode);
        }

        public Student GetStudentByEmail(string email)
        {
            return _studentDAO.GetStudentByEmail(email);
        }

        public List<Student> GetStudentsByClassId(int classId)
        {
            return _studentDAO.GetStudentsByClassId(classId);
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (string.IsNullOrWhiteSpace(student.StudentCode))
                throw new ArgumentException("Student code is required");

            if (string.IsNullOrWhiteSpace(student.FullName))
                throw new ArgumentException("Student full name is required");

            if (StudentCodeExists(student.StudentCode))
                throw new InvalidOperationException($"Student with code {student.StudentCode} already exists");

            _studentDAO.AddStudent(student);
        }

        public void UpdateStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (!StudentExists(student.StudentId))
                throw new InvalidOperationException($"Student with ID {student.StudentId} does not exist");

            _studentDAO.UpdateStudent(student);
        }

        public void DeleteStudent(int studentId)
        {
            if (!StudentExists(studentId))
                throw new InvalidOperationException($"Student with ID {studentId} does not exist");

            _studentDAO.DeleteStudent(studentId);
        }

        public bool StudentExists(int studentId)
        {
            return _studentDAO.StudentExists(studentId);
        }

        public bool StudentCodeExists(string studentCode)
        {
            return _studentDAO.StudentCodeExists(studentCode);
        }
    }
}
