                                                                                                                                                                                                            using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Models.Utility;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class StudySessionService : IStudySessionService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly IUserService _userService;

    public StudySessionService(IMapper mapper, IUnitOfWork unitOfWork, IUserService userService)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _userService = userService;
    }

    public async Task CreateAsync(StudySessionCreate studySessionCreate)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        StudySession studySession = studySessionCreate.ToEntity(user.Id);

        await _unitOfWork.StudySession.AddAsync(studySession);

        _unitOfWork.Complete();
    }

    public async Task UpdateAsync(StudySessionUpdate studySessionUpdate)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        StudySession? studySession = await _unitOfWork.StudySession.GetAsync(u => u.Id == studySessionUpdate.Id);

        if (studySession == null) throw new Exception("Study session not found");

        studySession = studySessionUpdate.ToEntity(studySession);

        _unitOfWork.StudySession.Update(studySession);
        _unitOfWork.Complete();
    }

    public async Task<ICollection<StudySession>> GetAllAsync(int userId)
    {
        IEnumerable<StudySession> studyTasks = await _unitOfWork.StudySession.GetAllAsync(
            u => u.User.Id == userId
        );
        return studyTasks.ToList();
    }

    public async Task<StudySession?> GetAsync(string UUID)
    {
        StudySession? studySession = await _unitOfWork.StudySession.GetAsync(u => u.SessionUUID == UUID);

        return studySession;
    }
}
