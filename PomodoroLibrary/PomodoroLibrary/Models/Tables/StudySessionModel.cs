using Dapper.Contrib.Extensions;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Models.Tables;

[Table("StudySession")]
public class StudySessionModel
{

    public int Id { get; set; }
    public int AspNetUsersId { get; set; }
    public DateTime Started { get; set; }
    public DateTime Ended { get; set; }
    public int StudyTypeId { get; set; }

}
