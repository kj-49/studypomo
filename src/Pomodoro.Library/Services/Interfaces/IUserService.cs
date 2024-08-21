using Microsoft.AspNetCore.Identity;
using Pomodoro.Library.Models.Identity;

namespace Pomodoro.Library.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser> GetCurrentUserAsync();
        Task<bool> IsExternallyAuthenticated(ApplicationUser user);
    }
}