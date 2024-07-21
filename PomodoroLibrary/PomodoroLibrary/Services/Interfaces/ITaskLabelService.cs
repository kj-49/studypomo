using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.TaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services.Interfaces;

public interface ITaskLabelService
{
    Task CreateAsync(TaskLabelCreate taskLabelCreate);
    Task<ICollection<TaskLabel>> GetAllAsync(int userId);
    Task RemoveAsync(int id);
    Task UpdateAsync(TaskLabelUpdate taskLabelUpdate);
}
