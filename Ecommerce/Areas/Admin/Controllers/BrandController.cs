using Ecommerce.Repository;
using Ecommerce.viwemodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class BrandController : Controller
    {

        //ApplicationDBContext db = new ApplicationDBContext();
        Repository<Brands> _db = new Repository<Brands>();
        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var brands = await _db.GetAllAsync(cancellationToken: cancellationToken, tracked: false);

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
        public async Task<IActionResult> Create(createBrandVM createBrandVM,CancellationToken cancellationToken)
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
           
              await _db.AddAsync(brand, cancellationToken);
                await _db.commitASync(cancellationToken);

            TempData["Notification"] = "Brand Created Successfully";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var brand = await _db.GetoneAsync(e => e.Id == id, cancellationToken: cancellationToken);
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
        public async Task<IActionResult> Edit(Editebrandvm editebrandvm, CancellationToken cancellationToken )
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
            var brandinDb = await _db.GetoneAsync(e => e.Id == editebrandvm.Id, tracked:false, cancellationToken: cancellationToken);
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
            
               _db.Update(brand);
                await _db.commitASync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id , CancellationToken cancellationToken)
        {
            var brand = await _db.GetoneAsync(e => e.Id == id ,cancellationToken:cancellationToken);
            if (brand is null)
                return NotFound();

            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", brand.Img);
            if (System.IO.File.Exists(oldpath))
                System.IO.File.Delete(oldpath);

            _db.Delete(brand);
            await _db.commitASync(cancellationToken);

            TempData["Notification"] = "Brand Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
 }

