using Pomodoro.Library.Models.Tables.LabelEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Tables.StudyTaskLabelEntities;

public class StudyTaskLabel
{
    public int StudyTaskId { get; set; }
    public StudyTask StudyTask { get; set; }

    public int TaskLabelId { get; set; }
    public TaskLabel TaskLabel { get; set; }
}
