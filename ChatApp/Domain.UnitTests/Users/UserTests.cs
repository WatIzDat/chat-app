using Domain.Discussions;
using Domain.Users;
using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class UserTests
{
    private static User CreateDefaultUser()
    {
        return User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            CreateDefaultDiscussionsList(),
            CreateDefaultRolesList(),
            "test").Value;
    }

    private static DiscussionsList CreateDefaultDiscussionsList()
    {
        return DiscussionsList.Create([]).Value;
    }

    private static RolesList CreateDefaultRolesList()
    {
        return RolesList.Create([]).Value;
    }

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Arrange
        User referenceUser = CreateDefaultUser();

        // Act
        Result<User> result = User.Create(
            referenceUser.Username,
            referenceUser.Email,
            referenceUser.DateCreatedUtc,
            referenceUser.AboutSection,
            referenceUser.Discussions,
            referenceUser.Roles,
            referenceUser.ClerkId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Arrange
        User referenceUser = CreateDefaultUser();

        string longerThanMaxLength = string.Empty.PadLeft(User.UsernameMaxLength + 1);

        // Act
        Result<User> result = User.Create(
            longerThanMaxLength,
            referenceUser.Email,
            referenceUser.DateCreatedUtc,
            referenceUser.AboutSection,
            referenceUser.Discussions,
            referenceUser.Roles,
            referenceUser.ClerkId);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public void ChangeUsername_Should_ReturnSuccess()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        string validUsername = "a";

        // Act
        Result result = defaultUser.ChangeUsername(validUsername);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void ChangeUsername_Should_ChangeUsername()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        string validUsername = "a";

        // Act
        defaultUser.ChangeUsername(validUsername);

        // Assert
        defaultUser.Username.Should().Be(validUsername);
    }

    [Fact]
    public void ChangeUsername_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        string longerThanMaxLength = string.Empty.PadLeft(User.UsernameMaxLength + 1);

        // Act
        Result result = defaultUser.ChangeUsername(longerThanMaxLength);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public void ChangeEmail_Should_ChangeEmail()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        string validEmail = "test@test.com";

        // Act
        defaultUser.ChangeEmail(Email.Create(validEmail).Value);

        // Assert
        defaultUser.Email.Should().Be(Email.Create(validEmail).Value);
    }

    [Fact]
    public void UpdateAboutSection_Should_ChangeAboutSection()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        string validAboutSection = "This is another test about section.";

        // Act
        defaultUser.UpdateAboutSection(AboutSection.Create(validAboutSection).Value);

        // Assert
        defaultUser.AboutSection.Should().Be(AboutSection.Create(validAboutSection).Value);
    }

    [Fact]
    public void JoinDiscussion_Should_ReturnSuccess()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid discussionId = Guid.Empty;

        // Act
        Result result = defaultUser.JoinDiscussion(discussionId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void JoinDiscussion_Should_AddDiscussionToDiscussionsList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid discussionId = Guid.Empty;

        // Act
        defaultUser.JoinDiscussion(discussionId);

        // Assert
        defaultUser.Discussions.Value.Should().Contain(discussionId);
    }

    [Fact]
    public void JoinDiscussion_Should_ReturnDuplicateDiscussions_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid discussionId = Guid.Empty;

        defaultUser.JoinDiscussion(discussionId);
        
        // Act
        Result result = defaultUser.JoinDiscussion(discussionId);

        // Assert
        result.Error.Should().Be(DiscussionsListErrors.DuplicateDiscussions);
    }

    [Fact]
    public void LeaveDiscussion_Should_ReturnSuccess()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid idInDiscussionsList = Guid.Empty;

        defaultUser.JoinDiscussion(idInDiscussionsList);

        // Act
        Result result = defaultUser.LeaveDiscussion(idInDiscussionsList);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void LeaveDiscussion_Should_RemoveDiscussionFromDiscussionsList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid idInDiscussionsList = Guid.Empty;

        defaultUser.JoinDiscussion(idInDiscussionsList);

        // Act
        defaultUser.LeaveDiscussion(idInDiscussionsList);

        // Assert
        defaultUser.Discussions.Value.Should().NotContain(idInDiscussionsList);
    }

    [Fact]
    public void LeaveDiscussion_Should_ReturnDiscussionNotFound_WhenGuidIsNotInDiscussionsList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid idNotInDiscussionsList = Guid.Empty;

        // Act
        Result result = defaultUser.LeaveDiscussion(idNotInDiscussionsList);

        // Assert
        result.Error.Should().Be(UserErrors.DiscussionNotFound);
    }

    [Fact]
    public void AddRole_Should_ReturnSuccess()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        defaultUser.JoinDiscussion(Guid.NewGuid());

        Guid roleId = Guid.Empty;

        // Act
        Result result = defaultUser.AddRole(roleId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void AddRole_Should_AddRoleToRolesList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        defaultUser.JoinDiscussion(Guid.NewGuid());

        Guid roleId = Guid.Empty;

        // Act
        defaultUser.AddRole(roleId);

        // Assert
        defaultUser.Roles.Value.Should().Contain(roleId);
    }

    [Fact]
    public void AddRole_Should_ReturnDuplicateRoles_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        defaultUser.JoinDiscussion(Guid.NewGuid());
        defaultUser.JoinDiscussion(Guid.NewGuid());
        
        Guid roleId = Guid.Empty;

        defaultUser.AddRole(roleId);

        // Act
        Result result = defaultUser.AddRole(roleId);

        // Assert
        result.Error.Should().Be(RolesListErrors.DuplicateRoles);
    }

    [Fact]
    public void AddRole_Should_ReturnTooManyRoles_WhenRolesCountIsGreaterThanDiscussionsCount()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid roleId = Guid.Empty;

        // Act
        Result result = defaultUser.AddRole(roleId);

        // Assert
        result.Error.Should().Be(RolesListErrors.TooManyRoles);
    }

    [Fact]
    public void RemoveRole_Should_ReturnSuccess()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        defaultUser.JoinDiscussion(Guid.NewGuid());

        Guid roleId = Guid.Empty;

        defaultUser.AddRole(roleId);

        // Act
        Result result = defaultUser.RemoveRole(roleId);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void RemoveRole_Should_RemoveRoleFromRolesList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid roleId = Guid.Empty;

        defaultUser.AddRole(roleId);

        // Act
        defaultUser.RemoveRole(roleId);

        // Assert
        defaultUser.Roles.Value.Should().NotContain(roleId);
    }

    [Fact]
    public void RemoveRole_Should_ReturnRoleNotFound_WhenGuidIsNotInRolesList()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();

        Guid roleId = Guid.Empty;
        
        // Act
        Result result = defaultUser.RemoveRole(roleId);

        // Assert
        result.Error.Should().Be(UserErrors.RoleNotFound);
    }

    [Fact]
    public void Delete_Should_SetIsDeletedToTrue()
    {
        // Arrange
        User defaultUser = CreateDefaultUser();
        
        // Act
        defaultUser.Delete();

        // Assert
        defaultUser.IsDeleted.Should().BeTrue();
    }
}
