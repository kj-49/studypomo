
using Pomodoro.Library.Models.Tables.StudyTaskEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Data.Interfaces;

public interface IStudyTaskRepository : IRepository<StudyTask>
{
    void Update(StudyTask model);
}
