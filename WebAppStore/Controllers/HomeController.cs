using Microsoft.AspNetCore.Mvc;
using WebAppStore.Models;
using System.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace WebAppStore.Controllers
{
    [Authorize]

    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly StoreContextDB db;
        private readonly UserManager<AppUser> userManager;
        public HomeController(StoreContextDB context, UserManager<AppUser> userManager)
        {
            db = context;
            this.userManager = userManager;
        }


        public async Task<IActionResult> Index()
        {
            var categories = db.Categories.ToList();
            ViewBag.Products = db.Products.ToList();
            ViewBag.Cats = db.Categories.Count();
            ViewBag.Prodcount = db.Products.Count();
            ViewBag.UsersCount = await userManager.Users.CountAsync();
            var productimages = db.ProductImages.ToList();
            ViewBag.images = productimages;
            var Reviews = db.Reviews.ToList();
            ViewBag.Reviews = Reviews;
            var users = await userManager.Users.ToListAsync();
            ViewBag.Users = users;

            return View(categories);
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
