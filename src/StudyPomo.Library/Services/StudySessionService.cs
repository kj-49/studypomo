                                                                                                                                                                                                            using AutoMapper;
using Microsoft.AspNetCore.Razor.TagHelpers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using StudyPomo.Library.Data.Database;
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
    private readonly IMapper _mapper;
    private readonly IUserService _userService;
    private readonly ApplicationDbContext _context;

    public StudySessionService(IMapper mapper, IUserService userService, ApplicationDbContext context)
    {
        _mapper = mapper;
        _userService = userService;
        _context = context;
    }

    public async Task CreateAsync(StudySessionCreate studySessionCreate)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        StudySession studySession = studySessionCreate.ToEntity(user.Id);

        await _context.StudySessions.AddAsync(studySession);

        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(StudySessionUpdate studySessionUpdate)
    {
        ApplicationUser user = await _userService.GetCurrentUserAsync();

        StudySession studySession = await _context.StudySessions.SingleAsync(u => u.Id == studySessionUpdate.Id);

        studySession = studySessionUpdate.ToEntity(studySession);

        _context.StudySessions.Update(studySession);

        await _context.SaveChangesAsync();
    }

    public async Task<ICollection<StudySession>> GetAllAsync(int userId)
    {
        return await _context.StudySessions
            .Where(u => u.UserId == userId)
            .ToListAsync();
    }

    public async Task<StudySession?> GetAsync(string UUID)
    {
        return await _context.StudySessions.SingleOrDefaultAsync(u => u.SessionUUID == UUID);
    }
}
