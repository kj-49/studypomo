using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.LabelEntities;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class TaskLabelService : ITaskLabelService
{
    public readonly IUnitOfWork _unitOfWork;
    private readonly IUserService _userService;

    public TaskLabelService(IUnitOfWork unitOfWork, IUserService userService)
    {
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task CreateAsync(TaskLabelCreate taskLabelCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        TaskLabel taskLabel = new TaskLabel {
            Name = taskLabelCreate.Name,
            HexColor = taskLabelCreate.HexColor,
            UserId = user.Id,
            User = user
        };

        await _unitOfWork.TaskLabel.AddAsync(taskLabel);
        _unitOfWork.Complete();
    }
}
