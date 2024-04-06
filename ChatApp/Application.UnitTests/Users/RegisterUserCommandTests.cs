using Application.Users.RegisterUser;
using Domain.Users;
using SharedKernel;

namespace Application.UnitTests.Users;

public class RegisterUserCommandTests
{
    private static readonly RegisterUserCommand Command = new(
        "Full name", "test@test.com");

    private readonly RegisterUserCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProviderMock;

    public RegisterUserCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();
        dateTimeOffsetProviderMock = Substitute.For<IDateTimeOffsetProvider>();

        commandHandler = new RegisterUserCommandHandler(userRepositoryMock, dateTimeOffsetProviderMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        userRepositoryMock.IsEmailUniqueAsync(Arg.Is(Email.Create(Command.Email).Value)).Returns(true);
        userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(Command.Username)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnEmailInvalidFormat_WhenEmailHasInvalidFormat()
    {
        RegisterUserCommand invalidEmailCommand = new(
            Command.Username,
            "thisisaninvalidemail");

        Result<Guid> result = await commandHandler.Handle(invalidEmailCommand, default);

        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmailNotUnique_WhenIsEmailUniqueAsyncReturnsFalse()
    {
        userRepositoryMock.IsEmailUniqueAsync(Arg.Is(Email.Create(Command.Email).Value)).Returns(false);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(UserErrors.EmailNotUnique);
    }

    [Fact]
    public async Task Handle_Should_ReturnUsernameTooLong_WhenUsernameIsLongerThanMaxLength()
    {
        userRepositoryMock.IsEmailUniqueAsync(Arg.Is(Email.Create(Command.Email).Value)).Returns(true);

        RegisterUserCommand usernameTooLongCommand = new(
            "thisusernameiswaytoolong",
            Command.Email);

        userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(usernameTooLongCommand.Username)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(usernameTooLongCommand, default);

        result.Error.Should().Be(UserErrors.UsernameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryInsert()
    {
        userRepositoryMock.IsEmailUniqueAsync(Arg.Is(Email.Create(Command.Email).Value)).Returns(true);
        userRepositoryMock.IsUsernameUniqueAsync(Arg.Is(Command.Username)).Returns(true);

        Result<Guid> result = await commandHandler.Handle(Command, default);

        userRepositoryMock
            .Received(1)
            .Insert(Arg.Is<User>(u => u.Id == result.Value));
    }
}
