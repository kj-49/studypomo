using Microsoft.AspNetCore.Http;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class StatisticService : IStatisticService
{
    private readonly IStudySessionService _studySessionService;

    public StatisticService(IStudySessionService studySessionService)
    {
        _studySessionService = studySessionService;
    }

    public int GetCurrentStreak(IEnumerable<StudySession> studySessions)
    {
        if (studySessions.Count() == 0)
        {
            return 0;
        }

        if (!studySessions.All(u => u.UserId == studySessions.First().UserId))
        {
            throw new ArgumentException("Study sessions must be for the same user");
        }


        var daysWithPomodoros = studySessions
            .Where(s => s.TotalPomodoros > 0)
            .GroupBy(s => s.DateStarted.Date)
            .Select(g => g.Key)
            .OrderByDescending(date => date)
            .ToList();

        int streak = 0;
        DateTime? previousDate = null;

        foreach (DateTime date in daysWithPomodoros)
        {
            if (previousDate == null || date == previousDate.Value.AddDays(-1))
            {
                streak++;
                previousDate = date;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

    public async Task<int> GetCurrentStreakAsync(int userId)
    {
        IEnumerable<StudySession> studySessions = await _studySessionService.GetAllAsync(userId);

        if (studySessions.Count() == 0)
        {
            return 0;
        }

        if (!studySessions.All(u => u.UserId == studySessions.First().UserId))
        {
            throw new ArgumentException("Study sessions must be for the same user");
        }


        var daysWithPomodoros = studySessions
            .Where(s => s.TotalPomodoros > 0)
            .GroupBy(s => s.DateStarted.Date)
            .Select(g => g.Key)
            .OrderByDescending(date => date)
            .ToList();

        int streak = 0;
        DateTime? previousDate = null;

        foreach (DateTime date in daysWithPomodoros)
        {
            if (previousDate == null || date == previousDate.Value.AddDays(-1))
            {
                streak++;
                previousDate = date;
            }
            else
            {
                break;
            }
        }

        return streak;
    }

}
