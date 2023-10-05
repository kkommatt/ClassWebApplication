using ClassWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CoursesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CoursesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Courses
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetCourses()
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            return await _context.Courses
               .Include(course => course.StudentCourses)
               .ThenInclude(studentCourse => studentCourse.Student)
               .Include(course => course.LectorCourses)
               .ThenInclude(lectorCourse => lectorCourse.Lector)
               .ToListAsync();

        }
        // GET: api/Courses/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Course>> GetCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);

            if (course == null)
            {
                return NotFound();
            }
            course.StudentCourses = await _context.StudentCourses.Where(sc => sc.CourseId == course.Id).ToListAsync();
            course.LectorCourses = await _context.LectorCourses.Where(lc => lc.CourseId == course.Id).ToListAsync();
            return course;
        }

        // PUT: api/Courses/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Course>> PutCourse(int id, Course course)
        {
            if (id != course.Id)
            {
                return BadRequest();
            }

            var existingCourse = await _context.Courses
            .Include(c => c.LectorCourses)
            .Include(c => c.StudentCourses)
            .FirstOrDefaultAsync(c => c.Id == id);

            if (existingCourse == null)
            {
                return NotFound();
            }
            try
            {
                var lectorIds = new List<int>();
                var studentIds = new List<int>();
                foreach (var lectorCourse in course.LectorCourses)
                {
                    lectorIds.Add(lectorCourse.LectorId);
                }
                foreach (var studentCourse in course.StudentCourses)
                {
                    studentIds.Add(studentCourse.StudentId);
                }
                _context.LectorCourses.RemoveRange(existingCourse.LectorCourses);
                _context.StudentCourses.RemoveRange(existingCourse.StudentCourses);
                course.LectorCourses = null;
                course.StudentCourses = null;
                existingCourse.Title = course.Title;
                existingCourse.Description = course.Description;
                existingCourse.MaterialLink = course.MaterialLink;
                existingCourse.LectorCourses.Clear();
                existingCourse.StudentCourses.Clear();

                foreach (var lectorId in lectorIds)
                {
                    var lectorCourse = new LectorCourse
                    {
                        LectorId = lectorId,
                        CourseId = course.Id,
                        Course = await _context.Courses.Where(c => c.Id == course.Id).FirstOrDefaultAsync(),
                        Lector = await _context.Lectors.Where(l => l.Id == lectorId).FirstOrDefaultAsync()
                    };
                    existingCourse.LectorCourses.Add(lectorCourse);
                }
                foreach (var studentId in studentIds)
                {
                    var studentCourse = new StudentCourse
                    {
                        StudentId = studentId,
                        CourseId = course.Id,
                        Course = await _context.Courses.Where(c => c.Id == course.Id).FirstOrDefaultAsync(),
                        Student = await _context.Students.Where(s => s.Id == studentId).FirstOrDefaultAsync()
                    };
                    existingCourse.StudentCourses.Add(studentCourse);
                }
                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CourseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(existingCourse);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }

        // POST: api/Courses
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Course>> PostCourse(Course course)
        {
            if (_context.Courses == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Courses'  is null.");
            }
            try {
                var studentIds = course.StudentCourses.Select(sc => sc.StudentId).ToList();
                var lectorIds = course.LectorCourses.Select(lc => lc.LectorId).ToList();
                course.StudentCourses = null;
                course.LectorCourses = null;
                _context.Courses.Add(course);
                await _context.SaveChangesAsync();
                var courseId = course.Id;

                foreach (var studentId in studentIds)
                {
                    var studentCourse = new StudentCourse()
                    {
                        StudentId = studentId,
                        CourseId = courseId,
                        Course = await _context.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync(),
                        Student = await _context.Students.Where(s => s.Id == studentId).FirstOrDefaultAsync()
                    };
                    _context.StudentCourses.Add(studentCourse);
                }
                foreach (var lectorId in lectorIds)
                {
                    var lectorCourse = new LectorCourse()
                    {
                        LectorId = lectorId,
                        CourseId = courseId,
                        Course = await _context.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync(),
                        Lector = await _context.Lectors.Where(l => l.Id == lectorId).FirstOrDefaultAsync()
                    };
                    _context.LectorCourses.Add(lectorCourse);
                }

                await _context.SaveChangesAsync();

                return CreatedAtAction("GetCourse", new { id = course.Id }, course);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
            }

        // DELETE: api/Courses/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (_context.Courses == null)
            {
                return NotFound();
            }
            var course = await _context.Courses.FindAsync(id);
            if (course == null)
            {
                return NotFound();
            }

            _context.Courses.Remove(course);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CourseExists(int id)
        {
            return (_context.Courses?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
