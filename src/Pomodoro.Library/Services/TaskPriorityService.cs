using Pomodoro.Library.Data.Interfaces;
using Pomodoro.Library.Models.Tables.TaskPriorityEntities;
using Pomodoro.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomodoro.Library.Services;

public class TaskPriorityService : ITaskPriorityService
{
    private readonly IUnitOfWork _unitOfWork;

    public TaskPriorityService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public async Task<ICollection<TaskPriority>> GetAllAsync()
    {
        IEnumerable<TaskPriority> taskPriorities = await _unitOfWork.TaskPriority.GetAllAsync();
        return taskPriorities.ToList();
    }
}
