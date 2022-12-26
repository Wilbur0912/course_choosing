using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace studentcourse.Models
{
    public class CourseChosen
    {
        public int Id { get; set; }
        public string StudentId { get; set; }
        public string StudentName { get; set; }
        public string CourseId { get; set; }
        public string Course { get; set; }
        public string Time { get; set; }
        public string Week { get; set; }
        public string Teacher { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public int Credit { get; set; }
    }
    public class MyCourse
    {
        public List<CourseChosen> MyCourses { get; set; }

    }
}
