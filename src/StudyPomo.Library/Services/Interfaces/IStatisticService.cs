using StudyPomo.Library.Models.Tables.StudySessionEntities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Services.Interfaces;

public interface IStatisticService
{
    int GetCurrentStreak(IEnumerable<StudySession> studySessions, TimeZoneInfo userTimeZone);
    Task<int> GetCurrentStreakAsync(int userId, TimeZoneInfo userTimeZone);
}
