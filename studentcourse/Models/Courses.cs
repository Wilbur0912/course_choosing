using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace studentcourse.Models
{
    public class Courses
    {
        public int Id { get; set; }
        public string CourseId { get; set; }
        public string Course { get; set; }
        public string Week { get; set; }
        public string Time { get; set; }
        public string Teacher { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
        public int Credit { get; set; }
        public string Classroom { get; set; }

    }

    public class Department_Genres
    {
        public List<Courses> Courses { get; set; }
        public SelectList DepartmentGenres { get; set; }
        public SelectList CategoryGenres { get; set; }
        public SelectList WeekGenres { get; set; }
        public List<CourseChosen> MyCourse { get; set; }
        public string Week { get; set; }
        public string SearchString { get; set; }
        public string Time { get; set; }
        public string Teacher { get; set; }
        public string CourseId { get; set; }
        public string Department { get; set; }
        public string Category { get; set; }
    }
}
