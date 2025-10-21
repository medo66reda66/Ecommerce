using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDBContext db;// = new ApplicationDBContext();
        private readonly  IRepository<Categores> _db ;

        public CategoryController( ApplicationDBContext db)
        {
            this.db = db;
        }

        public CategoryController(IRepository<Categores> db)
        {
            _db = db;
        }

        public async Task<IActionResult> Index(CancellationToken cancellationToken)
        {
            var Categorys = db.Categores.AsQueryable();

            return View(Categorys.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Categores());
        }
        [HttpPost]
        public async Task<IActionResult> Create(Categores categores,CancellationToken cancellationToken)
        {
            //if (!ModelState.IsValid)
            //{
            //    return View(categores);
            //}
         
            await _db.AddAsync(categores, cancellationToken);
            await _db.commitASync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public async Task<IActionResult> Edit(int id,CancellationToken cancellationToken)
        {
            var Categorys = await _db.GetoneAsync(e => e.Id == id, cancellationToken: cancellationToken);
            if (Categorys is null)
            {
                //ModelState.AddModelError(string.Empty, "Additinal Error");
                return RedirectToAction("NOTfoundnwe", "Home");
            }
            return View(Categorys);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Categores categores ,CancellationToken cancellationToken)
        {
           
            _db.Update(categores);
            await _db.commitASync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var Categorys = await _db.GetoneAsync(e => e.Id == id, cancellationToken:cancellationToken);
            if (Categorys is null)
            {
                return RedirectToAction("NOTfoundnwe", "Home");
            }
           
            _db.Delete(Categorys);
            await _db.commitASync(cancellationToken);
            return RedirectToAction(nameof(Index));
        }
    }
}



