using Microsoft.EntityFrameworkCore.Metadata.Conventions;
using Microsoft.Extensions.Configuration;
using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Data.Interfaces;

namespace PomodoroLibrary.Data;

public class TaskRepository : GeneralRepository<TaskModel>, ITaskRepository
{
    public const string TableName = "Task";

    public TaskRepository(IConfiguration config) : base(config)
    {

    }

}
