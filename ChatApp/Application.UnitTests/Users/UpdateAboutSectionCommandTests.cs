using Application.Users.UpdateAboutSection;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class UpdateAboutSectionCommandTests
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

    private readonly UpdateAboutSectionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    public UpdateAboutSectionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new UpdateAboutSectionCommandHandler(userRepositoryMock);
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

        UpdateAboutSectionCommand command = new(user.Id, "new about section");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        user.AboutSection.Should().Be(AboutSection.Create(command.AboutSection).Value);
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

        UpdateAboutSectionCommand command = new(user.Id, "new about section");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(UserErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnAboutSectionTooLong_WhenAboutSectionIsLongerThanMaxLength()
    {
        User user = User.Create(
            User.Username,
            User.Email,
            User.DateCreatedUtc,
            User.AboutSection,
            User.Discussions,
            User.Roles).Value;

        string aboutSection = string.Empty;

        for (int i = 0; i < AboutSection.MaxLength + 1; i++)
        {
            aboutSection += "a";
        }

        UpdateAboutSectionCommand command = new(user.Id, aboutSection);

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(AboutSectionErrors.TooLong);
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

        UpdateAboutSectionCommand command = new(user.Id, "new about section");

        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        Result result = await commandHandler.Handle(command, default);

        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
