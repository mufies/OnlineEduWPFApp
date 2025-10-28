using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Interfaces
{
    public interface IClassSubjectService
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
        int GetSubjectCountInClass(int classId);
        int GetClassCountForSubject(int subjectId);
    }
}
