using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;

namespace OnlineEduTaskRepository.Repositories
{
    public class SubjectRepository : ISubjectRepository
    {
        private readonly SubjectDAO _subjectDAO;

        public SubjectRepository()
        {
            _subjectDAO = SubjectDAO.Instance;
        }

        public List<Subject> GetAllSubjects()
        {
            return _subjectDAO.GetAllSubjects();
        }

        public Subject GetSubjectById(int subjectId)
        {
            return _subjectDAO.GetSubjectById(subjectId);
        }

        public Subject GetSubjectByCode(string subjectCode)
        {
            return _subjectDAO.GetSubjectByCode(subjectCode);
        }

        public List<Subject> GetSubjectsByClassId(int classId)
        {
            return _subjectDAO.GetSubjectsByClassId(classId);
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

            if (SubjectCodeExists(subject.SubjectCode))
                throw new InvalidOperationException($"Subject with code {subject.SubjectCode} already exists");

            _subjectDAO.AddSubject(subject);
        }

        public void UpdateSubject(Subject subject)
        {
            if (subject == null)
                throw new ArgumentNullException(nameof(subject));

            if (!SubjectExists(subject.SubjectId))
                throw new InvalidOperationException($"Subject with ID {subject.SubjectId} does not exist");

            if (subject.Credits <= 0)
                throw new ArgumentException("Credits must be greater than 0");

            _subjectDAO.UpdateSubject(subject);
        }

        public void DeleteSubject(int subjectId)
        {
            if (!SubjectExists(subjectId))
                throw new InvalidOperationException($"Subject with ID {subjectId} does not exist");

            _subjectDAO.DeleteSubject(subjectId);
        }

        public bool SubjectExists(int subjectId)
        {
            return _subjectDAO.SubjectExists(subjectId);
        }

        public bool SubjectCodeExists(string subjectCode)
        {
            return _subjectDAO.SubjectCodeExists(subjectCode);
        }
    }
}
