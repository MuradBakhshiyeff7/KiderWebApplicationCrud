using KiderWebApplication.DAL;
using KiderWebApplication.Models;
using KiderWebApplication.ViewModels;
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
            var model = new HomeVM
            {
                PopularTeachers = _context.PopularTeachers.ToList(),
                Sliders = _context.Sliders.Where(s => s.IsActive).ToList()
            };

            return View(model);
        }
    }
}
