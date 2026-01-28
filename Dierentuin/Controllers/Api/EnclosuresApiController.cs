using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dierentuin.Controllers.Api
{
    [Route("api/Enclosures")]
    [ApiController]
    public class EnclosuresApiController : ControllerBase
    {
        private readonly DierentuinContext _context;

        public EnclosuresApiController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: api/Enclosures
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enclosure>>> GetEnclosures()
        {
            return await _context.Enclosure.Include(e => e.Zoo).Include(e => e.Animals).ToListAsync();
        }

        // GET: api/Enclosures/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Enclosure>> GetEnclosure(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Zoo)
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return enclosure;
        }

        // PUT: api/Enclosures/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEnclosure(int id, Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return BadRequest();
            }

            _context.Entry(enclosure).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EnclosureExists(id))
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

        // POST: api/Enclosures
        [HttpPost]
        public async Task<ActionResult<Enclosure>> PostEnclosure(Enclosure enclosure)
        {
            _context.Enclosure.Add(enclosure);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEnclosure", new { id = enclosure.Id }, enclosure);
        }

        // DELETE: api/Enclosures/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnclosure(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            _context.Enclosure.Remove(enclosure);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Enclosures/5/Sunrise
        [HttpGet("{id}/Sunrise")]
        public async Task<ActionResult<IEnumerable<string>>> GetSunriseActions(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure.ActieSunrise());
        }

        // GET: api/Enclosures/5/Sunset
        [HttpGet("{id}/Sunset")]
        public async Task<ActionResult<IEnumerable<string>>> GetSunsetActions(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure.ActieSunset());
        }

        // GET: api/Enclosures/5/FeedingTime
        [HttpGet("{id}/FeedingTime")]
        public async Task<ActionResult<IEnumerable<string>>> GetFeedingTimeActions(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure.ActieFeedingTime());
        }

        // GET: api/Enclosures/5/CheckConstraints
        [HttpGet("{id}/CheckConstraints")]
        public async Task<ActionResult<IEnumerable<string>>> GetCheckConstraints(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .Include(e => e.Zoo)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure.CheckConstraints());
        }

        // GET: api/Enclosures/5/Animals
        [HttpGet("{id}/Animals")]
        public async Task<ActionResult<IEnumerable<Animal>>> GetEnclosureAnimals(int id)
        {
            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(e => e.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            return Ok(enclosure.Animals);
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }
    }
}
