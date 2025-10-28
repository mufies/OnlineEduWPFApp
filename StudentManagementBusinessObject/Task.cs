using System;
using System.Collections.Generic;

namespace StudentManagementBusinessObject
{
    // Assignment template created by teacher
    public class Task
    {
        public int TaskId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string? Description { get; set; }
        public DateTime CreatedDate { get; set; }
        public int CreatedByTeacherId { get; set; } // UserAccount with Role = "Teacher"

        // Navigation properties
        public virtual UserAccount CreatedByTeacher { get; set; }
        public virtual ICollection<ClassTask> ClassTasks { get; set; }
    }
}
