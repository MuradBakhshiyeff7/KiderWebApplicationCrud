using KiderWebApplication.DAL;
using KiderWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KiderWebApplication.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class SliderController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _env;

        public SliderController(AppDbContext context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public async Task<IActionResult> Index()
        {
            var sliders = await _context.Sliders.ToListAsync();
            return View(sliders);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(Slider slider)
        {
            if (!ModelState.IsValid) return View(slider);

            if (slider.Photo == null || !slider.Photo.ContentType.StartsWith("image/"))
            {
                ModelState.AddModelError("Photo", "Only image files are allowed.");
                return View(slider);
            }

            string fileName = Guid.NewGuid() + Path.GetExtension(slider.Photo.FileName);
            string path = Path.Combine(_env.WebRootPath, "uploads/sliders", fileName);

            using (var stream = new FileStream(path, FileMode.Create))
            {
                await slider.Photo.CopyToAsync(stream);
            }

            slider.Image = fileName;
            _context.Sliders.Add(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();
            return View(slider);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Slider model)
        {
            var slider = await _context.Sliders.FindAsync(model.Id);
            if (slider == null) return NotFound();

            if (!ModelState.IsValid) return View(model);

            slider.Title = model.Title;
            slider.Subtitle = model.Subtitle;
            slider.RedirectUrl = model.RedirectUrl;
            slider.IsActive = model.IsActive;

            if (model.Photo != null)
            {
                if (!model.Photo.ContentType.StartsWith("image/"))
                {
                    ModelState.AddModelError("Photo", "Only image files are allowed.");
                    return View(model);
                }

                string fileName = Guid.NewGuid() + Path.GetExtension(model.Photo.FileName);
                string path = Path.Combine(_env.WebRootPath, "uploads/sliders", fileName);

                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await model.Photo.CopyToAsync(stream);
                }

                
                string oldImagePath = Path.Combine(_env.WebRootPath, "uploads/sliders", slider.Image);
                if (System.IO.File.Exists(oldImagePath))
                {
                    System.IO.File.Delete(oldImagePath);
                }

                slider.Image = fileName;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var slider = await _context.Sliders.FindAsync(id);
            if (slider == null) return NotFound();

            string path = Path.Combine(_env.WebRootPath, "uploads/sliders", slider.Image);
            if (System.IO.File.Exists(path))
            {
                System.IO.File.Delete(path);
            }

            _context.Sliders.Remove(slider);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }

}
