using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

namespace Dierentuin.Controllers
{
    public class ZoosController : Controller
    {
        private readonly DierentuinContext _context;

        public ZoosController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: Zoos
        public async Task<IActionResult> Index(string searchString)
        {
            ViewData["CurrentFilter"] = searchString;

            var zoos = from z in _context.Zoo
                       select z;

            if (!String.IsNullOrEmpty(searchString))
            {
                zoos = zoos.Where(s => s.Name.Contains(searchString));
            }

            return View(await zoos.ToListAsync());
        }

        // GET: Zoos/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoo == null)
            {
                return NotFound();
            }

            return View(zoo);
        }

        // GET: Zoos/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Zoos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name")] Zoo zoo)
        {
            if (ModelState.IsValid)
            {
                _context.Add(zoo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(zoo);
        }

        // GET: Zoos/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo.FindAsync(id);
            if (zoo == null)
            {
                return NotFound();
            }
            return View(zoo);
        }

        // POST: Zoos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name")] Zoo zoo)
        {
            if (id != zoo.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(zoo);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ZooExists(zoo.Id))
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
            return View(zoo);
        }

        // GET: Zoos/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (zoo == null)
            {
                return NotFound();
            }

            return View(zoo);
        }

        // POST: Zoos/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var zoo = await _context.Zoo.FindAsync(id);
            if (zoo != null)
            {
                _context.Zoo.Remove(zoo);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Zoos/Sunrise/5
        public async Task<IActionResult> Sunrise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            ViewBag.Actions = zoo.ActieSunrise();
            return View("Actions", zoo);
        }

        // GET: Zoos/Sunset/5
        public async Task<IActionResult> Sunset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            ViewBag.Actions = zoo.ActieSunset();
            return View("Actions", zoo);
        }

        // GET: Zoos/FeedingTime/5
        public async Task<IActionResult> FeedingTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            ViewBag.Actions = zoo.ActieFeedingTime();
            return View("Actions", zoo);
        }

        // GET: Zoos/CheckConstraints/5
        public async Task<IActionResult> CheckConstraints(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            ViewBag.Constraints = zoo.CheckConstraints();
            return View("Constraints", zoo);
        }

        // GET: Zoos/AutoAssign/5
        public async Task<IActionResult> AutoAssign(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (zoo == null)
            {
                return NotFound();
            }

            return View(zoo);
        }

        // POST: Zoos/AutoAssign/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AutoAssign(int id, bool clearExisting)
        {
            var zoo = await _context.Zoo
                .Include(z => z.Enclosures)
                    .ThenInclude(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

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

                // Reload zoo to get fresh enclosures list
                zoo = await _context.Zoo
                    .Include(z => z.Enclosures)
                    .FirstOrDefaultAsync(m => m.Id == id);
                
                if (zoo == null)
                {
                    return NotFound();
                }
            }

            // Get unassigned animals
            var unassignedAnimals = allAnimals.Where(a => a.EnclosureId == null).ToList();

            // Group animals by security requirement for better assignment
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
                            Climate = ClimateEnum.Temperate, // Default
                            HabitatType = HabitatTypeEnum.Grassland, // Default
                            Size = Math.Max(animal.SpaceRequirement * 2, 1000), // At least double the requirement or 1000 m²
                            ZooId = zoo.Id
                        };

                        _context.Enclosure.Add(newEnclosure);
                        await _context.SaveChangesAsync();

                        animal.EnclosureId = newEnclosure.Id;
                    }
                }
            }

            await _context.SaveChangesAsync();

            TempData["SuccessMessage"] = clearExisting 
                ? "All enclosures cleared and animals reassigned successfully!" 
                : "Unassigned animals have been assigned to enclosures successfully!";

            return RedirectToAction(nameof(Details), new { id = zoo.Id });
        }

        private bool ZooExists(int id)
        {
            return _context.Zoo.Any(e => e.Id == id);
        }
    }
}



