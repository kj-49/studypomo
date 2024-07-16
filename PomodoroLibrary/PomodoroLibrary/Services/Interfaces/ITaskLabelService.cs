using PomodoroLibrary.Models.Tables.LabelEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services.Interfaces;

public interface ITaskLabelService
{
    Task CreateAsync(TaskLabelCreate taskLabelCreate);
}
