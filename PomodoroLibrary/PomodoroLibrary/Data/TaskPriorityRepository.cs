using PomodoroLibrary.Data.Database;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data;

public class TaskPriorityRepository : GeneralRepository<TaskPriority>, ITaskPriorityRepository
{
    public TaskPriorityRepository(ApplicationDbContext db) : base(db)
    {
    }
}
