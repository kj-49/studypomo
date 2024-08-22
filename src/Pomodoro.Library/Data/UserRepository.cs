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
    private readonly ApplicationDbContext _db;

    public UserRepository(ApplicationDbContext db) : base(db)
    {
        _db = db;
    }

    public void Update(ApplicationUser model)
    {
        _db.Update(model);
    }
}   
