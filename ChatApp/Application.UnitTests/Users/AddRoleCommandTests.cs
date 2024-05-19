using Application.Users.AddRole;
using Domain.Roles;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class AddRoleCommandTests
{
    private static User CreateDefaultUser()
    {
        return User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.MinValue,
            AboutSection.Create("This is a test.").Value,
            CreateDefaultDiscussionsList(),
            CreateDefaultRolesList(),
            "test").Value;
    }

    private static DiscussionsList CreateDefaultDiscussionsList()
    {
        // Discussions list has two arbitrary discussions by default to simplify testing
        return DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;
    }

    private static RolesList CreateDefaultRolesList()
    {
        return RolesList.Create([]).Value;
    }

    private static readonly Guid RoleId = Guid.Empty;

    private readonly AddRoleCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IRoleRepository roleRepositoryMock;

    private void ConfigureMocks(User user, AddRoleCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(true);
        roleRepositoryMock
            .RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions))
            .Returns(true);

        overrides?.Invoke();
    }

    public AddRoleCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new AddRoleCommandHandler(userRepositoryMock, roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);
        
        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command, overrides: () =>
        {
            userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenRoleExistsAsyncReturnsFalse()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command, overrides: () =>
        {
            roleRepositoryMock.RoleExistsAsync(Arg.Is(command.RoleId)).Returns(false);
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotInDiscussionsList_WhenRoleInDiscussionsListAsyncReturnsFalse()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command, overrides: () =>
        {
            roleRepositoryMock
                .RoleInDiscussionsListAsync(Arg.Is(command.RoleId), Arg.Is(user.Discussions))
                .Returns(false);
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.RoleNotInDiscussionsList);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoles_WhenDuplicateGuidIsAdded()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        await commandHandler.Handle(command, default);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RolesListErrors.DuplicateRoles);
    }

    [Fact]
    public async Task Handle_Should_ReturnTooManyRoles_WhenListIsLongerThanDiscussionsList()
    {
        // Arrange
        User referenceUser = CreateDefaultUser();

        DiscussionsList discussionsList = DiscussionsList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;
        RolesList rolesListWithSameLength = RolesList.Create([Guid.NewGuid(), Guid.NewGuid()]).Value;

        User user = User.Create(
            referenceUser.Username,
            referenceUser.Email,
            referenceUser.DateCreatedUtc,
            referenceUser.AboutSection,
            discussionsList,
            rolesListWithSameLength,
            referenceUser.ClerkId).Value;

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RolesListErrors.TooManyRoles);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        AddRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
