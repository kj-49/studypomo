using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using StudyPomo.Library.Data;
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
    private readonly IUnitOfWork _unitOfWork;
    public UserService(IHttpContextAccessor http, UserManager<ApplicationUser> userManager, IUnitOfWork unitOfWork)
    {
        _http = http;
        _userManager = userManager;
        _unitOfWork = unitOfWork;
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

        if (principle == null) return null;

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

    public void UpdateUser(ApplicationUser user)
    {
        _unitOfWork.User.Update(user);
        _unitOfWork.Complete();
    }
}
