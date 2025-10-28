using System;
using System.Collections.Generic;
using StudentManagementBusinessObject;

namespace OnlineEduTaskServices.Interfaces
{
    public interface ISubjectService
    {
        List<Subject> GetAllSubjects();
        Subject GetSubjectById(int subjectId);
        Subject GetSubjectByCode(string subjectCode);
        List<Subject> GetSubjectsByClassId(int classId);
        void AddSubject(Subject subject);
        void UpdateSubject(Subject subject);
        void DeleteSubject(int subjectId);
        bool SubjectExists(int subjectId);
        bool SubjectCodeExists(string subjectCode);
        int GetTotalSubjectCount();
        int GetTotalCredits();
    }
}
