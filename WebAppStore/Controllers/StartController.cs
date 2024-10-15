using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppStore.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;


namespace WebAppStore.Controllers
{

    public class StartController : Controller
    {
        private readonly UserManager<AppUser> userManager;
        private readonly StoreContextDB db;
        public StartController(UserManager<AppUser> userManager, StoreContextDB context)
        {
            db = context;
            this.userManager = userManager;
        }
        public async  Task<IActionResult> Index()
        {
            var Reviews = db.Reviews.ToList();
            var users = await userManager.Users.ToListAsync();
            ViewBag.Users = users;

            return View(Reviews);
            
        }
    }
}
