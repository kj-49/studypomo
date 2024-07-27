using System.ComponentModel.DataAnnotations;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public class StudyTaskCreate
{
    [Required]
    public string Name { get; set; }
    [Display(Name = "Priority")]
    public int? TaskPriorityId { get; set; }
    public DateTime? Deadline { get; set; }

    public List<int> TaskLabelIds { get; set; }
}
