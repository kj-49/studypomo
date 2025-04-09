using Microsoft.AspNetCore.Identity;
using StudyPomo.Library.Models.Identity;
using System.Security.Claims;

namespace StudyPomo.Library.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal cliamsPrinciple);
        Task<bool> IsExternallyAuthenticated(ApplicationUser user);
        Task UpdateUser(ApplicationUser user);
    }
}