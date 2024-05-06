using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PomodoroLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class UserService : IUserService
{
    private readonly IHttpContextAccessor _http;
    private readonly UserManager<IdentityUser<int>> _userManager;

    public UserService(IHttpContextAccessor http, UserManager<IdentityUser<int>> userManager)
    {
        _http = http;
        _userManager = userManager;
    }

    public async Task<IdentityUser<int>?> GetCurrentUserAsync()
    {
        ClaimsPrincipal? appUser = _http?.HttpContext?.User;

        if (appUser == null) return null;

        IdentityUser<int>? user = await _userManager.GetUserAsync(appUser);

        if (user == null) return null;

        return user;
    }

}
