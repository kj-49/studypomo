using Microsoft.AspNetCore.Http;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services;

public class StatisticService : IStatisticService
{
    private readonly IStudySessionService _studySessionService;

    public StatisticService(IStudySessionService studySessionService)
    {
        _studySessionService = studySessionService;
    }

    public int GetCurrentStreak(IEnumerable<StudySession> studySessions, TimeZoneInfo userTimeZone)
    {
        return ComputeStreak(studySessions, userTimeZone);
    }

    public async Task<int> GetCurrentStreakAsync(int userId, TimeZoneInfo userTimeZone)
    {
        IEnumerable<StudySession> studySessions = await _studySessionService.GetAllAsync(userId);
        return ComputeStreak(studySessions, userTimeZone);
    }

    private int ComputeStreak(IEnumerable<StudySession> studySessions, TimeZoneInfo userTimeZone)
    {
        if (!studySessions.Any())
        {
            return 0;
        }

        if (!studySessions.All(u => u.UserId == studySessions.First().UserId))
        {
            throw new ArgumentException("Study sessions must be for the same user");
        }

        // Convert session dates to the user's time zone and group by calendar day
        var daysWithPomodoros = studySessions
            .Where(s => s.TotalPomodoros > 0)
            .GroupBy(s => TimeZoneInfo.ConvertTimeFromUtc(s.DateStarted, userTimeZone).Date)
            .Select(g => g.Key)
            .OrderByDescending(date => date)
            .ToList();

        int streak = 0;

        if (!daysWithPomodoros.Any())
        {
            return streak;
        }

        DateTime previousDate = daysWithPomodoros.First();

        // Check if the streak starts today or yesterday in the user's time zone
        if (previousDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, userTimeZone).Date ||
            previousDate == TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow.AddDays(-1), userTimeZone).Date)
        {
            streak++;
        }
        else
        {
            return 0;
        }

        // Count consecutive days
        foreach (DateTime date in daysWithPomodoros.Skip(1))
        {
            if (previousDate.AddDays(-1) == date)
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
