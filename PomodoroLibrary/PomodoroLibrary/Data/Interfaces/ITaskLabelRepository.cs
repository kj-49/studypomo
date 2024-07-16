using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface ITaskLabelRepository : IRepository<TaskLabel>
{
    void Update(TaskLabel model);
}
