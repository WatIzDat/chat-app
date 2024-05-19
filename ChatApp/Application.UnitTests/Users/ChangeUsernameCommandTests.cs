using Application.Users.ChangeUsername;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class ChangeUsernameCommandTests : BaseUserTest<ChangeUsernameCommand>
{
    private const string ValidUsername = "test";

    private readonly ChangeUsernameCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, ChangeUsernameCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);
        userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(command.Username)).Returns(true);

        base.ConfigureMocks(user, command, overrides);
    }

    public ChangeUsernameCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new ChangeUsernameCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeUsernameCommand command = new(user.Id, ValidUsername);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ChangeUsername()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeUsernameCommand command = new(user.Id, ValidUsername);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.Username.Should().Be(command.Username);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeUsernameCommand command = new(user.Id, ValidUsername);

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
    public async Task Handle_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Arrange
        User user = CreateDefaultUser();

        string usernameLongerThanMaxLength = string.Empty.PadLeft(User.UsernameMaxLength + 1);

        ChangeUsernameCommand command = new(user.Id, usernameLongerThanMaxLength);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public async Task Handle_Should_ReturnUsernameNotUnique_WhenIsUsernameUniqueAsyncReturnsFalse()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeUsernameCommand command = new(user.Id, ValidUsername);

        ConfigureMocks(user, command, overrides: () =>
        {
            userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(command.Username)).Returns(false);
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameNotUnique);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeUsernameCommand command = new(user.Id, ValidUsername);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
