using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data.Interfaces;

public interface ITaskLabelRepository : IRepository<TaskLabel>
{
    void Update(TaskLabel model);
}
