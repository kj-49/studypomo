using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data.Interfaces;

public interface ITaskLabelRepository : IRepository<TaskLabel>
{
    void Update(TaskLabel model);
}
