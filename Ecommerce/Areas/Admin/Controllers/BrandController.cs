using Ecommerce.viwemodel;
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
            return View(new createBrandVM() );
        }
        [HttpPost]
        public IActionResult Create(createBrandVM createBrandVM)
        {
            if (!ModelState.IsValid)
            {
                return View(createBrandVM);
            }
            Brands brand = new Brands()
            {
                Name = createBrandVM.Name,
                Description = createBrandVM.Description,
                Status = createBrandVM.Status,
            };
            if (createBrandVM.Img is not null && createBrandVM.Img.Length>0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(createBrandVM.Img.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                using (var stream = System.IO.File.Create(pathname))
                {
                    createBrandVM.Img.CopyTo(stream);
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

            return View(new Editebrandvm()
            {
                Id = brand.Id,
                Name = brand.Name,
                Description = brand.Description,
                Status = brand.Status,
                Img = brand.Img
            });
        }
        [HttpPost]
        public IActionResult Edit(Editebrandvm editebrandvm)
        {
            if (!ModelState.IsValid)
            {
                return View(editebrandvm);
            }
            Brands brand = new Brands()
            {
                Id = editebrandvm.Id,
                Name = editebrandvm.Name,
                Description = editebrandvm.Description,
                Status = editebrandvm.Status,
            };
            var brandinDb = db.Brands.AsNoTracking().FirstOrDefault(e=>e.Id == brand.Id);
            if (brandinDb is null)
                return NotFound();
            if (editebrandvm.newImg is not null && editebrandvm.newImg.Length > 0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(editebrandvm.newImg.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                using (var stream = System.IO.File.Create(pathname))
                {
                    editebrandvm.newImg.CopyTo(stream);
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

