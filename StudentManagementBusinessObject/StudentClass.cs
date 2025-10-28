using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudentManagementBusinessObject
{
    public class StudentClass
    {
        public int ClassId { get; set; }
        public string ClassCode { get; set; }
        public string ClassName { get; set; }
        public virtual ICollection<Student> Students { get; set; }
        public virtual ICollection<ClassSubject> ClassSubjects { get; set; }
    }
}
