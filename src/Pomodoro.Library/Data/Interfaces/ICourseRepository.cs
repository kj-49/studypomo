
using Pomodoro.Library.Models.Tables.CourseEntities;
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data.Interfaces;

public interface ICourseRepository : IRepository<Course>
{
    void Update(Course model);
}
