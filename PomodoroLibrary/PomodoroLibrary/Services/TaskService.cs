using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Tables;
using PomodoroLibrary.Models.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class TaskService
{
    private readonly ITaskRepository _taskRepository;

    public TaskService(ITaskRepository taskRepository)
    {
        _taskRepository = taskRepository;
    }

    public Task<OperationResult<TaskModel>> CreateModelAsync(TaskModel model)
    {
        //return _taskRepository.InsertModelAndReturnAsync("uspTask_Insert", model);
     throw new NotImplementedException();
    }

}
