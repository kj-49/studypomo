using PomodoroLibrary.Models.Tables;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Data.Interfaces;

public interface ITaskRepository : IGeneralRepository<TaskModel>
{
}
