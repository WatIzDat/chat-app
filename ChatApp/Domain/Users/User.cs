using Domain.Discussions;
using SharedKernel;
using SharedKernel.Utility;

namespace Domain.Users;

public sealed class User : Entity
{
    private User(
        Guid id,
        string username,
        Email email,
        DateTimeOffset dateCreatedUtc,
        AboutSection aboutSection,
        DiscussionsList discussions,
        RolesList roles)
        : base(id)
    {
        Username = username;
        Email = email;
        DateCreatedUtc = dateCreatedUtc;
        AboutSection = aboutSection;
        Discussions = discussions;
        Roles = roles;
    }

    public string Username { get; private set; }

    public Email Email { get; private set; } 

    public DateTimeOffset DateCreatedUtc { get; private set; }

    public AboutSection AboutSection { get; private set; }

    public DiscussionsList Discussions { get; private set; }

    public RolesList Roles { get; private set; }

    public static Result<User> Create(
        string username,
        Email email,
        DateTimeOffset dateCreatedUtc,
        AboutSection aboutSection,
        DiscussionsList discussions,
        RolesList roles)
    {
        if (username.Length > 20)
        {
            return Result.Failure<User>(UserErrors.UsernameTooLong);
        }

        User user = new(Guid.NewGuid(), username, email, dateCreatedUtc, aboutSection, discussions, roles);

        return Result.Success(user);
    }

    public Result ChangeUsername(string username)
    {
        if (username.Length > 20)
        {
            return Result.Failure(UserErrors.UsernameTooLong);
        }

        Username = username;

        return Result.Success();
    }

    public void ChangeEmail(Email email)
    {
        Email = email;
    }

    public void UpdateAboutSection(AboutSection aboutSection)
    {
        AboutSection = aboutSection;
    }

    public Result JoinDiscussion(Guid discussionId)
    {
        List<Guid> discussions = [.. Discussions.Value];

        discussions.Add(discussionId);

        Result<DiscussionsList> result = DiscussionsList.Create(discussions);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Discussions = result.Value;

        return Result.Success();
    }

    public Result LeaveDiscussion(Guid discussionId)
    {
        List<Guid> discussions = [.. Discussions.Value];

        bool discussionRemoved = discussions.Remove(discussionId);

        if (!discussionRemoved)
        {
            return Result.Failure(UserErrors.DiscussionNotFound);
        }

        Result<DiscussionsList> result = DiscussionsList.Create(discussions);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Discussions = result.Value;

        return Result.Success();
    }

    public Result AddRole(Guid roleId)
    {
        List<Guid> roles = [.. Roles.Value];

        roles.Add(roleId);

        Result<RolesList> result = RolesList.Create(roles, Discussions);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Roles = result.Value;

        return Result.Success();
    }

    public Result RemoveRole(Guid roleId) 
    {
        List<Guid> roles = [.. Roles.Value];

        bool roleRemoved = roles.Remove(roleId);

        if (!roleRemoved)
        {
            return Result.Failure(UserErrors.RoleNotFound);
        }

        Result<RolesList> result = RolesList.Create(roles, Discussions);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Roles = result.Value;

        return Result.Success();
    }
}
