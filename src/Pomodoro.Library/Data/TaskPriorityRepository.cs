using Pomodoro.Library.Data.Database;
using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data;

public class TaskPriorityRepository : GeneralRepository<TaskPriority>, ITaskPriorityRepository
{
    public TaskPriorityRepository(ApplicationDbContext db) : base(db)
    {
    }
}
