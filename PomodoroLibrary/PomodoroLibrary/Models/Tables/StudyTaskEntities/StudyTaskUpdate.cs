using Microsoft.Build.Framework;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public class StudyTaskUpdate
{
    [Required]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; }
    [Required]
    public int TaskPriorityId { get; set; }
}
