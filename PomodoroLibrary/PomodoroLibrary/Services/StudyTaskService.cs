using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using PomodoroLibrary.Data.Interfaces;
using PomodoroLibrary.Models.Identity;
using PomodoroLibrary.Models.Tables.StudyTaskEntities;
using PomodoroLibrary.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PomodoroLibrary.Services;

public class StudyTaskService : IStudyTaskService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public StudyTaskService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task CreateAsync(StudyTaskVM studyTaskCreate)
    {
        ApplicationUser? user = await _userService.GetCurrentUserAsync();
        if (user == null) throw new Exception("User not found");

        var taskPriority = await _unitOfWork.TaskPriority.GetAsync(u => u.Level == studyTaskCreate.TaskPriority.ToString());

        StudyTask studyTask = new StudyTask
        {
            User = user,
            Name = studyTaskCreate.Name,
            Completed = false,
            DateCreated = DateTime.UtcNow,
            DateCompleted = null,
            TaskPriority = taskPriority
        };

        await _unitOfWork.StudyTask.AddAsync(studyTask);
        _unitOfWork.Complete();
    }

    public async Task RemoveAsync(int id)
    {
        StudyTask? studyTask = await _unitOfWork.StudyTask.GetAsync(u => u.Id == id);
        if (studyTask == null) throw new Exception("Study Task not found");

        _unitOfWork.StudyTask.Remove(studyTask);
        _unitOfWork.Complete();
    }

}
