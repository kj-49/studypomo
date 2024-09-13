using StudyPomo.Library.Data.Database;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data;

public class TaskPriorityRepository : GeneralRepository<TaskPriority>, ITaskPriorityRepository
{
    public TaskPriorityRepository(ApplicationDbContext db) : base(db)
    {
    }
}
