using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Moq;
using StudyPomo.Library.Authorization.LabelRequirements;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.LabelEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace StudyPomo.Library.Tests.Authorization.TaskLabelAuthorization;

public class TaskLabelAuthorizationHandlerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly TaskLabelAuthorizationHandler _handler;
    private readonly OperationAuthorizationRequirement _requirement;

    public TaskLabelAuthorizationHandlerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _handler = new TaskLabelAuthorizationHandler(_mockUserService.Object);
        _requirement = new OperationAuthorizationRequirement { Name = "TestOperation" };
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsOwner_ContextSucceeds()
    {
        // Arrange
        var userId = 1;
        var user = new ApplicationUser { Id = userId };
        var taskLabel = new TaskLabel { UserId = userId };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), taskLabel);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.True(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsNotOwner_ContextDoesNotSucceed()
    {
        // Arrange
        var userId = 1;
        var otherUserId = 2;
        var user = new ApplicationUser { Id = userId };
        var taskLabel = new TaskLabel { UserId = otherUserId };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), taskLabel);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync(user);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsNull_ContextDoesNotSucceed()
    {
        // Arrange
        var taskLabel = new TaskLabel { UserId = 1 };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), taskLabel);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync((ApplicationUser)null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}
