using Microsoft.AspNetCore.Identity;
using WebAppStore.Models;
using WebAppStore.ViewModels;

namespace WebAppStore.Interfaces
{
    public interface IAccountRepository
    {
        Task<SignInResult> LoginAsync(LoginVM model);
        Task<IdentityResult> RegisterAsync(RegisterVM model);
        Task LogoutAsync();
        Task<List<AppUser>> GetAllUsersAsync();
        Task<AppUser?> GetUserByIdAsync(string id);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<IdentityResult> UpdateUserAsync(EditUserVM model);
        Task<IList<string>> GetUserRolesAsync(AppUser user);
        Task<IdentityResult> UpdateUserRoleAsync(string userId, string newRole);
    }
}
