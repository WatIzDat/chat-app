using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Domain.UnitTests.Users;

public class UserTests
{
    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles).Value;

    [Fact]
    public void Create_Should_ReturnSuccess()
    {
        // Act
        Result<User> result = User.Create(
            "test123",
            User.Email,
            DateTimeOffset.UtcNow,
            User.AboutSection,
            Discussions,
            Roles);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public void Create_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Act
        Result<User> result = User.Create(
            "thisusernameiswaytoolong",
            User.Email,
            DateTimeOffset.UtcNow,
            User.AboutSection,
            Discussions,
            Roles);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public void ChangeUsername_Should_ReturnSuccess_And_ChangeUsername()
    {
        // Act
        Result result = User.ChangeUsername("hello");

        // Assert
        result.IsSuccess.Should().BeTrue();

        User.Username.Should().Be("hello");
    }

    [Fact]
    public void ChangeUsername_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Act
        Result result = User.ChangeUsername("thisusernameiswaytoolong");

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public void ChangeEmail_Should_ChangeEmail()
    {
        // Act
        User.ChangeEmail(Email.Create("hello@hello.com").Value);

        // Assert
        User.Email.Should().Be(Email.Create("hello@hello.com").Value);
    }

    [Fact]
    public void UpdateAboutSection_Should_ChangeAboutSection()
    {
        // Act
        User.UpdateAboutSection(AboutSection.Create("This is another test about section.").Value);

        // Assert
        User.AboutSection.Should().Be(AboutSection.Create("This is another test about section.").Value);
    }

    [Fact]
    public void JoinDiscussion_Should_ReturnSuccess_And_AddDiscussionToDiscussionsList()
    {
        // Arrange
        Guid discussionId = Guid.NewGuid();

        // Act
        Result result = User.JoinDiscussion(discussionId);

        // Assert
        result.IsSuccess.Should().BeTrue();

        User.Discussions.Value.Should().Contain(discussionId);
    }

    [Fact]
    public void JoinDiscussion_Should_ReturnDuplicateDiscussions_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        Guid discussionId = Guid.NewGuid();
        User.JoinDiscussion(discussionId);
        
        // Act
        Result result = User.JoinDiscussion(discussionId);

        // Assert
        result.Error.Should().Be(DiscussionsListErrors.DuplicateDiscussions);

        User.Discussions.Value.Should().ContainInConsecutiveOrder(
            Discussions.Value[0],
            Discussions.Value[1],
            discussionId);
    }

    [Fact]
    public void LeaveDiscussion_Should_ReturnSuccess_And_RemoveDiscussionFromDiscussionsList()
    {
        // Act
        Result result = User.LeaveDiscussion(Discussions.Value[0]);

        // Assert
        result.IsSuccess.Should().BeTrue();

        User.Discussions.Value.Should().NotContain(Discussions.Value[0]);
    }

    [Fact]
    public void LeaveDiscussion_Should_ReturnDiscussionNotFound_WhenGuidIsNotInDiscussionsList()
    {
        // Act
        Result result = User.LeaveDiscussion(Guid.NewGuid());

        // Assert
        result.Error.Should().Be(UserErrors.DiscussionNotFound);
    }

    [Fact]
    public void AddRole_Should_ReturnSuccess_And_AddRoleToRolesList()
    {
        // Arrange
        Guid roleId = Guid.NewGuid();

        User.JoinDiscussion(Guid.NewGuid());

        // Act
        Result result = User.AddRole(roleId);

        // Assert
        result.IsSuccess.Should().BeTrue();

        User.Roles.Value.Should().Contain(roleId);
    }

    [Fact]
    public void AddRole_Should_ReturnDuplicateRoles_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        Guid roleId = Guid.NewGuid();

        User.JoinDiscussion(Guid.NewGuid());
        User.JoinDiscussion(Guid.NewGuid());

        // Act
        User.AddRole(roleId);
        Result result = User.AddRole(roleId);

        // Assert
        result.Error.Should().Be(RolesListErrors.DuplicateRoles);

        User.Roles.Value.Should().ContainInConsecutiveOrder(
            Roles.Value[0],
            Roles.Value[1],
            roleId);
    }

    [Fact]
    public void RemoveRole_Should_ReturnSuccess_And_RemoveRoleFromRolesList()
    {
        // Act
        Result result = User.RemoveRole(Roles.Value[0]);

        // Assert
        result.IsSuccess.Should().BeTrue();

        User.Roles.Value.Should().NotContain(Discussions.Value[0]);
    }

    [Fact]
    public void RemoveRole_Should_ReturnRoleNotFound_WhenGuidIsNotInRolesList()
    {
        // Act
        Result result = User.RemoveRole(Guid.NewGuid());

        // Assert
        result.Error.Should().Be(UserErrors.RoleNotFound);
    }
}
