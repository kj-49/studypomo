using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data;

public class UserRepository : GeneralRepository<ApplicationUser>, IUserRepository
{
    public UserRepository(ApplicationDbContext db) : base(db)
    {
    }
}   
