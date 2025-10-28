using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskRepository.Interfaces
{
    public interface IClassSubjectRepository
    {
        List<ClassSubject> GetAllClassSubjects();
        ClassSubject GetClassSubject(int classId, int subjectId);
        List<ClassSubject> GetClassSubjectsByClassId(int classId);
        List<ClassSubject> GetClassSubjectsBySubjectId(int subjectId);
        void AddClassSubject(ClassSubject classSubject);
        void AddClassSubject(int classId, int subjectId);
        void RemoveClassSubject(int classId, int subjectId);
        void RemoveAllSubjectsFromClass(int classId);
        bool ClassSubjectExists(int classId, int subjectId);
    }
}
