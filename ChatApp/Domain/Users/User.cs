using System.Collections.ObjectModel;
using Domain.Bans;
using Domain.Discussions;
using Domain.Messages;
using SharedKernel;

namespace Domain.Users;

public sealed class User : Entity
{
    public const int UsernameMaxLength = 20;

    // Private parameterless constructor for EF
    private User()
    {
    }

    private User(
        Guid id,
        string username,
        Email email,
        DateTimeOffset dateCreatedUtc,
        AboutSection aboutSection,
        DiscussionsList discussions,
        RolesList roles,
        bool isDeleted,
        string clerkId)
        : base(id)
    {
        Username = username;
        Email = email;
        DateCreatedUtc = dateCreatedUtc;
        AboutSection = aboutSection;
        Discussions = discussions;
        Roles = roles;
        IsDeleted = isDeleted;
        ClerkId = clerkId;
    }

    public string Username { get; private set; }

    public Email Email { get; private set; }

    public DateTimeOffset DateCreatedUtc { get; private set; }

    public AboutSection AboutSection { get; private set; }

    public DiscussionsList Discussions { get; private set; }

    public RolesList Roles { get; private set; }

    public bool IsDeleted { get; private set; }

    public string ClerkId { get; private set; }

    // Navigation property for EF
    public ICollection<Discussion> DiscussionsNavigation { get; set; } = null!;

    // Navigation property for EF
    public ICollection<Message> MessagesNavigation { get; set; } = null!;

    // Navigation property for EF
    public ICollection<Ban> BansNavigation { get; set; } = null!;

    public static Result<User> Create(
        string username,
        Email email,
        DateTimeOffset dateCreatedUtc,
        AboutSection aboutSection,
        DiscussionsList discussions,
        RolesList roles,
        string clerkId)
    {
        if (username.Length > UsernameMaxLength)
        {
            return Result.Failure<User>(UserErrors.UsernameTooLong);
        }

        User user = new(Guid.NewGuid(), username, email, dateCreatedUtc, aboutSection, discussions, roles, isDeleted: false, clerkId);

        return Result.Success(user);
    }

    public Result ChangeUsername(string username)
    {
        if (username.Length > UsernameMaxLength)
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
        List<Guid> discussions = new(Discussions.Value)
        {
            discussionId
        };

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
        List<Guid> discussions = new(Discussions.Value);

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
        List<Guid> roles = new(Roles.Value)
        {
            roleId
        };

        if (roles.Count > Discussions.Value.Count)
        {
            return Result.Failure(RolesListErrors.TooManyRoles);
        }

        Result<RolesList> result = RolesList.Create(roles);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Roles = result.Value;

        return Result.Success();
    }

    public Result RemoveRole(Guid roleId)
    {
        List<Guid> roles = new(Roles.Value);

        bool roleRemoved = roles.Remove(roleId);

        if (!roleRemoved)
        {
            return Result.Failure(UserErrors.RoleNotFound);
        }

        Result<RolesList> result = RolesList.Create(roles);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        Roles = result.Value;

        return Result.Success();
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
