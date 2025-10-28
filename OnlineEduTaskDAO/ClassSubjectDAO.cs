using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using StudentManagementBusinessObject;

namespace OnlineEduTaskDAO
{
    public class ClassSubjectDAO
    {
        private static ClassSubjectDAO instance = null;
        private static readonly object instanceLock = new object();

        private ClassSubjectDAO() { }

        public static ClassSubjectDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ClassSubjectDAO();
                    }
                    return instance;
                }
            }
        }

        public List<ClassSubject> GetAllClassSubjects()
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Include(cs => cs.StudentClass)
                    .Include(cs => cs.Subject)
                    .ToList();
            }
        }

        public ClassSubject GetClassSubject(int classId, int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Include(cs => cs.StudentClass)
                    .Include(cs => cs.Subject)
                    .FirstOrDefault(cs => cs.ClassId == classId && cs.SubjectId == subjectId);
            }
        }

        public List<ClassSubject> GetClassSubjectsByClassId(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Include(cs => cs.Subject)
                    .Where(cs => cs.ClassId == classId)
                    .ToList();
            }
        }

        public List<ClassSubject> GetClassSubjectsBySubjectId(int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Include(cs => cs.StudentClass)
                    .Where(cs => cs.SubjectId == subjectId)
                    .ToList();
            }
        }

        public void AddClassSubject(ClassSubject classSubject)
        {
            using (var context = new StudentManagementDbContext())
            {
                context.ClassSubjects.Add(classSubject);
                context.SaveChanges();
            }
        }

        public void AddClassSubject(int classId, int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var classSubject = new ClassSubject
                {
                    ClassId = classId,
                    SubjectId = subjectId
                };
                context.ClassSubjects.Add(classSubject);
                context.SaveChanges();
            }
        }

        public void RemoveClassSubject(int classId, int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var classSubject = context.ClassSubjects
                    .FirstOrDefault(cs => cs.ClassId == classId && cs.SubjectId == subjectId);
                
                if (classSubject != null)
                {
                    context.ClassSubjects.Remove(classSubject);
                    context.SaveChanges();
                }
            }
        }

        public void RemoveAllSubjectsFromClass(int classId)
        {
            using (var context = new StudentManagementDbContext())
            {
                var classSubjects = context.ClassSubjects
                    .Where(cs => cs.ClassId == classId)
                    .ToList();
                
                context.ClassSubjects.RemoveRange(classSubjects);
                context.SaveChanges();
            }
        }

        public bool ClassSubjectExists(int classId, int subjectId)
        {
            using (var context = new StudentManagementDbContext())
            {
                return context.ClassSubjects
                    .Any(cs => cs.ClassId == classId && cs.SubjectId == subjectId);
            }
        }
    }
}
