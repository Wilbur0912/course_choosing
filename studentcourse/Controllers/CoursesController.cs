using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AuthSystem.Areas.Identity.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using studentcourse.Data;
using studentcourse.Models;



namespace studentcourse.Controllers
{

    [Authorize]
    public class CoursesController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly studentcourseContext _context;

        public CoursesController(UserManager<ApplicationUser> userManager, studentcourseContext context)
        {
            _userManager = userManager;
            _context = context;
        }
        // GET: Courses
        public async Task<IActionResult> Index(int? id, string searchstring, string courseid, string time, string week, string teacher, string department, string category, string credit)
        {
            var CourseChosen = from m in _context.CourseChosen
                               select m;
            var user = await _userManager.GetUserAsync(User);
            var identification = user.StudentId;
            CourseChosen = CourseChosen.Where(x => x.StudentId == identification);
            
            var StuOrTeacher = user.StuOrTeacher;
            bool v = StuOrTeacher;
            if (v) {
                HttpContext.Session.SetString("StuOrTeacher", "Teacher");
                return RedirectToAction(nameof(TeacherSys));
            }
            var courses = from m in _context.Courses
                          select m;


            if (!String.IsNullOrEmpty(searchstring))
            {
                courses = courses.Where(s => s.Course.Contains(searchstring));
            }
            if (!String.IsNullOrEmpty(time))
            {
                courses = courses.Where(s => s.Time.Contains(time));
            }
            if (!String.IsNullOrEmpty(courseid))
            {
                courses = courses.Where(s => s.CourseId.Contains(courseid));
            }

            if (!String.IsNullOrEmpty(category))
            {
                courses = courses.Where(s => s.Category.Contains(category));
            }
            if (!String.IsNullOrEmpty(department))
            {
                courses = courses.Where(s => s.Department.Contains(department));
            }
            if (!String.IsNullOrEmpty(week))
            {
                courses = courses.Where(s => s.Week.Contains(week));
            }
            if (!String.IsNullOrEmpty(teacher))
            {
                courses = courses.Where(s => s.Teacher == teacher);
            }

            IQueryable<string> departmentgenreQuery = from m in _context.Courses
                                                      orderby m.Department
                                                      select m.Department;

            IQueryable<string> categorygenreQuery = from m in _context.Courses
                                                    orderby m.Category
                                                    select m.Category;

            IQueryable<string> WeekQuery = from m in _context.Courses
                                           orderby m.Week
                                           select m.Week;
            var departmentgenres = new Department_Genres
            {
                DepartmentGenres = new SelectList(await departmentgenreQuery.Distinct().ToListAsync()),
                CategoryGenres = new SelectList(await categorygenreQuery.Distinct().ToListAsync()),
                WeekGenres = new SelectList(await WeekQuery.Distinct().ToListAsync()),
                Courses = await courses.ToListAsync(),
                MyCourse = await CourseChosen.ToListAsync()
            };


            if (id == 99)
            {
                TempData["error"] = 99;//"不能重複"

                id = 0;
            }
            else if (id == 98) {
                TempData["error"] = 98; // "已超過25學分";
                id = 0;
            }
            else
            {
                ViewData["error"] = null;
            }
            return View(departmentgenres);
            //return View(await courses.ToListAsync());
        }

        public IActionResult Login(string id, string pass)
        {


            if (!String.IsNullOrEmpty(id))
            {
                string ident;
                var student = _context.StudentsList.Single(s => s.StudentId.Contains(id));

                if (student != null)
                {
                    if (!String.IsNullOrEmpty(pass) && student.PassWord == pass)
                    {
                        ident = id;
                        return RedirectToAction(nameof(Index), new { id = ident });
                    }
                }
                else {
                    TempData["error"] = 97;//無此用戶
                    return View();
                }
            }

            return View();
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(string id)
        {
            ViewBag.StuOrTeacher = HttpContext.Session.GetString("StuOrTeacher");
            if (id == null)
            {
                return NotFound();
            }
            var CourseChosen = from m in _context.CourseChosen
                               select m;


            CourseChosen = CourseChosen.Where(x => x.StudentId == id);

            return View(await CourseChosen.ToListAsync());
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,CourseId,Course,Time,Week,Teacher,Department,Category,Credit")] Courses courses)
        {
            if (ModelState.IsValid)
            {
                _context.Add(courses);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }
        [AllowAnonymous]
        public IActionResult RealIndex() {

            return View();
        }

        public async Task<IActionResult> Add(int? id)
        {
            Courses _course = _context.Courses.Find(id);
            char[] time = _course.Time.ToCharArray();
            string week = _course.Week;
            int ctn = 0;
            var user = await _userManager.GetUserAsync(User);
            var identification = user.StudentId;
            var CourseChosen = _context.CourseChosen.Where(x => x.StudentId == identification);
            foreach (var item in CourseChosen)
            {
                ctn = ctn + item.Credit;
            }
            ctn = ctn + _course.Credit;
            if (ctn > 25)
            {
                return RedirectToAction(nameof(Index), new { id = 98 });
            }
            foreach (var ch in time)
            {
                foreach (var item in CourseChosen)
                {
                    if (item.Time.Contains(ch))
                    {
                        if (item.Week == week)
                        {
                            return RedirectToAction(nameof(Index), new { id = 99 });
                        }
                    }
                }
            }
            string stuname = user.Name;
            string studentid = user.StudentId;
            CourseChosen a = new CourseChosen { StudentId = studentid, StudentName = stuname, CourseId = _course.CourseId, Course = _course.Course, Time = _course.Time, Week = _course.Week, Teacher = _course.Teacher, Department = _course.Department, Category = _course.Category, Credit = _course.Credit };
            _context.CourseChosen.Add(a);

            var studenlist = from k in _context.StudentsList
                             select k;
            foreach (var item in studenlist)
            {
                if (item.StudentId == studentid)
                {
                    _context.StudentsList.Remove(item);
                }
            }

            StudentsList b = new StudentsList();
            b.StudentName = stuname;
            b.StudentId = studentid;
            _context.StudentsList.Add(b);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Del(int? id)
        {
            CourseChosen _course = _context.CourseChosen.Find(id);
            _context.CourseChosen.Remove(_course);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> classSchedule()
        {
            var CourseChosen = from m in _context.CourseChosen
                               select m;
            var user = await _userManager.GetUserAsync(User);
            var identification = user.StudentId;
            CourseChosen = CourseChosen.Where(x => x.StudentId == identification);

            var coursechosen = new MyCourse
            {
                MyCourses = await CourseChosen.ToListAsync()
            };

            return View(coursechosen);
        }

     

        public async Task<IActionResult> TeacherSys()
        {
            ViewBag.StuOrTeacher = HttpContext.Session.GetString("StuOrTeacher");

            return View(await _context.StudentsList.ToListAsync());
        }

        public async Task<IActionResult> TeacherSchedule()
        {
            ViewBag.StuOrTeacher = HttpContext.Session.GetString("StuOrTeacher");
            var courses = from m in _context.Courses
                               select m;
            var user = await _userManager.GetUserAsync(User);
            var identification = user.Name;
            courses = courses.Where(x => x.Teacher == identification);

            return View(await courses.ToListAsync());
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses.FindAsync(id);
            if (courses == null)
            {
                return NotFound();
            }
            return View(courses);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,CourseId,Course,Time,Week,Teacher,Department,Category")] Courses courses)
        {
            if (id != courses.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(courses);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CoursesExists(courses.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(courses);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var courses = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (courses == null)
            {
                return NotFound();
            }

            return View(courses);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var courses = await _context.Courses.FindAsync(id);
            _context.Courses.Remove(courses);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CoursesExists(int id)
        {
            return _context.Courses.Any(e => e.Id == id);
        }
    }
}
