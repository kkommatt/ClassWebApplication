using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClassWebApplication.Models;

namespace ClassWebApplication.Controllers
{
    public class CoursesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<CoursesController> _logger; 
        private readonly GoogleDriveService _googleDriveService;


        public CoursesController(ApplicationDbContext context, ILogger<CoursesController> logger, GoogleDriveService googleDriveService)
        {
            _context = context;
            _logger = logger;
            _googleDriveService = googleDriveService;
        }

        // GET: Courses
        public async Task<IActionResult> Index()
        {
              return _context.Courses != null ? 
                          View(await _context.Courses.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
        }

        // GET: Courses/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                        .Include(c => c.StudentCourses)
                        .ThenInclude(sc => sc.Student)
                        .Include(c => c.LectorCourses)
                        .ThenInclude(lc => lc.Lector)
                        .FirstOrDefaultAsync(m => m.Id == id);

            return View(course);
        }

        // GET: Courses/Create
        public IActionResult Create()
        {
            ViewBag.Lectors = _context.Lectors.ToList();
            return View();
        }

        // POST: Courses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Title,Description,MaterialLink,LectorCourses")] Course course, List<int> LectorCourses, IFormFile file)
        {
            if (ModelState.IsValid)
            {
                if (LectorCourses != null && LectorCourses.Any())
                {
                    course.LectorCourses = LectorCourses.Select(lectorId => new LectorCourse { LectorId = lectorId }).ToList();
                }

                _context.Courses.Add(course);
                await _context.SaveChangesAsync();

                if (file != null && file.Length > 0)
                {
                    try
                    {
                        byte[] fileContent;
                        using (var memoryStream = new MemoryStream())
                        {
                            file.CopyTo(memoryStream);
                            fileContent = memoryStream.ToArray();
                        }

                        var fileId = _googleDriveService.UploadFile(file.FileName, fileContent);

                        course.MaterialLink = "https://drive.google.com/file/d/" + fileId + "/view";
                        course.MaterialFileIdentifier = fileId;
                        await _context.SaveChangesAsync();
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError($"Error uploading file: {ex.Message}");
                    }
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Lectors = await _context.Lectors.ToListAsync();
            return View(course);
        }

        // GET: Courses/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }
            ViewBag.Lectors = _context.Lectors.ToList();
            return View(course);
        }

        // POST: Courses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Description,MaterialLink,LectorCourses")] Course course, List<int> LectorCourses)
        {
            if (id != course.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    if (LectorCourses != null && LectorCourses.Any())
                    {
                        course.LectorCourses = LectorCourses.Select(lectorId => new LectorCourse { LectorId = lectorId }).ToList();
                    }
                    _context.Update(course);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(course.Id))
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
            return View(course);
        }

        // GET: Courses/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Courses == null)
            {
                return NotFound();
            }

            var course = await _context.Courses
                .FirstOrDefaultAsync(m => m.Id == id);
            if (course == null)
            {
                return NotFound();
            }

            return View(course);
        }

        // POST: Courses/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }

            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.CourseId == id)
                .ToListAsync();
            if (studentCourses != null)
                _context.StudentCourses.RemoveRange(studentCourses);

            var lectorCourses = await _context.LectorCourses
                .Where(lc => lc.CourseId == id)
                .ToListAsync();
            if (lectorCourses != null)
                _context.LectorCourses.RemoveRange(lectorCourses);
            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CourseExists(int id)
        {
          return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        public async Task<IActionResult> DownloadMaterial(string materialFileIdentifier)
        {
            var fileContent = await _googleDriveService.DownloadFile(materialFileIdentifier);

            if (fileContent != null)
            {
                return File(fileContent, "application/octet-stream", "materials.pdf");
            }
            else
            {
                return NotFound();
            }
        }


    }
}
