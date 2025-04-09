using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudyPomo.Library.Data;
using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _http;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly ApplicationDbContext _context;
    public UserService(IHttpContextAccessor http, UserManager<ApplicationUser> userManager, ApplicationDbContext context)
    {
        _http = http;
        _userManager = userManager;
        _context = context;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync(ClaimsPrincipal cliamsPrinciple)
    {
        ApplicationUser? user = await _userManager.GetUserAsync(cliamsPrinciple);
        if (user == null)
        {
            throw new Exception("User not found");
        }
        return user;
    }

    public async Task<ApplicationUser> GetCurrentUserAsync()
    {
        ClaimsPrincipal? principle = _http?.HttpContext?.User;

        if (principle == null) throw new Exception("User not found.");

        ApplicationUser? user = await _userManager.GetUserAsync(principle);

        if (user == null) throw new Exception("User not found.");

        return user;
    }

    public async Task<bool> IsExternallyAuthenticated(ApplicationUser user)
    {
        IList<UserLoginInfo> userLogins = await _userManager.GetLoginsAsync(user);

        if (userLogins.Count > 0)
        {
            return true;
        }

        return false;
    }

    public async Task UpdateUser(ApplicationUser user)
    {
        _context.Users.Update(user);
        await _context.SaveChangesAsync();
    }
}
