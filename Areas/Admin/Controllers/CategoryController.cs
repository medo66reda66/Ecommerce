using Ecommerce.Models;
using Ecommerce.Repository;
using Ecommerce.Repository.IRepository;
using Ecommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE},{DS.EMPLOYEE_ROLE}")]
    public class CategoryController : Controller
    {
        private ApplicationDBContext db;// = new ApplicationDBContext();
        private readonly  IRepository<Categores> _db ;

        public CategoryController( ApplicationDBContext db, IRepository<Categores> dbcategory)
        {
            this.db = db;
            _db = dbcategory;
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
            TempData["Natification"] = "Add category sucsess";
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
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
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Edit(Categores categores ,CancellationToken cancellationToken)
        {
           
            _db.Update(categores);
            await _db.commitASync(cancellationToken);
            TempData["Natification"] = "Edit category sucsess";
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = $"{DS.SUPER_ADMIN_ROLE},{DS.ADMIN_ROLE}")]
        public async Task<IActionResult> Delete(int id,CancellationToken cancellationToken)
        {
            var Categorys = await _db.GetoneAsync(e => e.Id == id, cancellationToken:cancellationToken);
            if (Categorys is null)
            {
                return RedirectToAction("NOTfoundnwe", "Home");
            }
           
            _db.Delete(Categorys);
            await _db.commitASync(cancellationToken);
            TempData["Natification"] = "Delete category sucsess";
            return RedirectToAction(nameof(Index));
        }
    }
}



