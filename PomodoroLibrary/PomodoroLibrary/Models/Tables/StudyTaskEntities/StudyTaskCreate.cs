using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public class StudyTaskCreate
{
    public string Name { get; set; }
    public SD.TaskPriority TaskPriority { get; set; }
}
