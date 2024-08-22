using Microsoft.AspNetCore.Identity;
using Pomodoro.Library.Models.Identity;
using System.Security.Claims;

namespace Pomodoro.Library.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal cliamsPrinciple);
        Task<bool> IsExternallyAuthenticated(ApplicationUser user);
        void UpdateUser(ApplicationUser user);
    }
}