using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using GroupPaintOnlineWebApp.Server.Data;
using GroupPaintOnlineWebApp.Shared;

namespace GroupPaintOnlineWebApp.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaintingsController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public PaintingsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: api/Paintings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Painting>>> GetPainting()
        {
            var paintings = await _context.Painting.ToListAsync();
            foreach (var paint in paintings)
            {
                if (paint.UserName != HttpContext.User.Identity.Name)
                    paintings.Remove(paint);
            }
            return paintings;
        }

        // GET: api/Paintings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Painting>> GetPainting(string id)
        {
            var painting = await _context.Painting.FindAsync(id);

            if (painting == null)
            {
                return NotFound();
            }

            return painting;
        }

        // PUT: api/Paintings/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutPainting(string id, Painting painting)
        {
            if (id != painting.PaintingId)
            {
                return BadRequest();
            }

            _context.Entry(painting).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!PaintingExists(id))
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

        // POST: api/Paintings
        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://go.microsoft.com/fwlink/?linkid=2123754.
        [HttpPost]
        public async Task<ActionResult<Painting>> PostPainting(Painting painting)
        {
            _context.Painting.Add(painting);
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateException)
            {
                if (PaintingExists(painting.PaintingId))
                {
                    return Conflict();
                }
                else
                {
                    throw;
                }
            }

            return CreatedAtAction("GetPainting", new { id = painting.PaintingId }, painting);
        }

        // DELETE: api/Paintings/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<Painting>> DeletePainting(string id)
        {
            var painting = await _context.Painting.FindAsync(id);
            if (painting == null)
            {
                return NotFound();
            }

            _context.Painting.Remove(painting);
            await _context.SaveChangesAsync();

            return painting;
        }

        private bool PaintingExists(string id)
        {
            return _context.Painting.Any(e => e.PaintingId == id);
        }
    }
}
