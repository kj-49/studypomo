using Microsoft.AspNetCore.Identity;

namespace PomodoroLibrary.Services.Interfaces
{
    public interface IUserService
    {
        Task<IdentityUser<int>?> GetCurrentUserAsync();
    }
}