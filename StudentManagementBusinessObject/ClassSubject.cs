using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;


namespace StudentManagementBusinessObject
{
    public class ClassSubject
    {
        public int ClassId { get; set; }
        public virtual StudentClass StudentClass { get; set; }
        public int SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        public int? TeacherId { get; set; }
        public virtual UserAccount Teacher { get; set; }
    }
}
