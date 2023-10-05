using ClassWebAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ClassWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LectorsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public LectorsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Lectors
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Lector>>> GetLectors()
        {
            if (_context.Lectors == null)
            {
                return NotFound();
            }
            return await _context.Lectors.Include(l => l.LectorCourses).ToListAsync();
        }

        // GET: api/Lectors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lector>> GetLector(int id)
        {
            if (_context.Lectors == null)
            {
                return NotFound();
            }
            var lector = await _context.Lectors
    .Include(l => l.LectorCourses)
    .FirstOrDefaultAsync(l => l.Id == id);

            if (lector == null)
            {
                return NotFound();
            }
            var lectorCourses = await _context.LectorCourses
                .Where(lc => lc.LectorId == lector.Id)
                .ToListAsync();

            lector.LectorCourses = lectorCourses;

            return lector;
        }

        // PUT: api/Lectors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<ActionResult<Lector>> PutLector(int id, Lector lector)
        {
            if (id != lector.Id)
            {
                return BadRequest();
            }

            var existingLector = await _context.Lectors
            .Include(l => l.LectorCourses)
            .FirstOrDefaultAsync(l => l.Id == id);

            if (existingLector == null)
            {
                return NotFound();
            }
            try
            {
                var courseIds = new List<int>();
                foreach (var lectorCourse in lector.LectorCourses)
                {
                    courseIds.Add(lectorCourse.CourseId);
                }
                _context.LectorCourses.RemoveRange(existingLector.LectorCourses);
                lector.LectorCourses = null;
                existingLector.FullName = lector.FullName;
                existingLector.Email = lector.Email;
                existingLector.University = lector.University;
                existingLector.LectorCourses.Clear();
                foreach (var courseId in courseIds)
                {
                    var lectorCourse = new LectorCourse
                    {
                        LectorId = existingLector.Id,
                        CourseId = courseId,
                        Course = await _context.Courses.Where(c => c.Id == courseId).FirstOrDefaultAsync(),
                        Lector = await _context.Lectors.Where(l => l.Id == existingLector.Id).FirstOrDefaultAsync()
                    };
                    existingLector.LectorCourses.Add(lectorCourse);
                }

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LectorExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return Ok(existingLector);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }


        // POST: api/Lectors
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Lector>> PostLector(Lector lector)
        {
            if (_context.Lectors == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Lectors'  is null.");
            }
            try
            {
                var courseIds = lector.LectorCourses.Select(l => l.CourseId).ToList();
                lector.LectorCourses = null;
                _context.Lectors.Add(lector);
                await _context.SaveChangesAsync();
                var lectorId = lector.Id;
                foreach (var courseId in courseIds)
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

                return CreatedAtAction("GetLector", new { id = lector.Id }, lector);
            }
            catch (Exception ex) 
            {
                return BadRequest(ex);
            }
        }

        // DELETE: api/Lectors/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteLector(int id)
        {
            if (_context.Lectors == null)
            {
                return NotFound();
            }
            var lector = await _context.Lectors.FindAsync(id);
            if (lector == null)
            {
                return NotFound();
            }

            _context.Lectors.Remove(lector);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool LectorExists(int id)
        {
            return (_context.Lectors?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
