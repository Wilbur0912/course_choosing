using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using studentcourse.Models;

namespace studentcourse.Data
{
    public class studentcourseContext : DbContext
    {
        public studentcourseContext (DbContextOptions<studentcourseContext> options)
            : base(options)
        {
        }

        public DbSet<studentcourse.Models.Courses> Courses { get; set; }
        public DbSet<studentcourse.Models.CourseChosen> CourseChosen { get; set; }
        public DbSet<studentcourse.Models.StudentsList> StudentsList { get; set; }
    }
}
