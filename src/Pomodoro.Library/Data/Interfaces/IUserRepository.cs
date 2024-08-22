using Pomodoro.Library.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data.Interfaces;

public interface IUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser model);
}
