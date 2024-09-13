using AutoMapper;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskLabelEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class TaskLabelService : ITaskLabelService
{
    public readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;
    private readonly IMapper _mapper;

    public TaskLabelService(IUnitOfWork unitOfWork, IUserService userService, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
        _mapper = mapper;
    }

    public async Task CreateAsync(TaskLabelCreate taskLabelCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        TaskLabel taskLabel = taskLabelCreate.ToTaskLabel(user.Id);

        await _unitOfWork.TaskLabel.AddAsync(taskLabel);
        _unitOfWork.Complete();
    }

    public async Task<ICollection<TaskLabel>> GetAllAsync(int userId)
    {
        IEnumerable<TaskLabel> taskLabels = await _unitOfWork.TaskLabel.GetAllAsync(
            u => u.UserId == userId,
            t => t.User,
            t => t.StudyTasks
        );
        return taskLabels.ToList();
    }

    public async Task UpdateAsync(TaskLabelUpdate taskLabelUpdate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        TaskLabel? taskLabel = await _unitOfWork.TaskLabel.GetAsync(u => u.Id == taskLabelUpdate.Id);
        if (taskLabel == null) throw new Exception("Task label not found");

        taskLabel = taskLabelUpdate.ToTaskLabel(user.Id, taskLabel);

        _unitOfWork.TaskLabel.Update(taskLabel);
        _unitOfWork.Complete();
    }

    public async Task RemoveAsync(int id)
    {
        TaskLabel? taskLabel = await _unitOfWork.TaskLabel.GetAsync(u => u.Id == id);
        if (taskLabel == null) throw new Exception("Task label not found");

        _unitOfWork.TaskLabel.Remove(taskLabel);
        _unitOfWork.Complete();
    }

    public async Task<TaskLabel?> GetAsync(int id)
    {
        TaskLabel? taskLabel = await _unitOfWork.TaskLabel.GetAsync(u => u.Id == id);
        if (taskLabel == null) throw new Exception("Task label not found");

        return taskLabel;
    }
}
