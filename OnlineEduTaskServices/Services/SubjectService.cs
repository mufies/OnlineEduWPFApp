using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;

namespace OnlineEduTaskServices.Services
{
    public class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _subjectRepository;

        public SubjectService(ISubjectRepository subjectRepository)
        {
            _subjectRepository = subjectRepository ?? throw new ArgumentNullException(nameof(subjectRepository));
        }

        public List<Subject> GetAllSubjects()
        {
            return _subjectRepository.GetAllSubjects();
        }

        public Subject GetSubjectById(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return _subjectRepository.GetSubjectById(subjectId);
        }

        public Subject GetSubjectByCode(string subjectCode)
        {
            if (string.IsNullOrWhiteSpace(subjectCode))
                throw new ArgumentException("Subject code is required");

            return _subjectRepository.GetSubjectByCode(subjectCode);
        }

        public List<Subject> GetSubjectsByClassId(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _subjectRepository.GetSubjectsByClassId(classId);
        }

        public void AddSubject(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (string.IsNullOrWhiteSpace(subject.SubjectCode))
                throw new ArgumentException("Subject code is required");

            if (string.IsNullOrWhiteSpace(subject.SubjectName))
                throw new ArgumentException("Subject name is required");

            if (subject.Credits <= 0)
                throw new ArgumentException("Credits must be greater than 0");

            _subjectRepository.AddSubject(subject);
        }

        public void UpdateSubject(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (subject.SubjectId <= 0)
                throw new ArgumentException("Valid subject ID is required");

            if (subject.Credits <= 0)
                throw new ArgumentException("Credits must be greater than 0");

            _subjectRepository.UpdateSubject(subject);
        }

        public void DeleteSubject(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            _subjectRepository.DeleteSubject(subjectId);
        }

        public bool SubjectExists(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return _subjectRepository.SubjectExists(subjectId);
        }

        public bool SubjectCodeExists(string subjectCode)
        {
            if (string.IsNullOrWhiteSpace(subjectCode))
                throw new ArgumentException("Subject code is required");

            return _subjectRepository.SubjectCodeExists(subjectCode);
        }

        public int GetTotalSubjectCount()
        {
            return GetAllSubjects().Count;
        }

        public int GetTotalCredits()
        {
            var subjects = GetAllSubjects();
            return subjects.Sum(s => s.Credits);
        }
    }
}
