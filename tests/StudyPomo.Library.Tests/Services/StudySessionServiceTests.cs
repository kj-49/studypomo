using AutoMapper;
using Moq;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.StudySessionEntities;
using StudyPomo.Library.Services.Interfaces;
using StudyPomo.Library.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StudyPomo.Library.Models.Tables.CourseEntities;
using System.Linq.Expressions;

namespace StudyPomo.Library.Tests.Services;

public class StudySessionServiceTests
{
    private readonly Mock<IUnitOfWork> _mockUnitOfWork;
    private readonly Mock<IMapper> _mockMapper;
    private readonly Mock<IUserService> _mockUserService;
    private readonly StudySessionService _service;

    public StudySessionServiceTests()
    {
        _mockUnitOfWork = new Mock<IUnitOfWork>();
        _mockMapper = new Mock<IMapper>();
        _mockUserService = new Mock<IUserService>();
        _service = new StudySessionService(_mockMapper.Object, _mockUnitOfWork.Object, _mockUserService.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateStudySession()
    {
        // Arrange
        var userId = 1;
        var studySessionCreate = new StudySessionCreate
        {
            SessionUUID = "test-uuid",
            TotalPomodoros = 5,
            TotalFocusTime = 1500,
            TotalBreakTime = 300
        };
        var user = new ApplicationUser { Id = userId };
        var studySession = new StudySession
        {
            UserId = userId,
            SessionUUID = studySessionCreate.SessionUUID,
            TotalPomodoros = studySessionCreate.TotalPomodoros,
            TotalFocusTime = studySessionCreate.TotalFocusTime,
            TotalBreakTime = studySessionCreate.TotalBreakTime,
            DateStarted = DateTime.UtcNow
        };

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);
        _mockUnitOfWork.Setup(u => u.StudySession.AddAsync(It.IsAny<StudySession>())).ReturnsAsync(studySession);

        _mockMapper.Setup(m => m.Map<StudySession>(It.IsAny<StudySessionCreate>())).Returns(studySession);

        // Act
        await _service.CreateAsync(studySessionCreate);

        // Assert
        _mockUnitOfWork.Verify(u => u.StudySession.AddAsync(It.IsAny<StudySession>()), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateStudySession()
    {
        // Arrange
        var userId = 1;
        var studySessionUpdate = new StudySessionUpdate
        {
            Id = 1,
            TotalPomodoros = 10,
            TotalFocusTime = 2000,
            TotalBreakTime = 400
        };
        var user = new ApplicationUser { Id = userId };
        var existingStudySession = new StudySession
        {
            Id = 1,
            UserId = userId,
            SessionUUID = "test-uuid",
            TotalPomodoros = 5,
            TotalFocusTime = 1500,
            TotalBreakTime = 300
        };

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);
        _mockUnitOfWork.Setup(u => u.StudySession.GetAsync(
                It.IsAny<Expression<Func<StudySession, bool>>>(),
                It.IsAny<Expression<Func<StudySession, object>>[]>()
            )).ReturnsAsync(existingStudySession);

        _mockMapper.Setup(m => m.Map(It.IsAny<StudySessionUpdate>(), existingStudySession)).Returns(existingStudySession);

        // Act
        await _service.UpdateAsync(studySessionUpdate);

        // Assert
        _mockUnitOfWork.Verify(u => u.StudySession.Update(existingStudySession), Times.Once);
        _mockUnitOfWork.Verify(u => u.Complete(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllStudySessionsForUser()
    {
        // Arrange
        var userId = 1;
        var user = new ApplicationUser { Id = userId };
        var studySessions = new List<StudySession>
            {
                new StudySession { UserId = userId, SessionUUID = "uuid1" },
                new StudySession { UserId = userId, SessionUUID = "uuid2" }
            };

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);
        _mockUnitOfWork.Setup(u => u.StudySession.GetAllAsync(
                It.IsAny<Expression<Func<StudySession, bool>>>(),
                It.IsAny<Expression<Func<StudySession, object>>[]>()
            )).ReturnsAsync(studySessions);

        // Act
        var result = await _service.GetAllAsync(userId);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnStudySessionByUUID()
    {
        // Arrange
        var uuid = "test-uuid";
        var existingStudySession = new StudySession
        {
            SessionUUID = uuid,
            UserId = 1,
            TotalPomodoros = 5,
            TotalFocusTime = 1500,
            TotalBreakTime = 300
        };

        _mockUnitOfWork.Setup(u => u.StudySession.GetAsync(
                It.IsAny<Expression<Func<StudySession, bool>>>(),
                It.IsAny<Expression<Func<StudySession, object>>[]>()
            )).ReturnsAsync(existingStudySession);

        // Act
        var result = await _service.GetAsync(uuid);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(uuid, result.SessionUUID);
    }
}