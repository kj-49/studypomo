using PomodoroLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface ILoginRepository : IGeneralRepository<LoginModel>
{
    Task<ICollection<LoginModel>> GetModelsAsync(int aspNetUsersId);
}
