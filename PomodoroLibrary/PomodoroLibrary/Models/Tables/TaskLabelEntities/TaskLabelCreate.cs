using PomodoroLibrary.Models.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.LabelEntities;

public class TaskLabelCreate
{
    public string Name { get; set; }
    public string HexColor { get; set; }
    public int UserId { get; set; }
}
