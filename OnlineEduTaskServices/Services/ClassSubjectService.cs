using System;
using System.Collections.Generic;
using System.Linq;
using StudentManagementBusinessObject;
using OnlineEduTaskRepository.Interfaces;
using OnlineEduTaskServices.Interfaces;

namespace OnlineEduTaskServices.Services
{
    public class ClassSubjectService : IClassSubjectService
    {
        private readonly IClassSubjectRepository _classSubjectRepository;

        public ClassSubjectService(IClassSubjectRepository classSubjectRepository)
        {
            _classSubjectRepository = classSubjectRepository ?? throw new ArgumentNullException(nameof(classSubjectRepository));
        }

        public List<ClassSubject> GetAllClassSubjects()
        {
            return _classSubjectRepository.GetAllClassSubjects();
        }

        public ClassSubject GetClassSubject(int classId, int subjectId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return _classSubjectRepository.GetClassSubject(classId, subjectId);
        }

        public List<ClassSubject> GetClassSubjectsByClassId(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return _classSubjectRepository.GetClassSubjectsByClassId(classId);
        }

        public List<ClassSubject> GetClassSubjectsBySubjectId(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return _classSubjectRepository.GetClassSubjectsBySubjectId(subjectId);
        }

        public void AddClassSubject(ClassSubject classSubject)
        {
            if (classSubject == null)
                throw new ArgumentNullException(nameof(classSubject));

            if (classSubject.ClassId <= 0)
                throw new ArgumentException("Valid class ID is required");

            if (classSubject.SubjectId <= 0)
                throw new ArgumentException("Valid subject ID is required");

            _classSubjectRepository.AddClassSubject(classSubject);
        }

        public void AddClassSubject(int classId, int subjectId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            _classSubjectRepository.AddClassSubject(classId, subjectId);
        }

        public void RemoveClassSubject(int classId, int subjectId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            _classSubjectRepository.RemoveClassSubject(classId, subjectId);
        }

        public void RemoveAllSubjectsFromClass(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            _classSubjectRepository.RemoveAllSubjectsFromClass(classId);
        }

        public bool ClassSubjectExists(int classId, int subjectId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return _classSubjectRepository.ClassSubjectExists(classId, subjectId);
        }

        public int GetSubjectCountInClass(int classId)
        {
            if (classId <= 0)
                throw new ArgumentException("Class ID must be greater than 0");

            return GetClassSubjectsByClassId(classId).Count;
        }

        public int GetClassCountForSubject(int subjectId)
        {
            if (subjectId <= 0)
                throw new ArgumentException("Subject ID must be greater than 0");

            return GetClassSubjectsBySubjectId(subjectId).Count;
        }
    }
}
