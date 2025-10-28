using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class SubjectDAO
    {
        private static SubjectDAO instance = null;
        private static readonly object instanceLock = new object();

        private SubjectDAO() { }

        public static SubjectDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new SubjectDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Subject> GetAllSubjects()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Subjects.ToList();
            }
        }

        public Subject GetSubjectById(int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Subjects
                    .Include(s => s.ClassSubjects)
                    .ThenInclude(cs => cs.StudentClass)
                    .FirstOrDefault(s => s.SubjectId == subjectId);
            }
        }

        public Subject GetSubjectByCode(string subjectCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Subjects
                    .FirstOrDefault(s => s.SubjectCode == subjectCode);
            }
        }

        public List<Subject> GetSubjectsByClassId(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Where(cs => cs.ClassId == classId)
                    .Include(cs => cs.Subject)
                    .Select(cs => cs.Subject)
                    .ToList();
            }
        }

        public void AddSubject(Subject subject)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Subjects.Add(subject);
                context.SaveChanges();
            }
        }

        public void UpdateSubject(Subject subject)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.Entry(subject).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public void DeleteSubject(int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var subject = context.Subjects.Find(subjectId);
                if (subject != null)
                {
                    context.Subjects.Remove(subject);
                    context.SaveChanges();
                }
            }
        }

        public bool SubjectExists(int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Subjects.Any(s => s.SubjectId == subjectId);
            }
        }

        public bool SubjectCodeExists(string subjectCode)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.Subjects.Any(s => s.SubjectCode == subjectCode);
            }
        }
    }
}
