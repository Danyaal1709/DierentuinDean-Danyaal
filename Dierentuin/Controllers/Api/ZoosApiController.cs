using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Dierentuin.Controllers.Api
{
    [Route("api/Zoos")]
    [ApiController]
    public class ZoosApiController : ControllerBase
    {
        private readonly DierentuinContext _context;

        public ZoosApiController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: api/Zoos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Zoo>>> GetZoos()
        {
            return await _context.Zoo.Include(z => z.Enclosures).ToListAsync();
        }

        // GET: api/Zoos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Zoo>> GetZoo(int id)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return zoo;
        }

        // PUT: api/Zoos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutZoo(int id, Zoo zoo)
        {
            if (id != zoo.Id)
            {
                return BadRequest();
            }

            _context.Entry(zoo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ZooExists(id))
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

        // POST: api/Zoos
        [HttpPost]
        public async Task<ActionResult<Zoo>> PostZoo(Zoo zoo)
        {
            _context.Zoo.Add(zoo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetZoo", new { id = zoo.Id }, zoo);
        }

        // DELETE: api/Zoos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteZoo(int id)
        {
            var zoo = await _context.Zoo.FindAsync(id);
            if (zoo == null)
            {
                return NotFound();
            }

            _context.Zoo.Remove(zoo);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/Zoos/5/Sunrise
        [HttpGet("{id}/Sunrise")]
        public async Task<ActionResult<IEnumerable<string>>> GetSunriseActions(int id)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return Ok(zoo.ActieSunrise());
        }

        // GET: api/Zoos/5/Sunset
        [HttpGet("{id}/Sunset")]
        public async Task<ActionResult<IEnumerable<string>>> GetSunsetActions(int id)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return Ok(zoo.ActieSunset());
        }

        // GET: api/Zoos/5/FeedingTime
        [HttpGet("{id}/FeedingTime")]
        public async Task<ActionResult<IEnumerable<string>>> GetFeedingTimeActions(int id)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return Ok(zoo.ActieFeedingTime());
        }

        // GET: api/Zoos/5/CheckConstraints
        [HttpGet("{id}/CheckConstraints")]
        public async Task<ActionResult<IEnumerable<string>>> GetCheckConstraints(int id)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return Ok(zoo.CheckConstraints());
        }

        // POST: api/Zoos/5/AutoAssign
        [HttpPost("{id}/AutoAssign")]
        public async Task<IActionResult> AutoAssign(int id, [FromBody] bool clearExisting)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(z => z.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            // Get all animals (including those not yet assigned)
            var allAnimals = await _context.Animal.ToListAsync();

            if (clearExisting)
            {
                // Clear all existing assignments
                foreach (var animal in allAnimals)
                {
                    animal.EnclosureId = null;
                }
                await _context.SaveChangesAsync();

                // Remove all existing enclosures for this zoo
                var enclosuresToRemove = zoo.Enclosures.ToList();
                foreach (var enclosure in enclosuresToRemove)
                {
                    _context.Enclosure.Remove(enclosure);
                }
                await _context.SaveChangesAsync();

                // Reload zoo
                zoo = await _context.Zoo
                    .Include(z => z.Enclosures)
                    .FirstOrDefaultAsync(z => z.Id == id);
                
                if (zoo == null)
                {
                    return NotFound();
                }
            }

            // Get unassigned animals
            var unassignedAnimals = allAnimals.Where(a => a.EnclosureId == null).ToList();

            // Group animals by security requirement
            var animalsBySecurity = unassignedAnimals.GroupBy(a => a.SecurityRequirement).ToList();

            foreach (var securityGroup in animalsBySecurity)
            {
                var securityLevel = securityGroup.Key;
                var animalsInGroup = securityGroup.ToList();

                // Try to assign to existing enclosures first
                var availableEnclosures = zoo.Enclosures
                    .Where(e => e.SecurityLevel >= securityLevel)
                    .OrderBy(e => e.Animals!.Sum(a => a.SpaceRequirement))
                    .ToList();

                foreach (var animal in animalsInGroup)
                {
                    bool assigned = false;

                    // Try existing enclosures
                    foreach (var enclosure in availableEnclosures)
                    {
                        var currentSpaceUsed = enclosure.Animals!.Sum(a => a.SpaceRequirement);
                        if (currentSpaceUsed + animal.SpaceRequirement <= enclosure.Size)
                        {
                            animal.EnclosureId = enclosure.Id;
                            assigned = true;
                            break;
                        }
                    }

                    // If no existing enclosure fits, create a new one
                    if (!assigned)
                    {
                        var newEnclosure = new Enclosure
                        {
                            Name = $"Auto-Enclosure {securityLevel} {zoo.Enclosures!.Count + 1}",
                            SecurityLevel = securityLevel,
                            Climate = ClimateEnum.Temperate,
                            HabitatType = HabitatTypeEnum.Grassland,
                            Size = System.Math.Max(animal.SpaceRequirement * 2, 1000),
                            ZooId = zoo.Id
                        };

                        _context.Enclosure.Add(newEnclosure);
                        await _context.SaveChangesAsync();

                        animal.EnclosureId = newEnclosure.Id;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok(new { message = clearExisting 
                ? "All enclosures cleared and animals reassigned successfully!" 
                : "Unassigned animals have been assigned to enclosures successfully!" });
        }

        private bool ZooExists(int id)
        {
            return _context.Zoo.Any(e => e.Id == id);
        }
    }
}
