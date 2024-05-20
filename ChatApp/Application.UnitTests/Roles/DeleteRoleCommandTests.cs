using Application.Roles.DeleteRole;
using Domain.Roles;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Roles;

public class DeleteRoleCommandTests : BaseRoleTest<DeleteRoleCommand>
{
    private readonly DeleteRoleCommandHandler commandHandler;
    private readonly IRoleRepository roleRepositoryMock;

    protected override void ConfigureMocks(Role role, DeleteRoleCommand command, Action? overrides = null)
    {
        roleRepositoryMock.GetByIdAsync(Arg.Is(command.RoleId)).Returns(role);

        base.ConfigureMocks(role, command, overrides);
    }

    public DeleteRoleCommandTests()
    {
        roleRepositoryMock = Substitute.For<IRoleRepository>();

        commandHandler = new DeleteRoleCommandHandler(roleRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        // Arrange
        Role role = CreateDefaultRole();

        DeleteRoleCommand command = new(role.Id);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        result.IsSuccess.Should().BeTrue();
    }
    
    [Fact]
    public async Task Handle_Should_ReturnRoleNotFound_WhenGetByIdAsyncReturnsNull()
    {
        // Arrange
        Role role = CreateDefaultRole();

        DeleteRoleCommand command = new(role.Id);

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
    public async Task Handle_Should_CallRoleRepositoryDelete()
    {
        // Arrange
        Role role = CreateDefaultRole();

        DeleteRoleCommand command = new(role.Id);

        ConfigureMocks(role, command);

        // Act
        Result result = await commandHandler.Handle(command, default);

        // Assert
        roleRepositoryMock
            .Received(1)
            .Delete(Arg.Is<Role>(r => r.Id == command.RoleId));
    }
}
