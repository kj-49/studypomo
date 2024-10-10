using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Moq;
using StudyPomo.Library.Authorization.StudyTaskAuthorization;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Models.Tables.StudyTaskEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Tests.Authorization.StudyTaskAuthorization;

public class StudyTaskAuthorizationHandlerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly Mock<ICourseService> _mockCourseService;
    private readonly StudyTaskAuthorizationHandler _handler;

    public StudyTaskAuthorizationHandlerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _mockCourseService = new Mock<ICourseService>();
        _handler = new StudyTaskAuthorizationHandler(_mockUserService.Object, _mockCourseService.Object);
    }

    [Theory]
    [InlineData("Create")]
    [InlineData("Update")]
    public async Task HandleRequirementAsync_UserOwnsTaskAndCourse_ContextSucceedsForCreateOrUpdate(string operation)
    {
        // Arrange
        var userId = 1;
        var courseId = 2;
        var user = new ApplicationUser { Id = userId };
        var studyTask = new StudyTask { UserId = userId, CourseId = courseId };
        var course = new Course { UserId = userId };
        var requirement = new OperationAuthorizationRequirement { Name = operation };
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), studyTask);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);
        _mockCourseService.Setup(s => s.GetAsync(courseId)).ReturnsAsync(course);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnsTaskWithoutCourse_ContextSucceedsForCreateOrUpdate()
    {
        // Arrange
        var userId = 1;
        var user = new ApplicationUser { Id = userId };
        var studyTask = new StudyTask { UserId = userId, CourseId = null };
        var requirement = new OperationAuthorizationRequirement { Name = "Create" };
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), studyTask);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Theory]
    [InlineData("Create")]
    [InlineData("Update")]
    public async Task HandleRequirementAsync_UserOwnsTaskButNotCourse_ContextDoesNotSucceedForCreateOrUpdate(string operation)
    {
        // Arrange
        var userId = 1;
        var otherUserId = 3;
        var courseId = 2;
        var user = new ApplicationUser { Id = userId };
        var studyTask = new StudyTask { UserId = userId, CourseId = courseId };
        var course = new Course { UserId = otherUserId };
        var requirement = new OperationAuthorizationRequirement { Name = operation };
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), studyTask);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);
        _mockCourseService.Setup(s => s.GetAsync(courseId)).ReturnsAsync(course);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserOwnsTask_ContextSucceedsForReadOrDelete()
    {
        // Arrange
        var userId = 1;
        var user = new ApplicationUser { Id = userId };
        var studyTask = new StudyTask { UserId = userId };
        var requirement = new OperationAuthorizationRequirement { Name = "Read" };
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), studyTask);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsNull_ContextDoesNotSucceed()
    {
        // Arrange
        var studyTask = new StudyTask { UserId = 1 };
        var requirement = new OperationAuthorizationRequirement { Name = "Delete" };
        var context = new AuthorizationHandlerContext(new[] { requirement }, new ClaimsPrincipal(), studyTask);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync((ApplicationUser)null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}
