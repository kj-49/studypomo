using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Data.Interfaces;

public interface IUnitOfWork
{
    IStudyTaskRepository StudyTask { get; }
    ITaskPriorityRepository TaskPriority { get; }
    IUserRepository User { get; }
    ITaskLabelRepository TaskLabel { get; }
    IStudyTaskLabelRepository StudyTaskLabel { get; }
    ICourseRepository Course { get; }
    IStudySessionRepository StudySession { get; }

    int Complete();
}
