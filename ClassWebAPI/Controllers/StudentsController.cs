using ClassWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public StudentsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Students
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Student>>> GetStudents()
        {
          if (_context.Students == null)
          {
              return NotFound();
          }
            return await _context.Students.Include(s => s.StudentCourses).ToListAsync();
        }

        // GET: api/Students/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }

            var student = await _context.Students
                .Include(s => s.StudentCourses)
                .FirstOrDefaultAsync(s => s.Id == id);

            if (student == null)
            {
                return NotFound();
            }
            var studentCourses = await _context.StudentCourses
                .Where(sc => sc.StudentId == student.Id)
                .ToListAsync();

            student.StudentCourses = studentCourses;

            return student;
        }

        // PUT: api/Students/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> PutStudent(int id, Student student)
        {
            if (id != student.Id)
            {
                return BadRequest();
            }

            var existingStudent = await _context.Students
            .Include(s => s.StudentCourses)
            .FirstOrDefaultAsync(s => s.Id == id);

            if (existingStudent == null)
            {
                return NotFound();
            }
            var courseIds = new List<int>();
            foreach (var studentCourse in student.StudentCourses)
            {
                courseIds.Add(studentCourse.CourseId);
            }
            _context.StudentCourses.RemoveRange(existingStudent.StudentCourses);
            student.StudentCourses = null;
            existingStudent.FullName = student.FullName;
            existingStudent.Email = student.Email;
            existingStudent.StudentCourses.Clear();
            foreach (var courseId in courseIds)
            {
                var studentCourse = new StudentCourse
                {
                    StudentId = existingStudent.Id,
                    CourseId = courseId,
                    Course = await _context.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync(),
                    Student = await _context.Students.Where(s => s.Id == existingStudent.Id).FirstOrDefaultAsync()
                };
                existingStudent.StudentCourses.Add(studentCourse);
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return existingStudent;
        }

        // POST: api/Students
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Student>> PostStudent(Student student)
        {
          if (_context.Students == null)
          {
              return Problem("Entity set 'ApplicationDbContext.Students'  is null.");
          }
            var courseIds = student.StudentCourses.Select(s => s.CourseId).ToList();
            student.StudentCourses = null;
            _context.Students.Add(student);
            await _context.SaveChangesAsync();  
            var studentId = student.Id;

            foreach (var courseId in courseIds)
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

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetStudent", new { id = student.Id }, student);
        }

        // DELETE: api/Students/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteStudent(int id)
        {
            if (_context.Students == null)
            {
                return NotFound();
            }
            var student = await _context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            _context.Students.Remove(student);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool StudentExists(int id)
        {
            return (_context.Students?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
