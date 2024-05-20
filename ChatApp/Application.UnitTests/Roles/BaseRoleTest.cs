using Domain.Roles;

namespace Application.UnitTests.Roles;

public abstract class BaseRoleTest<T>
{
    protected virtual Role CreateDefaultRole()
    {
        return Role.Create(
            Guid.Empty,
            "test",
            CreateDefaultPermissionsList()).Value;
    }

    protected virtual List<Permission> CreateDefaultPermissionsList()
    {
        return [];
    }

    protected virtual void ConfigureMocks(T command, Action? overrides = null)
    {
        overrides?.Invoke();
    }

    protected virtual void ConfigureMocks(Role role, T command, Action? overrides = null)
    {
        overrides?.Invoke();
    }
}
