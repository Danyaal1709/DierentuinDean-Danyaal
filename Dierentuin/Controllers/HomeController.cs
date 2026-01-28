using System.Diagnostics;
using Dierentuin.Models;
using Dierentuin.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dierentuin.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly DierentuinContext _context;

        public HomeController(ILogger<HomeController> logger, DierentuinContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: Home/Index
        public async Task<IActionResult> Index()
        {
            var stats = new
            {
                TotalAnimals = await _context.Animal.CountAsync(),
                TotalCategories = await _context.Category.CountAsync(),
                TotalEnclosures = await _context.Enclosure.CountAsync(),
                TotalZoos = await _context.Zoo.CountAsync(),
                AnimalsInEnclosures = await _context.Animal.CountAsync(a => a.EnclosureId != null),
                AnimalsWithCategories = await _context.Animal.CountAsync(a => a.CategoryId != null)
            };

            ViewBag.Stats = stats;
            return View();
        }

        // GET: Home/Privacy
        public IActionResult Privacy()
        {
            return View();
        }

        // GET: Home/Error
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

