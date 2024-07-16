using System.ComponentModel.DataAnnotations;

namespace PomodoroLibrary.Models.Tables.StudyTaskEntities;

public class StudyTaskCreate
{
    [Required]
    public string Name { get; set; }
    [Required]
    [Display(Name = "Priority")]
    public int TaskPriorityId { get; set; }
    public List<int> StudyLabelIds { get; set; }
}
