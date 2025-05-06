using KiderWebApplication.DAL;
using KiderWebApplication.Models;
using Microsoft.AspNetCore.Mvc;

namespace KiderWebApplication.Controllers
{
    public class PopularTeacherController : Controller
    {
        private readonly AppDbContext _context;

        public PopularTeacherController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var activeTeachers = _context.PopularTeachers
                .Where(t => t.IsActive)
                .ToList();

            return View(activeTeachers);
        }
    }
}
