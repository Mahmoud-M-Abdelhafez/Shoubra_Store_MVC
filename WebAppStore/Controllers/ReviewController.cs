using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Controllers
{
    public class ReviewController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly StoreContextDB db;
        public ReviewController( UserManager<AppUser> userManager, StoreContextDB context)
        {
            db = context;
            this.userManager = userManager;
        }
        public async Task<IActionResult> Index()
        {
            var Reviews = db.Reviews.ToList();
            var users = await userManager.Users.ToListAsync();
            ViewBag.Users=users;

            return View(Reviews);
        }

        public async  Task<IActionResult> Add(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound(); // User not found
            }

            return View(user);
        }


        [HttpPost]
        public async Task<IActionResult> Save(AddReviewVM model)
        {
            if (!ModelState.IsValid)
            {
                // Return the view with the model to show validation errors
                return View("Add", model);
            }
            int star = model.stars;

				var review = new Review
                {
                    stars = star,
                    Description = model.Description,
                   UserId = model.UserId
                };

                db.Reviews.Add(review);
                db.SaveChanges();
            TempData["SuccessMessage"] = "Review added successfully!";
            return RedirectToAction("Index");

        }




        public IActionResult Edit(int Id)
        {

            Review review = db.Reviews.SingleOrDefault(r => r.Id == Id);

            if (review == null)
            {
                return NotFound(); 
            }



            return View(review);
        }

        [HttpPost]
        public IActionResult Update(int Id, AddReviewVM rev)
        {
            Review review = db.Reviews.SingleOrDefault(r => r.Id == Id);
            if (review == null) { return NotFound(); }
            int star = rev.stars;
            review.stars = star;
            review.Description = rev.Description;
            db.SaveChanges();
            TempData["SuccessMessage"] = "Review updated successfully!";
            return RedirectToAction("Index");
        }
        
        public IActionResult Delete(int id)
        {
            Review review = db.Reviews.SingleOrDefault(r => r.Id == id);
            if (review == null) { return NotFound(); }
            db.Reviews.Remove(review);
            db.SaveChanges();
            TempData["SuccessMessage"] = "Review deleted successfully!";
            return RedirectToAction("Index");

        }
    }
}
