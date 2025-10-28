using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementBusinessObject;

public class Subject
{
    public int SubjectId { get; set; }
    public string SubjectCode { get; set; }
    public string SubjectName { get; set; }
    public int Credits { get; set; }
    public virtual ICollection<ClassSubject> ClassSubjects { get; set; }
}
