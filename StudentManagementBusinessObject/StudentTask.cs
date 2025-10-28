using System;

namespace StudentManagementBusinessObject
{
    // Student's submission for an assignment
    public class StudentTask
    {
        public int StudentTaskId { get; set; }
        public int ClassTaskId { get; set; }
        public int StudentId { get; set; }
        
        // Submission status
        public bool IsSubmitted { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public string? SubmissionContent { get; set; } // Student's answer/submission
        public string? SubmissionFilePath { get; set; } // Path to uploaded file if any
        
        // Grading
        public int? Score { get; set; } // Score received
        public string? TeacherFeedback { get; set; }
        public DateTime? GradedDate { get; set; }
        
        // Navigation properties
        public virtual ClassTask ClassTask { get; set; }
        public virtual Student Student { get; set; }
    }
}
