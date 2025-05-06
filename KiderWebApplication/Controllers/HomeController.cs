using KiderWebApplication.DAL;
using KiderWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace KiderWebApplication.Controllers
{
    public class HomeController : Controller
    {
        private readonly AppDbContext _context;

        public HomeController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var popularTeachers = _context.PopularTeachers
                .Where(t => t.IsActive)
                .ToList();

            return View(popularTeachers);
        }
    }
}
