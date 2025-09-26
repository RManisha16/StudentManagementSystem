using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StudentManagementSystem.Models;

namespace StudentManagementSystem.Controllers
{
    public class CoursesController : Controller
    {
        
            private readonly ApplicationDbContext _context;

            public CoursesController(ApplicationDbContext context)
            {
                _context = context;
            }

            public async Task<IActionResult> Index()
            {
                return View(await _context.Courses.ToListAsync());
            }

            public IActionResult Create()
            {
                return View();
            }

            [HttpPost]
            public async Task<IActionResult> Create(Course course)
            {
                if (ModelState.IsValid)
                {
                    _context.Courses.Add(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(course);
            }

            public async Task<IActionResult> Edit(int id)
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null) return NotFound();
                return View(course);
            }

            [HttpPost]
            public async Task<IActionResult> Edit(Course course)
            {
                if (ModelState.IsValid)
                {
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                return View(course);
            }

            public async Task<IActionResult> Delete(int id)
            {
                var course = await _context.Courses.FindAsync(id);
                if (course == null) return NotFound();
                return View(course);
            }

            [HttpPost, ActionName("Delete")]
            public async Task<IActionResult> DeleteConfirmed(int id)
            {
                var course = await _context.Courses.FindAsync(id);
                _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }

            public async Task<IActionResult> Details(int id)
            {
                var course = await _context.Courses
                    .Include(c => c.StudentCourses).ThenInclude(sc => sc.Student).FirstOrDefaultAsync(c => c.CourseId == id);

                if (course == null) return NotFound();
                return View(course);
            }
        }
    }
