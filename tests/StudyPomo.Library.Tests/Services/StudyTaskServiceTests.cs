using Moq;
using StudyPomo.Library.Data.Interfaces;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Models.Tables.TaskPriorityEntities;
using StudyPomo.Library.Models.Tables.StudyTaskLabelEntities;
using StudyPomo.Library.Services.Interfaces;
using AutoMapper;
using StudyPomo.Library.Services;
using StudyPomo.Library.Models.Tables.LabelEntities;
using System.Linq.Expressions;
using StudyPomo.Library.Data;

namespace StudyPomo.Library.Tests.Services;

public class StudyTaskServiceTests
{
    private readonly StudyTaskService _studyTaskService;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IUserService> _userServiceMock;

    public StudyTaskServiceTests()
    {
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();
        _userServiceMock = new Mock<IUserService>();
        _studyTaskService = new StudyTaskService(_mapperMock.Object, _unitOfWorkMock.Object, _userServiceMock.Object);
    }

    [Fact]
    public async Task CreateAsync_ShouldCreateStudyTask_WhenValidDataProvided()
    {
        // Arrange
        var user = new ApplicationUser { Id = 1, TimeZoneId = "UTC" };
        var studyTaskCreate = new StudyTaskCreate
        {
            Name = "Test Task",
            TaskPriorityId = 1,
            TaskLabelIds = new List<int> { 1, 2 }
        };
        var taskPriority = new TaskPriority { Id = 1 };
        var label1 = new TaskLabel { Id = 1 };
        var label2 = new TaskLabel { Id = 2 };

        _userServiceMock.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(user);
        _unitOfWorkMock.Setup(x => x.TaskPriority.GetAsync(It.IsAny<Expression<Func<TaskPriority, bool>>>()))
            .ReturnsAsync(taskPriority);
        _unitOfWorkMock.Setup(x => x.TaskLabel.GetAllAsync(It.IsAny<Expression<Func<TaskLabel, bool>>>()))
            .ReturnsAsync(new List<TaskLabel> { label1, label2 });

        _unitOfWorkMock.Setup(x => x.StudyTaskLabel.AddRangeAsync(It.IsAny<IEnumerable<StudyTaskLabel>>()))
             .Returns(Task.CompletedTask)
             .Verifiable();

        _unitOfWorkMock.Setup(x => x.StudyTask.AddAsync(It.IsAny<StudyTask>()))
             .ReturnsAsync((StudyTask task) => task)
             .Verifiable();

        // Act
        await _studyTaskService.CreateAsync(studyTaskCreate);

        // Assert
        _unitOfWorkMock.Verify(x => x.StudyTask.AddAsync(It.IsAny<StudyTask>()), Times.Exactly(1));
        _unitOfWorkMock.Verify(x => x.StudyTaskLabel.AddRangeAsync(It.IsAny<IEnumerable<StudyTaskLabel>>()), Times.Exactly(1));
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }

    [Fact]
    public async Task CreateAsync_ShouldThrowException_WhenUserNotFound()
    {
        // Arrange
        _userServiceMock.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync((ApplicationUser?)null);

        var studyTaskCreate = new StudyTaskCreate { Name = "Test Task" };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _studyTaskService.CreateAsync(studyTaskCreate));
    }

    [Fact]
    public async Task RemoveAsync_ShouldRemoveStudyTask_WhenTaskExists()
    {
        // Arrange
        var studyTask = new StudyTask { Id = 1 };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync(studyTask);

        // Act
        await _studyTaskService.RemoveAsync(1);

        // Assert
        _unitOfWorkMock.Verify(x => x.StudyTask.Remove(studyTask), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }

    [Fact]
    public async Task RemoveAsync_ShouldThrowException_WhenTaskNotFound()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync((StudyTask?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _studyTaskService.RemoveAsync(1));
    }

    [Fact]
    public async Task ArchiveAsync_ShouldArchiveStudyTask_WhenTaskExists()
    {
        // Arrange
        var studyTask = new StudyTask { Id = 1, Archived = false };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync(studyTask);

        // Act
        await _studyTaskService.ArchiveAsync(1);

        // Assert
        Assert.True(studyTask.Archived);
        _unitOfWorkMock.Verify(x => x.StudyTask.Update(studyTask), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }

    [Fact]
    public async Task CompleteAsync_ShouldCompleteStudyTask_WhenTaskExists()
    {
        // Arrange
        var studyTask = new StudyTask { Id = 1, Completed = false };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync(studyTask);

        // Act
        await _studyTaskService.CompleteAsync(1);

        // Assert
        Assert.True(studyTask.Completed);
        Assert.NotNull(studyTask.DateCompleted);
        _unitOfWorkMock.Verify(x => x.StudyTask.Update(studyTask), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }

    [Fact]
    public async Task UncompleteAsync_ShouldUncompleteStudyTask_WhenTaskExists()
    {
        // Arrange
        var studyTask = new StudyTask { Id = 1, Completed = true, DateCompleted = DateTime.UtcNow };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync(studyTask);

        // Act
        await _studyTaskService.UncompleteAsync(1);

        // Assert
        Assert.False(studyTask.Completed);
        Assert.Null(studyTask.DateCompleted);
        _unitOfWorkMock.Verify(x => x.StudyTask.Update(studyTask), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnAllTasks_WhenIncludeArchivedIsTrue()
    {
        // Arrange
        var tasks = new List<StudyTask>
        {
            new StudyTask { Id = 1, Archived = true },
            new StudyTask { Id = 2, Archived = false }
        };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAllAsync(
            It.IsAny<Expression<Func<StudyTask, bool>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _studyTaskService.GetAllAsync(1, includeArchived: true);

        // Assert
        Assert.Equal(2, result.Count);
    }

    [Fact]
    public async Task GetAllAsync_ShouldReturnOnlyUnarchived_WhenIncludeArchivedIsFalse()
    {
        // Arrange
        var tasks = new List<StudyTask>
        {
            new StudyTask { Id = 2, Archived = false }
        };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAllAsync(
            It.IsAny<Expression<Func<StudyTask, bool>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>()))
            .ReturnsAsync(tasks);

        // Act
        var result = await _studyTaskService.GetAllAsync(1, includeArchived: false);

        // Assert
        Assert.Single(result);
        Assert.False(result.First().Archived);
    }

    [Fact]
    public async Task GetAsync_ShouldReturnTask_WhenTaskExists()
    {
        // Arrange
        var studyTask = new StudyTask { Id = 1 };
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(
            It.IsAny<Expression<Func<StudyTask, bool>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>()))
            .ReturnsAsync(studyTask);

        // Act
        var result = await _studyTaskService.GetAsync(1);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetAsync_ShouldThrowException_WhenTaskNotFound()
    {
        // Arrange
        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(
            It.IsAny<Expression<Func<StudyTask, bool>>>(),
            It.IsAny<Expression<Func<StudyTask, object>>>()))
            .ReturnsAsync((StudyTask?)null);

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => _studyTaskService.GetAsync(1));
    }

    [Fact]
    public async Task UpdateAsync_ShouldUpdateTask_WhenTaskExists()
    {
        // Arrange
        var user = new ApplicationUser { Id = 1, TimeZoneId = "UTC" };
        var existingTask = new StudyTask
            {
                Id = 1,
                Name = "Existing Task",
                StudyTaskLabels = new List<StudyTaskLabel>
            {
                new StudyTaskLabel { StudyTaskId = 1, TaskLabelId = 1 },
                new StudyTaskLabel { StudyTaskId = 1, TaskLabelId = 2 }
            }
        };

        var studyTaskUpdate = new StudyTaskUpdate
        {
            Id = 1,
            Name = "Updated Task",
            TaskLabelIds = new List<int> { 3 }
        };

        var newLabel = new TaskLabel { Id = 3, Name = "New Label" };

        _userServiceMock.Setup(x => x.GetCurrentUserAsync())
            .ReturnsAsync(user);

        _unitOfWorkMock.Setup(x => x.StudyTaskLabel.GetAllAsync(It.IsAny<Expression<Func<StudyTaskLabel, bool>>>()))
            .ReturnsAsync(existingTask.StudyTaskLabels);

        _unitOfWorkMock.Setup(x => x.StudyTask.GetAsync(It.IsAny<Expression<Func<StudyTask, bool>>>()))
            .ReturnsAsync(existingTask);

        _unitOfWorkMock.Setup(x => x.TaskLabel.GetAllAsync(It.IsAny<Expression<Func<TaskLabel, bool>>>()))
            .ReturnsAsync(new List<TaskLabel> { newLabel });

        // Mock the AddAsync for StudyTaskLabel
        _unitOfWorkMock.Setup(x => x.StudyTaskLabel.AddRangeAsync(It.IsAny<IEnumerable<StudyTaskLabel>>()))
            .Returns(Task.CompletedTask);

        // Act
        await _studyTaskService.UpdateAsync(studyTaskUpdate);

        // Assert
        _unitOfWorkMock.Verify(x => x.StudyTaskLabel.RemoveRange(It.Is<IEnumerable<StudyTaskLabel>>(l => l.Count() == 2)), Times.Once);
        _unitOfWorkMock.Verify(x => x.StudyTaskLabel.AddRangeAsync(It.IsAny<IEnumerable<StudyTaskLabel>>()), Times.Once);
        _unitOfWorkMock.Verify(x => x.StudyTask.Update(It.Is<StudyTask>(t => t.Name == "Updated Task")), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Exactly(2));
    }




    [Fact]
    public async Task RemoveAllLabels_ShouldRemoveAllLabelsFromTask()
    {
        // Arrange
        var labels = new List<StudyTaskLabel>
        {
            new StudyTaskLabel { StudyTaskId = 1, TaskLabelId = 1 },
            new StudyTaskLabel { StudyTaskId = 1, TaskLabelId = 2 }
        };
        _unitOfWorkMock.Setup(x => x.StudyTaskLabel.GetAllAsync(It.IsAny<Expression<Func<StudyTaskLabel, bool>>>()))
            .ReturnsAsync(labels);

        // Act
        await _studyTaskService.RemoveAllLabels(1);

        // Assert
        _unitOfWorkMock.Verify(x => x.StudyTaskLabel.RemoveRange(labels), Times.Once);
        _unitOfWorkMock.Verify(x => x.Complete(), Times.Once);
    }
}