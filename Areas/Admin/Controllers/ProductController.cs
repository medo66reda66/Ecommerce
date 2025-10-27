using Ecommerce.Repository;
using Ecommerce.Repository.IRepository;
using Ecommerce.ViewModel;
using Ecommerce.viwemodel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {

        ApplicationDBContext db ;
        private readonly productIRepositry _producrRepository;// = new producrRepository();
        private readonly IRepository<Categores> _Categores;//= new Repository<Categores>();
        private readonly IRepository<Brands> _brand;//= new Repository<Brands>();
        private readonly IRepository<ProductSubimgs> _ProductSubimgs;//= new Repository<ProductSubimgs>();
        private readonly IRepository<ProductColors> _ProductColors;//= new Repository<ProductColors>();
        private readonly productcolerIRepositry _productcolerRepository;//= new productcolerRepository();

        public ProductController(ApplicationDBContext db, productIRepositry producrRepository,
            IRepository<Categores> categores, IRepository<Brands> brand, IRepository<ProductSubimgs> productSubimgs
            , IRepository<ProductColors> productColors, productcolerIRepositry productcolerRepository)
        {
            this.db = db;
            _producrRepository = producrRepository;
            _Categores = categores;
            _brand = brand;
            _ProductSubimgs = productSubimgs;
            _ProductColors = productColors;
            _productcolerRepository = productcolerRepository;
        }

        public async Task<IActionResult> Index(filterdataVM filterdataVM ,CancellationToken cancellationToken)
        {
            var products = await _producrRepository.GetAllAsync(includes:[e=>e.Brand,e=>e.Category],tracked:false, cancellationToken:cancellationToken) ;
          
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
            var category = await _Categores.GetAllAsync(cancellationToken:cancellationToken);
            ViewBag.category = category.AsQueryable();
            var brand = await _brand.GetAllAsync(cancellationToken:cancellationToken);
            ViewBag.brands = brand.AsQueryable();
            return View(products.AsEnumerable());
        }

        [HttpGet]
        public async Task<IActionResult> Create(CancellationToken cancellationToken)
        {
            var category = await _Categores.GetAllAsync(cancellationToken:cancellationToken);
            var brand = await _brand.GetAllAsync(cancellationToken: cancellationToken);
            return View(new ProductVM
            {
                Products = new Products(),
                Categores = category.ToList(),
                Brands = brand.ToList(),
               
            });
        }

        [HttpPost]
        public async Task<IActionResult> Create(Products product,IFormFile img, List<IFormFile>? Supimgs,string[] Colors ,CancellationToken cancellationToken)
        {
             
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

                var productid = await _producrRepository.AddAsync(product, cancellationToken: cancellationToken);
                await _producrRepository.commitASync(cancellationToken: cancellationToken);

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
                        await _ProductSubimgs.AddAsync(new ProductSubimgs
                        {
                            ProductId = productid.Id,
                            Img = filename
                        }, cancellationToken: cancellationToken);
                    }
                    await _ProductSubimgs.commitASync(cancellationToken: cancellationToken);
                }
                if (Colors is not null && Colors.Length > 0)
                {
                    foreach (var color in Colors)
                    {
                        await _ProductColors.AddAsync(new ProductColors
                        {
                            ProductId = productid.Id,
                            Color = color
                        }, cancellationToken: cancellationToken);
                    }
                    await _ProductColors.commitASync(cancellationToken: cancellationToken);
                }

                TempData["sucess-Notification"] = "Product Created Successfully";
             
            }
            catch (Exception ex)
            {
               TempData["error-Notification"] = "Product Created error";
            }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var product = await _producrRepository.GetoneAsync(b => b.Id == id,includes: [b => b.ProductSubimgs, c => c.ProductColors],tracked:false, cancellationToken:cancellationToken);
            if (product is null)
                return NotFound();
            var category = await _Categores.GetAllAsync(cancellationToken:cancellationToken);
            var brand = await _brand.GetAllAsync(cancellationToken: cancellationToken);
            return View(new ProductVM
            {
                Categores = category.ToList(),
                Brands = brand.ToList(),
                Products = product,
               
            });
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Products product,IFormFile? img, string[] Colors ,CancellationToken cancellationToken)
        {
            var productinDb = await _producrRepository.GetoneAsync(e => e.Id == product.Id, includes: [c=>c.ProductColors],tracked:false ,cancellationToken : cancellationToken);
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
            _producrRepository.Update(product);
            await _producrRepository.commitASync(cancellationToken: cancellationToken);

            if (Colors is not null && Colors.Length > 0)
            {
                _productcolerRepository.RemoveRang(productinDb.ProductColors);

                product.ProductColors = new List<ProductColors>();
                foreach (var color in Colors)
                {
                    product.ProductColors.Add(new ProductColors
                    {
                        ProductId = product.Id,
                        Color = color
                    });
                }

                await _productcolerRepository.commitASync(cancellationToken: cancellationToken);
            }

            
            

            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int id ,CancellationToken cancellationToken)
        {
            var product = await _producrRepository.GetoneAsync(b => b.Id == id, includes: [b => b.ProductSubimgs, c => c.ProductColors], tracked: false, cancellationToken: cancellationToken);
            if (product is null)
                return NotFound();

            var oldpath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\image", product.MainImg);
            if (System.IO.File.Exists(oldpath))
                System.IO.File.Delete(oldpath);


            _producrRepository.Update(product);
            await _producrRepository.commitASync(cancellationToken: cancellationToken);

            TempData["Notification"] = "Product Deleted Successfully";

            return RedirectToAction(nameof(Index));
        }
    }
 }

