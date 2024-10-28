using Moq;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    public void GetCurrentStreak_ShouldReturnZero_WhenNoStudySessions()
    {
        // Arrange
        var studySessions = new List<StudySession>();

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(0, result);
    }

    [Fact]
    public void GetCurrentStreak_ShouldReturnCorrectStreak_WhenSessionsAreConsecutive()
    {
        // Arrange
        var studySessions = new List<StudySession>
            {
                new StudySession { UserId = 1, DateStarted = DateTime.Now, TotalPomodoros = 3 },
                new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-1), TotalPomodoros = 2 },
                new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-2), TotalPomodoros = 1 }
            };

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(3, result);
    }

    [Fact]
    public void GetCurrentStreak_ShouldReturnStreakCountUntilNonConsecutiveDate()
    {
        // Arrange
        var studySessions = new List<StudySession>
            {
                new StudySession { UserId = 1, DateStarted = DateTime.Now, TotalPomodoros = 1 },
                new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-1), TotalPomodoros = 1 },
                new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-4), TotalPomodoros = 1 },
                new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-5), TotalPomodoros = 1 }
            };

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(2, result);
    }

    [Fact]
    public void GetCurrentStreak_ShouldReturnOneIfOnlyToday()
    {
        // Arrange
        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, DateStarted = DateTime.Now, TotalPomodoros = 1 },
            new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-1), TotalPomodoros = 0 },
            new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-4), TotalPomodoros = 0 },
            new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-5), TotalPomodoros = 0 }
        };

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetCurrentStreak_ShouldReturnOneIfYesterdayButNotToday()
    {
        // Arrange
        var studySessions = new List<StudySession>
        {
            new StudySession { UserId = 1, DateStarted = DateTime.Now.AddDays(-1), TotalPomodoros = 1 }
        };

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(1, result);
    }

    [Fact]
    public void GetCurrentStreak_ShouldThrowException_WhenSessionsForDifferentUsers()
    {
        // Arrange
        var studySessions = new List<StudySession>
            {
                new StudySession { UserId = 1, DateStarted = DateTime.Now, TotalPomodoros = 1 },
                new StudySession { UserId = 2, DateStarted = DateTime.Now.AddDays(-1), TotalPomodoros = 1 }
            };

        // Act & Assert
        Assert.Throws<ArgumentException>(() => _statisticService.GetCurrentStreak(studySessions));
    }

    [Fact]
    public void GetCurrentStreak_ShouldReturnZero_WhenConsecutiveButNotRecent()
    {
        // Arrange
        var studySessions = new List<StudySession>
            {
                new StudySession { UserId = 2, DateStarted = DateTime.Now.AddDays(-5), TotalPomodoros = 1 },
                new StudySession { UserId = 2, DateStarted = DateTime.Now.AddDays(-6), TotalPomodoros = 1 },
                new StudySession { UserId = 2, DateStarted = DateTime.Now.AddDays(-7), TotalPomodoros = 1 },
            };

        // Act
        var result = _statisticService.GetCurrentStreak(studySessions);

        // Assert
        Assert.Equal(0, result);
    }
}