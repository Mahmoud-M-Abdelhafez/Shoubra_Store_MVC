using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppStore.Interfaces;
using WebAppStore.ViewModels;

namespace WebAppStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountRepository _accountRepository;

        public AccountController(IAccountRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountRepository.LoginAsync(model);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Login successful!";
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid login attempt");
            return View(model);
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterVM model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await _accountRepository.RegisterAsync(model);
            if (result.Succeeded)
            {
                TempData["SuccessMessage"] = "Registration successful!";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError("", error.Description);
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await _accountRepository.LogoutAsync();
            return RedirectToAction("Start", "Home");
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Users()
        {
            var users = await _accountRepository.GetAllUsersAsync();
            return View(users);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            var result = await _accountRepository.DeleteUserAsync(id);
            TempData["SuccessMessage"] = result.Succeeded ? "User deleted successfully!" : "Failed to delete user.";
            return RedirectToAction("Users");
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Role(RoleVM newrole)
        {
            var result = await _accountRepository.UpdateUserRoleAsync(newrole.Id, newrole.RoleName);
            TempData["SuccessMessage"] = result.Succeeded ? "Role updated successfully!" : "Failed to update role.";
            return RedirectToAction("Users");
        }

        [Authorize]
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _accountRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();

            ViewBag.Role = (await _accountRepository.GetUserRolesAsync(user)).FirstOrDefault();
            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> EditUser(EditUserVM model)
        {
            var result = await _accountRepository.UpdateUserAsync(model);
            TempData["SuccessMessage"] = result.Succeeded ? "User updated successfully!" : "Failed to update user.";
            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Details(string id)
        {
            var user = await _accountRepository.GetUserByIdAsync(id);
            if (user == null) return NotFound();
            return View(user);
        }
    }
}
