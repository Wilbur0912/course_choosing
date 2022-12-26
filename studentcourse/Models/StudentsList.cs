using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace studentcourse.Models
{
    public class StudentsList
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string PassWord { get; set; }
    }
    public class StudentRightnow
    {
        public string StudentId { get; set; }
        public string StudentNamme { get; set; }
    }
}
