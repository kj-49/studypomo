using Dapper.Contrib.Extensions;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables;

[Table("Task")]
public class TaskModel
{

    public int Id { get; set; }
    public int AspNetUsersId { get; set; }
    public string Name { get; set; }
    public bool Completed { get; set; }
    public DateTime DateCreated { get; set; }
    public DateTime? DateCompleted { get; set; }
    public int TaskPriorityId { get; set; }

}
