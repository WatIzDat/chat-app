using Application.Roles.EditName;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class EditNameCommandTests : BaseRoleTest<EditNameCommand>
{
    private const string ValidName = "test";

    private readonly EditNameCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    protected override void ConfigureMocks(Role role, EditNameCommand command, Action? overrides = null)
    {
        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);
        roleRepositoryMock.DuplicateRoleNamesInDiscussionAsync(command.Name, role.DiscussionId).Returns(false);

        base.ConfigureMocks(role, command, overrides);
    }

    public EditNameCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new EditNameCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Role role = CreateDefaultRole();

        EditNameCommand command = new(role.Id, ValidName);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_EditName()
    {
        // Arrange
        Role role = CreateDefaultRole();

        EditNameCommand command = new(role.Id, ValidName);

        ConfigureMocks(role, command);

        // Act
        await commandHandler.Handle(command, default);

        // Assert
        role.Name.Should().Be(command.Name);
    }

    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Role role = CreateDefaultRole();

        EditNameCommand command = new(role.Id, ValidName);

        ConfigureMocks(role, command, overrides: () =>
        {
            roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).ReturnsNull();
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnDuplicateRoleNamesInDiscussion_WhenDuplicateRoleNamesInDiscussionAsyncReturnsTrue()
    {
        // Arrange
        Role role = CreateDefaultRole();

        EditNameCommand command = new(role.Id, ValidName);

        ConfigureMocks(role, command, overrides: () =>
        {
            roleRepositoryMock
                .DuplicateRoleNamesInDiscussionAsync(command.Name, role.DiscussionId)
                .Returns(true);
        });

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.DuplicateRoleNamesInDiscussion);
    }

    [Fact]
    public async Task Handle_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        // Arrange
        Role role = CreateDefaultRole();

        string nameLongerThanMaxLength = string.Empty.PadLeft(Role.NameMaxLength + 1);

        EditNameCommand command = new(role.Id, nameLongerThanMaxLength);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.Error.Should().Be(RoleErrors.NameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallRoleRepositoryUpdate()
    {
        // Arrange
        Role role = CreateDefaultRole();

        EditNameCommand command = new(role.Id, ValidName);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        roleRepositoryMock
            .Received(1)
            .Update(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
