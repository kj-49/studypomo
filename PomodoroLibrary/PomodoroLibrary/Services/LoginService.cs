using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Models.Utility;
using PomodoroLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class LoginService : ILoginService
{
    private readonly ILoginRepository _loginRepository;

    public LoginService(ILoginRepository loginRepository)
    {
        _loginRepository = loginRepository;
    }

    public async Task<OperationResult> CreateModelAsync(LoginModel model)
    {
        return (await _loginRepository.InsertModelAndReturnAsync("uspLogin_Insert", model)).ToNonGenericResult();
    }

    public Task<ICollection<LoginModel>> GetModelsAsync(int aspNetUsersId)
    {
        throw new NotImplementedException();
    }

    public async Task<ICollection<LoginModel>> GetAllAsync()
    {
        return await _loginRepository.GetAllAsync();
    }
}
