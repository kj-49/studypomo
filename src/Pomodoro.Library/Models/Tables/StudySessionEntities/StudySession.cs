using Pomodoro.Library.Models.Identity;
using Pomodoro.Library.Models.Tables.StudyTypeEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Models.Tables.StudySessionEntities;

public class StudySession
{
    public int Id { get; set; }
    public ApplicationUser User { get; set; }
    public DateTime Started { get; set; }
    public DateTime Ended { get; set; }
    public StudyType StudyType { get; set; }
}
