using System;
using System.Collections.Generic;

namespace StudentManagementBusinessObject
{
    // Assignment assigned to a class for a subject
    public class ClassTask
    {
        public int ClassTaskId { get; set; }
        public int TaskId { get; set; }
        public int ClassId { get; set; }
        public int SubjectId { get; set; }
        public DateTime AssignedDate { get; set; }
        public DateTime? DueDate { get; set; }
        public int MaxScore { get; set; } // Maximum score for this assignment
        
        // Navigation properties
        public virtual Task Task { get; set; }
        public virtual StudentClass StudentClass { get; set; }
        public virtual Subject Subject { get; set; }
        public virtual ICollection<StudentTask> StudentTasks { get; set; }
    }
}
