using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;

namespace OnlineEduTaskServices.Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepository _studentRepository;

        public StudentService(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository ?? throw new ArgumentNullException(nameof(studentRepository));
        }

        public List<Student> GetAllStudents()
        {
            return _studentRepository.GetAllStudents();
        }

        public Student GetStudentById(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");

            return _studentRepository.GetStudentById(studentId);
        }

        public Student GetStudentByCode(string studentCode)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                throw new ArgumentException("Student code is required");

            return _studentRepository.GetStudentByCode(studentCode);
        }

        public Student GetStudentByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            return _studentRepository.GetStudentByEmail(email);
        }

        public List<Student> GetStudentsByClassId(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _studentRepository.GetStudentsByClassId(classId);
        }

        public void AddStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (string.IsNullOrWhiteSpace(student.StudentCode))
                throw new ArgumentException("Student code is required");

            if (string.IsNullOrWhiteSpace(student.FullName))
                throw new ArgumentException("Full name is required");

            if (string.IsNullOrWhiteSpace(student.Email))
                throw new ArgumentException("Email is required");

            _studentRepository.AddStudent(student);
        }

        public void UpdateStudent(Student student)
        {
            if (student == null)
                throw new ArgumentNullException(nameof(student));

            if (student.StudentId <= 0)
                throw new ArgumentException("Valid student ID is required");

            _studentRepository.UpdateStudent(student);
        }

        public void DeleteStudent(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");

            _studentRepository.DeleteStudent(studentId);
        }

        public bool StudentExists(int studentId)
        {
            if (studentId <= 0)
                throw new ArgumentException("Student ID must be greater than 0");

            return _studentRepository.StudentExists(studentId);
        }

        public bool StudentCodeExists(string studentCode)
        {
            if (string.IsNullOrWhiteSpace(studentCode))
                throw new ArgumentException("Student code is required");

            return _studentRepository.StudentCodeExists(studentCode);
        }

        public int GetTotalStudentCount()
        {
            return GetAllStudents().Count;
        }
    }
}
