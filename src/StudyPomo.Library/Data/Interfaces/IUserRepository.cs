using StudyPomo.Library.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data.Interfaces;

public interface IUserRepository : IRepository<ApplicationUser>
{
    void Update(ApplicationUser model);
}
