using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Tables.TaskPriorityEntities;

public class TaskPriority
{
    public int Id { get; set; }
    public string Level { get; set; }
    public string? DisplayHexColor { get; set; }
}
