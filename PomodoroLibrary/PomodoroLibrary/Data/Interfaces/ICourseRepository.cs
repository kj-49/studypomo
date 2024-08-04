
using PomodoroLibrary.Models.Tables.CourseEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    void Update(Course model);
}
