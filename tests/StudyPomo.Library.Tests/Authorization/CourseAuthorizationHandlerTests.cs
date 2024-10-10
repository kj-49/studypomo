using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization;
using Moq;
using StudyPomo.Library.Authorization.CourseAuthorization;
using StudyPomo.Library.Models.Identity;
using StudyPomo.Library.Models.Tables.CourseEntities;
using StudyPomo.Library.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Claims;

namespace StudyPomo.Library.Tests.Authorization;

public class CourseAuthorizationHandlerTests
{
    private readonly Mock<IUserService> _mockUserService;
    private readonly CourseAuthorizationHandler _handler;
    private readonly OperationAuthorizationRequirement _requirement;

    public CourseAuthorizationHandlerTests()
    {
        _mockUserService = new Mock<IUserService>();
        _handler = new CourseAuthorizationHandler(_mockUserService.Object);
        _requirement = new OperationAuthorizationRequirement { Name = "TestOperation" };
    }

    [Fact]
    public async Task HandleRequirementAsync_UserIsOwner_ContextSucceeds()
    {
        // Arrange
        var userId = 6;
        var user = new ApplicationUser { Id = userId };
        var course = new Course { UserId = userId };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), course);

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
        var userId = 4;
        var otherUserId = 2;
        var user = new ApplicationUser { Id = userId };
        var course = new Course { UserId = otherUserId };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), course);

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
        var course = new Course { UserId = 5 };
        var context = new AuthorizationHandlerContext(new[] { _requirement }, new ClaimsPrincipal(), course);

        _mockUserService.Setup(s => s.GetCurrentUserAsync()).ReturnsAsync((ApplicationUser)null);

        // Act
        await _handler.HandleAsync(context);

        // Assert
        Assert.False(context.HasSucceeded);
    }
}