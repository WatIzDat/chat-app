using Domain.Discussions;
using Domain.Users;
using SharedKernel;

namespace Domain.Messages;

public sealed class Message : Entity
{
    public const int ContentsMaxLength = 500;

    // Private parameterless constructor for EF
    private Message()
    {
    }

    private Message(
        Guid id,
        Guid userId,
        Guid discussionId,
        string contents,
        DateTimeOffset dateSentUtc,
        bool isEdited)
        : base(id)
    {
        UserId = userId;
        DiscussionId = discussionId;
        Contents = contents;
        DateSentUtc = dateSentUtc;
        IsEdited = isEdited;
    }

    public Guid UserId { get; private set; }

    public Guid DiscussionId { get; private set; }

    public string Contents { get; private set; }

    public DateTimeOffset DateSentUtc { get; private set; }

    public bool IsEdited { get; private set; }

    // Navigation property for EF
    public User UserNavigation { get; set; } = null!;

    // Navigation property for EF
    public Discussion DiscussionNavigation { get; set; } = null!;

    public static Result<Message> Create(
        Guid userId,
        Guid discussionId,
        string contents,
        DateTimeOffset dateSentUtc)
    {
        if (contents.Length > ContentsMaxLength)
        {
            return Result.Failure<Message>(MessageErrors.ContentsTooLong);
        }

        Message message = new(Guid.NewGuid(), userId, discussionId, contents, dateSentUtc, isEdited: false);

        return Result.Success(message);
    }

    public Result EditContents(string contents)
    {
        if (contents.Length > ContentsMaxLength)
        {
            return Result.Failure(MessageErrors.ContentsTooLong);
        }

        Contents = contents;

        IsEdited = true;

        return Result.Success();
    }
}
