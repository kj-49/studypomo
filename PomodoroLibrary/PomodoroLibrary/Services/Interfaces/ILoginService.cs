using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Models.Utility;

namespace PomodoroLibrary.Services.Interfaces
{
    public interface ILoginService
    {
        Task<OperationResult> CreateModelAsync(LoginModel model);
        Task<ICollection<LoginModel>> GetModelsAsync(int aspNetUsersId);
        Task<ICollection<LoginModel>> GetAllAsync();
    }
}