using KiderWebApplication.DAL;
using KiderWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KiderWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class PopularTeacherController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public PopularTeacherController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.PopularTeachers.ToListAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var teacher = await _context.PopularTeachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PopularTeacher teacher)
        {
            if (!ModelState.IsValid) return View(teacher);

            if (teacher.ImageFile != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(teacher.ImageFile.ContentType))
                {
                    ModelState.AddModelError("ImageFile", "Yalnız şəkil formatları yükləyə bilərsiniz (jpeg, png, gif, webp)");
                    return View(teacher);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(teacher.ImageFile.FileName);
                string path = Path.Combine(_env.WebRootPath, "uploads", fileName);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await teacher.ImageFile.CopyToAsync(stream);
                }
                teacher.Image = fileName;
            }

            _context.Add(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var teacher = await _context.PopularTeachers.FindAsync(id);
            if (teacher == null) return NotFound();

            return View(teacher);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(PopularTeacher model, IFormFile? ImageFile)
        {
            if (!ModelState.IsValid)
            {
                
                var existing = await _context.PopularTeachers.AsNoTracking().FirstOrDefaultAsync(p => p.Id == model.Id);
                if (existing != null)
                {
                    model.Image = existing.Image;
                }

                return View(model);
            }

            var teacher = await _context.PopularTeachers.FindAsync(model.Id);
            if (teacher == null) return NotFound();

            teacher.FullName = model.FullName;
            teacher.Profession = model.Profession;
           
            teacher.IsActive = model.IsActive;

            if (ImageFile != null)
            {
                var allowedTypes = new[] { "image/jpeg", "image/png", "image/gif", "image/webp" };
                if (!allowedTypes.Contains(model.ImageFile.ContentType))
                {
                    ModelState.AddModelError("ImageFile", "Yalnız şəkil formatı yükləyə bilərsiniz (jpeg, png, gif, webp)");
                    return View(model);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(ImageFile.FileName);
                string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await ImageFile.CopyToAsync(stream);
                }

               
                string oldPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads", teacher.Image);
                if (System.IO.File.Exists(oldPath)) System.IO.File.Delete(oldPath);

                teacher.Image = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var teacher = await _context.PopularTeachers.FindAsync(id);
            if (teacher == null) return NotFound();
            return View(teacher);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacher = await _context.PopularTeachers.FindAsync(id);
            if (teacher == null) return NotFound();

            
            string path = Path.Combine(_env.WebRootPath, "uploads", teacher.Image);
            if (System.IO.File.Exists(path)) System.IO.File.Delete(path);

            _context.PopularTeachers.Remove(teacher);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
