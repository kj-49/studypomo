using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using PomodoroLibrary.Data;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
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
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly IUnitOfWork _unitOfWork;

    public UserService(IHttpContextAccessor http, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _http = http;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
    }

    public async Task<ApplicationUser?> GetCurrentUserAsync()
    {
        ClaimsPrincipal? principle = _http?.HttpContext?.User;

        if (principle == null) return null;

        ApplicationUser? user = await _userManager.GetUserAsync(principle);

        return user;
    }

}
