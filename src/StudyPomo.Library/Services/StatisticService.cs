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
        return ComputeStreak(studySessions);
    }

    public async Task<int> GetCurrentStreakAsync(int userId)
    {
        IEnumerable<StudySession> studySessions = await _studySessionService.GetAllAsync(userId);

        return ComputeStreak(studySessions);
    }

    private int ComputeStreak(IEnumerable<StudySession> studySessions)
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

        if (daysWithPomodoros.Count == 0)
        {
            return streak;
        }

        DateTime previousDate = daysWithPomodoros.First();
        
        // If today or yesterday, we have a running streak.
        if (previousDate.Date == DateTime.Today || previousDate.Date == DateTime.Now.AddDays(-1).Date)
        {
            streak++;
        }

        IEnumerable<DateTime> remainingDates = daysWithPomodoros.Skip(1);

        foreach (DateTime date in remainingDates)
        {
            if (previousDate.AddDays(-1).Date == date.Date)
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
