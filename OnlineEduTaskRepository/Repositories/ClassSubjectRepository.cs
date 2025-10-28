using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;
using OnlineEduTaskDAO;
using OnlineEduTaskRepository.Interfaces;

namespace OnlineEduTaskRepository.Repositories
{
    public class ClassSubjectRepository : IClassSubjectRepository
    {
        private readonly ClassSubjectDAO _classSubjectDAO;

        public ClassSubjectRepository()
        {
            _classSubjectDAO = ClassSubjectDAO.Instance;
        }

        public List<ClassSubject> GetAllClassSubjects()
        {
            return _classSubjectDAO.GetAllClassSubjects();
        }

        public ClassSubject GetClassSubject(int classId, int subjectId)
        {
            return _classSubjectDAO.GetClassSubject(classId, subjectId);
        }

        public List<ClassSubject> GetClassSubjectsByClassId(int classId)
        {
            return _classSubjectDAO.GetClassSubjectsByClassId(classId);
        }

        public List<ClassSubject> GetClassSubjectsBySubjectId(int subjectId)
        {
            return _classSubjectDAO.GetClassSubjectsBySubjectId(subjectId);
        }

        public void AddClassSubject(ClassSubject classSubject)
        {
            if (classSubject == null)
                throw new ArgumentNullException(nameof(classSubject));

            if (ClassSubjectExists(classSubject.ClassId, classSubject.SubjectId))
                throw new InvalidOperationException($"Subject {classSubject.SubjectId} is already assigned to class {classSubject.ClassId}");

            _classSubjectDAO.AddClassSubject(classSubject);
        }

        public void AddClassSubject(int classId, int subjectId)
        {
            if (ClassSubjectExists(classId, subjectId))
                throw new InvalidOperationException($"Subject {subjectId} is already assigned to class {classId}");

            _classSubjectDAO.AddClassSubject(classId, subjectId);
        }

        public void RemoveClassSubject(int classId, int subjectId)
        {
            if (!ClassSubjectExists(classId, subjectId))
                throw new InvalidOperationException($"Subject {subjectId} is not assigned to class {classId}");

            _classSubjectDAO.RemoveClassSubject(classId, subjectId);
        }

        public void RemoveAllSubjectsFromClass(int classId)
        {
            _classSubjectDAO.RemoveAllSubjectsFromClass(classId);
        }

        public bool ClassSubjectExists(int classId, int subjectId)
        {
            return _classSubjectDAO.ClassSubjectExists(classId, subjectId);
        }
    }
}
