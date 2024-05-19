using Domain.Users;

namespace Application.UnitTests.Users;

public abstract class BaseUserTest<T>
{
    protected virtual User CreateDefaultUser()
    {
        return User.Create(
            "test123",
            Email.Create("test@test.com").Value,
            DateTimeOffset.MinValue,
            AboutSection.Create("This is a test.").Value,
            CreateDefaultDiscussionsList(),
            CreateDefaultRolesList(),
            "test").Value;
    }

    protected virtual DiscussionsList CreateDefaultDiscussionsList()
    {
        return DiscussionsList.Create([]).Value;
    }

    protected virtual RolesList CreateDefaultRolesList()
    {
        return RolesList.Create([]).Value;
    }

    protected virtual void ConfigureMocks(User user, T command, Action? overrides = null)
    {
        overrides?.Invoke();
    }
}
