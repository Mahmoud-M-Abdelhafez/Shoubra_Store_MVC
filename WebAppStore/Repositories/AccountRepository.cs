using WebAppStore.ViewModels;
using WebAppStore.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using WebAppStore.Interfaces;

public class AccountRepository : IAccountRepository
{
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;

    public AccountRepository(SignInManager<AppUser> signInManager, UserManager<AppUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    public async Task<SignInResult> LoginAsync(LoginVM model)
    {
        return await _signInManager.PasswordSignInAsync(model.Username!, model.Password!, model.RememberMe, false);
    }

    public async Task<IdentityResult> RegisterAsync(RegisterVM model)
    {
        AppUser user = new()
        {
            Name = model.Name,
            UserName = model.Email,
            Email = model.Email,
            Address = model.Address
        };

        if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/profiles", model.ProfilePicture.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfilePicture.CopyToAsync(stream);
            }
            user.ProfilePicture = "/images/profiles/" + model.ProfilePicture.FileName;
        }

        var result = await _userManager.CreateAsync(user, model.Password!);
        if (result.Succeeded)
        {
            await _userManager.AddToRoleAsync(user, "User");
            await _signInManager.SignInAsync(user, false);
        }
        return result;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<List<AppUser>> GetAllUsersAsync()
    {
        return await _userManager.Users.ToListAsync();
    }

    public async Task<AppUser?> GetUserByIdAsync(string id)
    {
        return await _userManager.FindByIdAsync(id);
    }

    public async Task<IdentityResult> DeleteUserAsync(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        return await _userManager.DeleteAsync(user);
    }

    public async Task<IdentityResult> UpdateUserAsync(EditUserVM model)
    {
        var user = await _userManager.FindByIdAsync(model.Id);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        user.UserName = model.Email;
        user.Email = model.Email;
        user.Name = model.Name;
        user.Address = model.Address;

        if (model.ProfilePicture != null && model.ProfilePicture.Length > 0)
        {
            var filePath = Path.Combine("wwwroot/images/profiles", model.ProfilePicture.FileName);
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await model.ProfilePicture.CopyToAsync(stream);
            }
            user.ProfilePicture = "/images/profiles/" + model.ProfilePicture.FileName;
        }

        return await _userManager.UpdateAsync(user);
    }

    public async Task<IList<string>> GetUserRolesAsync(AppUser user)
    {
        return await _userManager.GetRolesAsync(user);
    }

    public async Task<IdentityResult> UpdateUserRoleAsync(string userId, string newRole)
    {
        var user = await _userManager.FindByIdAsync(userId);
        if (user == null) return IdentityResult.Failed(new IdentityError { Description = "User not found" });

        var currentRoles = await _userManager.GetRolesAsync(user);
        var removeResult = await _userManager.RemoveFromRolesAsync(user, currentRoles);
        if (!removeResult.Succeeded) return removeResult;

        return await _userManager.AddToRoleAsync(user, newRole);
    }
}
