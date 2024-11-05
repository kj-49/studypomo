using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using StudyPomo.Library.Services.Interfaces;

namespace StudyPomo.Library.Tests.Services;

public class StatisticServiceTests
{
    private readonly StatisticService _statisticService;
    private readonly Mock<IStudySessionService> _studySessionServiceMock;

    public StatisticServiceTests()
    {
        _studySessionServiceMock = new Mock<IStudySessionService>();
        _statisticService = new StatisticService(_studySessionServiceMock.Object);
    }

    [Fact]
    public async Task GetCurrentStreakAsync_ShouldReturnZero_WhenNoStudySessions()
    {
        // Arrange
        var studySessions = new List<StudySession>();
        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act
        var result = await _statisticService.GetCurrentStreakAsync(1, userTimeZone);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public async Task GetCurrentStreakAsync_ShouldReturnCorrectStreak_WhenSessionsAreOnConsecutiveCalendarDays()
    {
        // Arrange
        var today = DateTime.UtcNow.Date; // Get today's date at 00:00 AM
        var yesterday = today.AddDays(-1); // Get yesterday's date
        var dayBeforeYesterday = today.AddDays(-2); // Get the day before yesterday

        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(today.Year, today.Month, today.Day, 1, 0, 0) }, // Today at 1:00 AM
            new StudySession { UserId = 1, TotalPomodoros = 2, DateStarted = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 0, 0) }, // Yesterday at 11:00 PM
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(dayBeforeYesterday.Year, dayBeforeYesterday.Month, dayBeforeYesterday.Day, 9, 0, 0) }   // Day before yesterday at 9:00 AM
        };

        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act
        var result = await _statisticService.GetCurrentStreakAsync(1, userTimeZone);

        // Assert
        Assert.Equal(3, result);
    }


    [Fact]
    public async Task GetCurrentStreakAsync_ShouldReturnStreak_WhenSessionsSpanMidnight()
    {
        // Arrange
        var today = DateTime.UtcNow.Date; // Get today's date at 00:00 AM
        var yesterday = today.AddDays(-1); // Get yesterday's date

        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(today.Year, today.Month, today.Day, 2, 0, 0) }, // Today at 2:00 AM
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 59, 0) }  // Yesterday at 11:59 PM
        };

        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act
        var result = await _statisticService.GetCurrentStreakAsync(1, userTimeZone);

        // Assert
        Assert.Equal(2, result);
    }


    [Fact]
    public async Task GetCurrentStreakAsync_ShouldReturnOneIfOnlyTodayInUserTimeZone()
    {
        // Arrange
        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = DateTime.UtcNow }
        };
        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act
        var result = await _statisticService.GetCurrentStreakAsync(1, userTimeZone);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetCurrentStreakAsync_ShouldStopStreak_WhenNonConsecutiveDaysButRecent()
    {
        // Arrange
        var today = DateTime.UtcNow.Date; // Get today's date at 00:00 AM

        var threeDaysBack = today.AddDays(-3); // Three days back
        var fourDaysBack = today.AddDays(-4); // Two days back

        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(today.Year, today.Month, today.Day, 10, 0, 0) }, // Today at 10:00 AM
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(threeDaysBack.Year, threeDaysBack.Month, threeDaysBack.Day, 9, 0, 0) },
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(fourDaysBack.Year, fourDaysBack.Month, fourDaysBack.Day, 9, 0, 0) }
        };
        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act
        var result = await _statisticService.GetCurrentStreakAsync(1, userTimeZone);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public async Task GetCurrentStreakAsync_ShouldThrowException_WhenSessionsForDifferentUsers()
    {
        // Arrange
        var today = DateTime.UtcNow.Date; // Get today's date at 00:00 AM
        var yesterday = today.AddDays(-1); // Get yesterday's date
        var dayBeforeYesterday = today.AddDays(-2); // Get the day before yesterday

        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(today.Year, today.Month, today.Day, 1, 0, 0) }, // Today at 1:00 AM
            new StudySession { UserId = 2, TotalPomodoros = 2, DateStarted = new DateTime(yesterday.Year, yesterday.Month, yesterday.Day, 23, 0, 0) }, // Yesterday at 11:00 PM
            new StudySession { UserId = 1, TotalPomodoros = 1, DateStarted = new DateTime(dayBeforeYesterday.Year, dayBeforeYesterday.Month, dayBeforeYesterday.Day, 9, 0, 0) }   // Day before yesterday at 9:00 AM
        };

        var userTimeZone = TimeZoneInfo.Utc;
        _studySessionServiceMock.Setup(s => s.GetAllAsync(It.IsAny<int>())).ReturnsAsync(studySessions);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => _statisticService.GetCurrentStreakAsync(1, userTimeZone));
    }
}
