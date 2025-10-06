using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {

        ApplicationDBContext db = new ApplicationDBContext();
        public IActionResult Index()
        {
            var brands = db.Brands.AsNoTracking().AsQueryable();

            return View(brands.Select(e=>new
            {
                Id=e.Id,
                Name=e.Name,
                Description=e.Description,
                Status=e.Status,
                e.Img
            }).AsQueryable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Brands());
        }
        [HttpPost]
        public IActionResult Create(Brands brand,IFormFile img)
        {
            if (img is not null && img.Length>0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                using (var stream = System.IO.File.Create(pathname))
                {
                    img.CopyTo(stream);
                }

                brand.Img = filename;
            }
           
                db.Brands.Add(brand);
                db.SaveChanges();
            
            TempData["Notification"] = "Brand Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var brand = db.Brands.FirstOrDefault(b => b.Id == id);
            if (brand is null)
                return NotFound();

            return View(brand);
        }
        [HttpPost]
        public IActionResult Edit(Brands brand,IFormFile? img)
        {
            var brandinDb = db.Brands.AsNoTracking().FirstOrDefault(e=>e.Id == brand.Id);
            if (brandinDb is null)
                return NotFound();
            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                using (var stream = System.IO.File.Create(pathname))
                {
                    img.CopyTo(stream);
                }
                var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", brandinDb.Img);
                System.IO.File.Delete(oldpath);

                brand.Img = filename;
            }else
            {
                brand.Img = brandinDb.Img;
            }
            
                db.Brands.Update(brand);
                db.SaveChanges();
            
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var brand = db.Brands.FirstOrDefault(b => b.Id == id);
            if (brand is null)
                return NotFound();

            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", brand.Img);
            if (System.IO.File.Exists(oldpath))
                System.IO.File.Delete(oldpath);


            db.Brands.Remove(brand);
            db.SaveChanges();

            TempData["Notification"] = "Brand Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
 }

