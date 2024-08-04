using System.ComponentModel.DataAnnotations;
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
    [Display(Name = "Priority")]
    public int? TaskPriorityId { get; set; }
    public DateTime? Deadline { get; set; }
    public int? CourseId { get; set; }

    public List<int> TaskLabelIds { get; set; }
}
