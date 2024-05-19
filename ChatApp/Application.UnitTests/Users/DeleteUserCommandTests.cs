using Application.Users.DeleteUser;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class DeleteUserCommandTests : BaseUserTest<DeleteUserCommand>
{
    private readonly DeleteUserCommmandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, DeleteUserCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        base.ConfigureMocks(user, command, overrides);
    }

    public DeleteUserCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new DeleteUserCommmandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        DeleteUserCommand command = new(user.Id);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_SetIsDeletedToTrue()
    {
        // Arrange
        User user = CreateDefaultUser();

        DeleteUserCommand command = new(user.Id);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        DeleteUserCommand command = new(user.Id);

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
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        DeleteUserCommand command = new(user.Id);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
