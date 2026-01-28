using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dierentuin.Data;
using Dierentuin.Models;

namespace Dierentuin.Controllers
{
    public class EnclosuresController : Controller
    {
        private readonly DierentuinContext _context;

        public EnclosuresController(DierentuinContext context)
        {
            _context = context;
        }

        // GET: Enclosures
        public async Task<IActionResult> Index(string searchString)
        {
            var enclosures = _context.Enclosure.Include(e => e.Zoo).AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                enclosures = enclosures.Where(s => s.Name.Contains(searchString) ||
                                                   s.Climate.ToString().Contains(searchString) ||
                                                   s.HabitatType.ToString().Contains(searchString) ||
                                                   s.SecurityLevel.ToString().Contains(searchString) ||
                                                   (s.Zoo != null && s.Zoo.Name.Contains(searchString)));
            }

            return View(await enclosures.ToListAsync());
        }

        // GET: Enclosures/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Zoo)
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // GET: Enclosures/Create
        public IActionResult Create()
        {
            PopulateViewData();
            return View();
        }

        // POST: Enclosures/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size,ZooId")] Enclosure enclosure)
        {
            if (ModelState.IsValid)
            {
                _context.Add(enclosure);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            PopulateViewData(enclosure);
            return View(enclosure);
        }

        // GET: Enclosures/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure == null)
            {
                return NotFound();
            }

            PopulateViewData(enclosure);
            return View(enclosure);
        }

        // POST: Enclosures/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Climate,HabitatType,SecurityLevel,Size,ZooId")] Enclosure enclosure)
        {
            if (id != enclosure.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(enclosure);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EnclosureExists(enclosure.Id))
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

            PopulateViewData(enclosure);
            return View(enclosure);
        }

        // GET: Enclosures/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (enclosure == null)
            {
                return NotFound();
            }

            return View(enclosure);
        }

        // POST: Enclosures/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var enclosure = await _context.Enclosure.FindAsync(id);
            if (enclosure != null)
            {
                _context.Enclosure.Remove(enclosure);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        // GET: Enclosures/Sunrise/5
        public async Task<IActionResult> Sunrise(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            ViewBag.Actions = enclosure.ActieSunrise();
            return View("Actions", enclosure);
        }

        // GET: Enclosures/Sunset/5
        public async Task<IActionResult> Sunset(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            ViewBag.Actions = enclosure.ActieSunset();
            return View("Actions", enclosure);
        }

        // GET: Enclosures/FeedingTime/5
        public async Task<IActionResult> FeedingTime(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            ViewBag.Actions = enclosure.ActieFeedingTime();
            return View("Actions", enclosure);
        }

        // GET: Enclosures/CheckConstraints/5
        public async Task<IActionResult> CheckConstraints(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var enclosure = await _context.Enclosure
                .Include(e => e.Animals)
                .Include(e => e.Zoo)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (enclosure == null)
            {
                return NotFound();
            }

            ViewBag.Constraints = enclosure.CheckConstraints();
            return View("Constraints", enclosure);
        }

        private bool EnclosureExists(int id)
        {
            return _context.Enclosure.Any(e => e.Id == id);
        }

        // Helper method to populate ViewData for dropdown lists
        private void PopulateViewData(Enclosure? enclosure = null)
        {
            ViewData["ZooId"] = new SelectList(_context.Zoo, "Id", "Name", enclosure?.ZooId ?? null);
            ViewData["ClimateTypes"] = new SelectList(Enum.GetValues(typeof(ClimateEnum))
                .Cast<ClimateEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                }), "Value", "Text");

            ViewData["SecurityLevels"] = new SelectList(Enum.GetValues(typeof(SecurityLevelEnum))
                .Cast<SecurityLevelEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                }), "Value", "Text");

            ViewData["HabitatTypes"] = new SelectList(Enum.GetValues(typeof(HabitatTypeEnum))
                .Cast<HabitatTypeEnum>()
                .Select(e => new SelectListItem
                {
                    Text = e.ToString(),
                    Value = ((int)e).ToString()
                }), "Value", "Text");
        }
    }
}



