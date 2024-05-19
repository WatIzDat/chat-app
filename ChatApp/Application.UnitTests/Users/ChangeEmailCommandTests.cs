using Application.Users.ChangeEmail;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class ChangeEmailCommandTests : BaseUserTest<ChangeEmailCommand>
{
    private const string ValidEmail = "test@test.com";

    private readonly ChangeEmailCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, ChangeEmailCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        base.ConfigureMocks(user, command, overrides);
    }

    public ChangeEmailCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new ChangeEmailCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeEmailCommand command = new(user.Id, ValidEmail);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ChangeEmail()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeEmailCommand command = new(user.Id, ValidEmail);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.Email.Should().Be(Email.Create(command.Email).Value);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeEmailCommand command = new(user.Id, ValidEmail);

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
    public async Task Handle_Should_ReturnEmailInvalidFormat_WhenEmailHasInvalidFormat()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeEmailCommand command = new(user.Id, "thisemailisinvalid");

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        ChangeEmailCommand command = new(user.Id, ValidEmail);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
