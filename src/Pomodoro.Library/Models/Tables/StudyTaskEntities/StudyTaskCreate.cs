using System.ComponentModel.DataAnnotations;

namespace Pomodoro.Library.Models.Tables.StudyTaskEntities;

public class StudyTaskCreate
{
    [Required]
    public string Name { get; set; }
    [Display(Name = "Priority")]
    public int? TaskPriorityId { get; set; }
    public DateTime? Deadline { get; set; }
    public int? CourseId { get; set; }

    public List<int> TaskLabelIds { get; set; }
}
