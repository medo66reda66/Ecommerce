using Ecommerce.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Ecommerce.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private ApplicationDBContext db = new ApplicationDBContext();

        public IActionResult Index()
        {
            var Categorys = db.Categores.AsNoTracking().AsQueryable();
            return View(Categorys.AsEnumerable());
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View(new Categores());
        }
        [HttpPost]
        public IActionResult Create(Categores categores)
        {
            db.Categores.Add(categores);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        [HttpGet]
        public IActionResult Edit(int id)
        {
            var Categorys = db.Categores.FirstOrDefault(e=>e.Id==id);
            if (Categorys is null)
            {
                return RedirectToAction("NOTfoundnwe", "Home");
            }
            return View(Categorys);
        }
        [HttpPost]
        public IActionResult Edit(Categores categores)
        {
            db.Categores.Update(categores);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
        public IActionResult Delete(int id)
        {
            var Categorys = db.Categores.FirstOrDefault(e => e.Id == id);
            if (Categorys is null)
            {
                return RedirectToAction("NOTfoundnwe", "Home");
            }
            db.Categores.Remove(Categorys);
            db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }
    }
}

