using Application.Users.RemoveRole;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class RemoveRoleCommandTests : BaseUserTest<RemoveRoleCommand>
{
    protected override RolesList CreateDefaultRolesList()
    {
        return RolesList.Create([Guid.Empty]).Value;
    }

    private static readonly Guid RoleId = Guid.Empty;

    private readonly RemoveRoleCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, RemoveRoleCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        base.ConfigureMocks(user, command, overrides);
    }

    public RemoveRoleCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new RemoveRoleCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        RemoveRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_RemoveRole()
    {
        // Arrange
        User user = CreateDefaultUser();

        RemoveRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.Roles.Value.Should().NotContain(command.RoleId);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        RemoveRoleCommand command = new(user.Id, RoleId);

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
    public async Task Handle_Should_ReturnRoleNotFound_WhenGuidIsNotInRolesList()
    {
        // Arrange
        User user = CreateDefaultUser();

        RemoveRoleCommand roleNotFoundCommand = new(
            user.Id,
            Guid.NewGuid());

        ConfigureMocks(user, roleNotFoundCommand);

        // Act
        Result result = await commandHandler.Handle(roleNotFoundCommand, default);

        // Assert
        result.Error.Should().Be(UserErrors.RoleNotFound);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        RemoveRoleCommand command = new(user.Id, RoleId);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
