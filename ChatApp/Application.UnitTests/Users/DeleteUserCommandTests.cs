using Application.Users.DeleteUser;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class DeleteUserCommandTests
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
            Roles).Value;

    private readonly DeleteUserCommmandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    public DeleteUserCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new DeleteUserCommmandHandler(userRepositoryMock);
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
            User.Roles).Value;

        DeleteUserCommand command = new(user.Id);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        user.IsDeleted.Should().BeTrue();
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
            User.Roles).Value;

        DeleteUserCommand command = new(user.Id);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
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
            User.Roles).Value;

        DeleteUserCommand command = new(user.Id);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
