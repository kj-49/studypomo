using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Models.Tables.LabelEntities;

public class TaskLabel
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string HexColor { get; set; }
    public int UserId { get; set; }
    public ApplicationUser User { get; set; }

    public List<StudyTask> StudyTasks { get; set; } = [];
}
