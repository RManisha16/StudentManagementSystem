using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;


namespace StudentManagementSystem.Controllers
{
    public class StudentsController : Controller
    {
        private readonly ApplicationDbContext _context;
        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Index()
        {
            var students = await _context.Students
                .Include(s => s.StudentCourses)
                .ThenInclude(sc => sc.Course)
                .ToListAsync();
            return View(students);
        }
        public async Task<IActionResult> Enroll(int id)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.StudentId == id);
            var courses = await _context.Courses.ToListAsync();
            var viewModel = new StudentCourseViewModel
            {
                StudentId = student.StudentId,
                StudentName = student.Name,
                AvailableCourses = courses.Select(c => new CourseCheckbox
                {
                    CourseId = c.CourseId,
                    Title = c.Title,
                    IsSelected = student.StudentCourses.Any(sc => sc.CourseId == c.CourseId)
                }).ToList()
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Enroll(StudentCourseViewModel model)
        {
            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.StudentId == model.StudentId);

            student.StudentCourses.Clear();

            foreach (var course in model.AvailableCourses.Where(c => c.IsSelected))
            {
                student.StudentCourses.Add(new StudentCourse
                {
                    StudentId = student.StudentId,
                    CourseId = course.CourseId
                });
            }

            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}