using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly StoreContextDB db;
        public CategoryController(StoreContextDB context)
        {
            db = context;
        }
        [Authorize]
        public async Task<IActionResult> Index()
        {
            var Cats = await db.Categories.ToListAsync();
            return View(Cats);
        }
        [Authorize]
        public IActionResult Details(int Id)
        {
            var Products = db.Products.Where(p => p.CategoryId == Id).ToList();
            var productimages = db.ProductImages.ToList();
            ViewBag.images = productimages;
            var Cat = db.Categories.SingleOrDefault(c=>c.Id==Id);
            ViewBag.category = Cat;
            return View(Products);
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Add()
        {

            return View();
        }

        [HttpPost]
           public IActionResult Save(AddCategoryVM model)
        {
            if (!ModelState.IsValid)
            {
                // If the model state is invalid, return the view with the current model to show validation errors
                return View(model);
            }

            try
            {
                var cat = new Category
                {
                    Name = model.Name,
                    Description = model.Description,
                    Icon = model.Icon
                };

                db.Categories.Add(cat);
                db.SaveChanges();

                TempData["SuccessMessage"] = "Category added successfully!";
                return RedirectToAction("Index");
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError("", "An error occurred while saving the category. Please try again.");
                return View(model);
            }
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Edit(int id)
        {
            Category cat = db.Categories.SingleOrDefault(p => p.Id == id);
            if (cat == null)
            {
                return NotFound();
            }
            return View(cat);
        }
        [HttpPost]
        public IActionResult Update(int id , Category Ct)
        {
            Category cat = db.Categories.SingleOrDefault(p => p.Id == id);
            if (cat == null) { return NotFound(); }
            cat.Name = Ct.Name;
            cat.Description = Ct.Description;
            cat.Icon = Ct.Icon;
            db.SaveChanges();
            TempData["SuccessMessage"] = "Category updated successfully!";
            return RedirectToAction("Index");
        }
        [Authorize(Roles = "Admin")]
        public IActionResult Delete(int id)
        {
            Category cat = db.Categories.SingleOrDefault(p => p.Id == id);
            if (cat == null) { return NotFound(); }
            db.Categories.Remove(cat);
            TempData["SuccessMessage"] = "Category deleted successfully!";
            db.SaveChanges();
            return RedirectToAction("Index");
       
        }



    }
}
