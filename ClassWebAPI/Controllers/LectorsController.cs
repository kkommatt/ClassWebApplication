using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ClassWebAPI.Models;

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
            return await _context.Lectors.ToListAsync();
        }

        // GET: api/Lectors/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Lector>> GetLector(int id)
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

            return lector;
        }

        // PUT: api/Lectors/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutLector(int id, Lector lector)
        {
            if (id != lector.Id)
            {
                return BadRequest();
            }

            _context.Entry(lector).State = EntityState.Modified;

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

            return NoContent();
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
            _context.Lectors.Add(lector);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetLector", new { id = lector.Id }, lector);
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
