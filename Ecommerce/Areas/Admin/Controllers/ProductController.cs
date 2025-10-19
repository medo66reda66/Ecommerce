using Ecommerce.ViewModel;
using Ecommerce.viwemodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        ApplicationDBContext db = new ApplicationDBContext();
        public IActionResult Index(filterdataVM filterdataVM)
        {
            var products = db.Products.AsNoTracking().AsQueryable();
            products = products.Include(p => p.Brand).Include(p => p.Category);
            if (filterdataVM.name is not null)
            {
                products = products.Where(e => e.Name.Contains(filterdataVM.name.Trim()));
                ViewBag.name = filterdataVM.name;
            }

            if (filterdataVM.minprice is not null)
            {
                products = products.Where(e => e.Price - e.Price * (e.Discount / 100) >= filterdataVM.minprice);
                ViewBag.minprice = filterdataVM.minprice;
            }

            if (filterdataVM.maxprice is not null)
            {
                products = products.Where(e => e.Price - e.Price * (e.Discount / 100) <= filterdataVM.maxprice);
                ViewBag.maxprice = filterdataVM.maxprice;
            }

            if (filterdataVM.catecoryid is not null)
            {
                products = products.Where(e => e.CategoryId == filterdataVM.catecoryid);
                ViewBag.catecoryid = filterdataVM.catecoryid;
            }

            if (filterdataVM.brandid is not null)
            {
                products = products.Where(e => e.BrandId == filterdataVM.brandid);
                ViewBag.brandid = filterdataVM.brandid;
            }

            if (filterdataVM.lessQuantity)
            {
                products = products.OrderBy(e =>e.Quantity);
                ViewBag.lessQuantity = filterdataVM.lessQuantity;
            }
            var category = db.Categores;
            ViewBag.category = category.AsQueryable();
            var brand = db.Brands;
            ViewBag.brands = brand.AsQueryable();
            return View(products.AsEnumerable());
        }

        [HttpGet]
        public IActionResult Create()
        {
            var category = db.Categores;
            var brand = db.Brands;
            return View(new ProductVM
            {
                Products = new Products(),
                Categores = category.ToList(),
                Brands = brand.ToList(),

            });
        }

        [HttpPost]
        public IActionResult Create(Products product,IFormFile img, List<IFormFile>? Supimgs,string[] Colors)
        {
           var database= db.Database.BeginTransaction();
            try
            {
                if (img is not null && img.Length > 0)
                {
                    var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                    var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                    using (var stream = System.IO.File.Create(pathname))
                    {
                        img.CopyTo(stream);
                    }

                    product.MainImg = filename;
                }

                var productid = db.Products.Add(product);
                db.SaveChanges();
                if (Supimgs is not null && Supimgs.Count > 0)
                {
                    foreach (var subimg in Supimgs)
                    {
                        var filename = Guid.NewGuid().ToString() + Path.GetExtension(subimg.FileName);
                        var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Supimg", filename);
                        using (var stream = System.IO.File.Create(pathname))
                        {
                            subimg.CopyTo(stream);
                        }
                        db.ProductSubimgs.Add(new ProductSubimgs
                        {
                            ProductId = productid.Entity.Id,
                            Img = filename
                        });
                    }
                    db.SaveChanges();
                }
                if (Colors is not null && Colors.Length > 0)
                {
                    foreach (var color in Colors)
                    {
                        db.ProductColors.Add(new ProductColors
                        {
                            ProductId = productid.Entity.Id,
                            Color = color
                        });
                    }
                    db.SaveChanges();
                }
                TempData["Notification"] = "Product Created Successfully";
               
                database.Commit();
            }
            catch (Exception ex)
            {
                Console.WriteLine( ex.Message);
                database.Rollback();
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var product = db.Products.Include(e=>e.ProductColors).FirstOrDefault(b => b.Id == id);
            if (product is null)
                return NotFound();
            var category = db.Categores;
            var brand = db.Brands;
            return View(new ProductVM
            {
                Categores = category.ToList(),
                Brands = brand.ToList(),
                Products = product
            });
        }
        [HttpPost]
        public IActionResult Edit(Products product,IFormFile? img, string[] Colors)
        {
            var productinDb = db.Products.AsNoTracking().Include(e => e.ProductColors).FirstOrDefault(e=>e.Id == product.Id);
            if (productinDb is null)
                return NotFound();

            if (img is not null && img.Length > 0)
            {
                var filename = Guid.NewGuid().ToString() + Path.GetExtension(img.FileName);
                var pathname = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", filename);
                using (var stream = System.IO.File.Create(pathname))
                {
                    img.CopyTo(stream);
                }
                var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", productinDb.MainImg);
                System.IO.File.Delete(oldpath);

                product.MainImg = filename;
            }else
            {
                product.MainImg = productinDb.MainImg;
            }


            if (Colors is not null && Colors.Length > 0)
            {
                db.ProductColors.RemoveRange(productinDb.ProductColors);

                product.ProductColors = new List<ProductColors>();
                foreach (var color in Colors)
                {
                    product.ProductColors.Add(new ProductColors
                    {
                        ProductId = product.Id,
                        Color = color
                    });
                }

                db.SaveChanges();
            }

            db.Products.Update(product);
            db.SaveChanges();

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Delete(int id)
        {
            var product = db.Products.FirstOrDefault(b => b.Id == id);
            if (product is null)
                return NotFound();

            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", product.MainImg);
            if (System.IO.File.Exists(oldpath))
                System.IO.File.Delete(oldpath);


            db.Products.Remove(product);
            db.SaveChanges();

            TempData["Notification"] = "Product Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
 }

