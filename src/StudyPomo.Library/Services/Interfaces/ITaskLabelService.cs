using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.TaskLabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services.Interfaces;

public interface ITaskLabelService
{
    Task CreateAsync(TaskLabelCreate taskLabelCreate);
    Task<ICollection<TaskLabel>> GetAllAsync(int userId);
    Task<TaskLabel> GetAsync(int id);
    Task RemoveAsync(int id);
    Task UpdateAsync(TaskLabelUpdate taskLabelUpdate);
}
