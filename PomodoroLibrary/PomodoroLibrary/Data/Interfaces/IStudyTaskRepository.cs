
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface IStudyTaskRepository : IRepository<StudyTask>
{
    void Update(StudyTask model);
}
