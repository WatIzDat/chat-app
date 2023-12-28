using SharedKernel;

namespace Domain.Discussions;

public sealed class Discussion : Entity
{
    private Discussion(
        Guid id,
        Guid userCreatedBy,
        string name,
        DateTimeOffset dateCreatedUtc)
        : base(id)
    {
        UserCreatedBy = userCreatedBy;
        Name = name;
        DateCreatedUtc = dateCreatedUtc;
    }

    public Guid UserCreatedBy { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset DateCreatedUtc { get; private set; }

    public static Result<Discussion> Create(
        Guid userCreatedBy,
        string name,
        DateTimeOffset dateCreatedUtc)
    {
        if (name.Length > 50)
        {
            return Result.Failure<Discussion>(DiscussionErrors.NameTooLong);
        }

        Discussion discussion = new(Guid.NewGuid(), userCreatedBy, name, dateCreatedUtc);

        return Result.Success(discussion);
    }

    public void EditName(string name)
    {
        Name = name;
    }
}
