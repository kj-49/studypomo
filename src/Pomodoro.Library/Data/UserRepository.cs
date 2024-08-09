using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data;

public class UserRepository : GeneralRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext db) : base(db)
    {
    }
}   
