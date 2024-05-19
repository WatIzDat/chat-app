using Application.Users.RegisterUser;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Users;

public class RegisterUserCommandTests : BaseUserTest<RegisterUserCommand>
{
    private static readonly RegisterUserCommand Command = new(
        "Full name", "test@test.com", "test");

    private readonly RegisterUserCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;

    protected override void ConfigureMocks(RegisterUserCommand command, Action? overrides = null)
    {
        Result<Email> emailResult = Email.Create(command.Email);

        if (emailResult.IsSuccess)
        {
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is(emailResult.Value)).Returns(true);
        }

        userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(command.Username)).Returns(true);

        base.ConfigureMocks(command, overrides);
    }

    public RegisterUserCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new RegisterUserCommandHandler(userRepositoryMock, dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        ConfigureMocks(Command);

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnEmailInvalidFormat_WhenEmailHasInvalidFormat()
    {
        // Arrange
        RegisterUserCommand invalidEmailCommand = new(
            Command.Username,
            "thisisaninvalidemail",
            Command.ClerkId);

        ConfigureMocks(invalidEmailCommand);

        // Act
        Result<Guid> result = await commandHandler.Handle(invalidEmailCommand, default);

        // Assert
        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmailNotUnique_WhenIsEmailUniqueAsyncReturnsFalse()
    {
        // Arrange
        ConfigureMocks(Command, overrides: () =>
        {
            userRepositoryMock.IsEmailUniqueAsync(Arg.Is(Email.Create(Command.Email).Value)).Returns(false);
        });

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        result.Error.Should().Be(UserErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        // Arrange
        string usernameLongerThanMaxLength = string.Empty.PadLeft(User.UsernameMaxLength + 1);

        RegisterUserCommand usernameTooLongCommand = new(
            usernameLongerThanMaxLength,
            Command.Email,
            Command.ClerkId);

        ConfigureMocks(usernameTooLongCommand);

        // Act
        Result<Guid> result = await commandHandler.Handle(usernameTooLongCommand, default);

        // Assert
        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryInsert()
    {
        // Arrange
        ConfigureMocks(Command);

        // Act
        Result<Guid> result = await commandHandler.Handle(Command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Insert(Arg.Is<User>(u => u.Id == result.Value));
    }
}
