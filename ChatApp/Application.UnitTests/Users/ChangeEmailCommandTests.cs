using Application.Users.ChangeEmail;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class ChangeEmailCommandTests
{
    private static readonly DiscussionsList Discussions =
        DiscussionsList.Create([Guid.NewGuid()]).Value;

    private static readonly RolesList Roles =
        RolesList.Create([Guid.NewGuid()]).Value;

    private static readonly User User = User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.UtcNow,
            AboutSection.Create("This is a test.").Value,
            Discussions,
            Roles,
            "test").Value;

    private readonly ChangeEmailCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    public ChangeEmailCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new ChangeEmailCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        ChangeEmailCommand command = new(user.Id, "hello@hello.com");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        user.Email.Should().Be(Email.Create(command.Email).Value);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        ChangeEmailCommand command = new(user.Id, "hello@hello.com");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnEmailInvalidFormat_WhenEmailHasInvalidFormat()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        ChangeEmailCommand command = new(user.Id, "thisemailisinvalid");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(EmailErrors.InvalidFormat);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles,
            User.ClerkId).Value;

        ChangeEmailCommand command = new(user.Id, "hello@hello.com");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
