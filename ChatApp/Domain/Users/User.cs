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
        string aboutSection,
        List<Guid> discussions,
        List<Guid> roles)
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

    public string AboutSection { get; private set; }

    public List<Guid> Discussions { get; private set; }

    public List<Guid> Roles { get; private set; }

    public static Result<User> Create(
        string username,
        Email email,
        DateTimeOffset dateCreatedUtc,
        string aboutSection,
        List<Guid> discussions,
        List<Guid> roles)
    {
        if (username.Length > 20)
        {
            return Result.Failure<User>(UserErrors.UsernameTooLong);
        }

        aboutSection = aboutSection.ReplaceLineEndings("\r\n");

        if (aboutSection.Length > 200)
        {
            return Result.Failure<User>(UserErrors.AboutSectionTooLong);
        }

        if (discussions.Count > 100)
        {
            return Result.Failure<User>(UserErrors.TooManyDiscussions);
        }

        if (ListUtility.HasDuplicates(discussions))
        {
            return Result.Failure<User>(UserErrors.DuplicateDiscussions);
        }

        if (roles.Count > discussions.Count)
        {
            return Result.Failure<User>(UserErrors.TooManyRoles);
        }

        if (ListUtility.HasDuplicates(roles))
        {
            return Result.Failure<User>(UserErrors.DuplicateRoles);
        }

        User user = new(Guid.NewGuid(), username, email, dateCreatedUtc, aboutSection, discussions, roles);

        return Result.Success(user);
    }

    public void UpdateAboutSection(string aboutSection)
    {
        AboutSection = aboutSection;
    }

    public void JoinDiscussion(Guid discussionId)
    {
        Discussions.Add(discussionId);
    }

    public void LeaveDiscussion(Guid discussionId)
    {
        Discussions.Remove(discussionId);
    }

    public void AddRole(Guid roleId)
    {
        Roles.Add(roleId);
    }

    public void RemoveRole(Guid roleId) 
    {
        Roles.Remove(roleId);
    }
}
