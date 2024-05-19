using Application.Users.UpdateAboutSection;
using Domain.Users;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Users;

public class UpdateAboutSectionCommandTests : BaseUserTest<UpdateAboutSectionCommand>
{
    private const string ValidAboutSection = "test";

    private readonly UpdateAboutSectionCommandHandler commandHandler;
    private readonly IUserRepository userRepositoryMock;

    protected override void ConfigureMocks(User user, UpdateAboutSectionCommand command, Action? overrides = null)
    {
        userRepositoryMock.GetByIdAsync(Arg.Is(command.UserId)).Returns(user);

        base.ConfigureMocks(user, command, overrides);
    }

    public UpdateAboutSectionCommandTests()
    {
        userRepositoryMock = Substitute.For<IUserRepository>();

        commandHandler = new UpdateAboutSectionCommandHandler(userRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        User user = CreateDefaultUser();

        UpdateAboutSectionCommand command = new(user.Id, ValidAboutSection);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ChangeAboutSection()
    {
        // Arrange
        User user = CreateDefaultUser();

        UpdateAboutSectionCommand command = new(user.Id, ValidAboutSection);

        ConfigureMocks(user, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        user.AboutSection.Should().Be(AboutSection.Create(command.AboutSection).Value);
    }

    [Fact]
    public async Task Handle_Should_ReturnUserNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        User user = CreateDefaultUser();

        UpdateAboutSectionCommand command = new(user.Id, ValidAboutSection);

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
    public async Task Handle_Should_ReturnAboutSectionTooLong_WhenAboutSectionIsLongerThanMaxLength()
    {
        // Arrange
        User user = CreateDefaultUser();

        string aboutSectionLongerThanMaxLength = string.Empty.PadLeft(AboutSection.MaxLength + 1);

        UpdateAboutSectionCommand command = new(user.Id, aboutSectionLongerThanMaxLength);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(AboutSectionErrors.TooLong);
    }

    [Fact]
    public async Task Handle_Should_CallUserRepositoryUpdate()
    {
        // Arrange
        User user = CreateDefaultUser();

        UpdateAboutSectionCommand command = new(user.Id, ValidAboutSection);

        ConfigureMocks(user, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        userRepositoryMock
            .Received(1)
            .Update(Arg.Is<User>(u => u.Id == command.UserId));
    }
}
