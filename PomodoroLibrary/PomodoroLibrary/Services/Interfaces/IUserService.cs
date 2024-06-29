using Microsoft.AspNetCore.Identity;
using PomodoroLibrary.Models.Identity;

namespace PomodoroLibrary.Services.Interfaces
{
    public interface IUserService
    {
        Task<ApplicationUser?> GetCurrentUserAsync();
    }
}