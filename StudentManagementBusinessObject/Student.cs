using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementBusinessObject;

public class Student
{
    public int StudentId { get; set; }
    public string StudentCode { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public int ClassId { get; set; }
    public virtual StudentClass Class { get; set; }
    public virtual ICollection<Task> Tasks { get; set; }
}