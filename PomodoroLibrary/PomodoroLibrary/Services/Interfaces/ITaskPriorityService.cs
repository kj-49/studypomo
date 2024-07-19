using PomodoroLibrary.Models.Tables.TaskPriorityEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services.Interfaces;

public interface ITaskPriorityService
{
    Task<ICollection<TaskPriority>> GetAllAsync();
}
