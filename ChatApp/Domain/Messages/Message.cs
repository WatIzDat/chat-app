using SharedKernel;

namespace Domain.Messages;

public sealed class Message : Entity
{
    public const int ContentsMaxLength = 500;

    private Message(
        Guid id,
        Guid userId,
        Guid discussionId,
        string contents,
        DateTimeOffset dateSentUtc)
        : base(id)
    {
        UserId = userId;
        DiscussionId = discussionId;
        Contents = contents;
        DateSentUtc = dateSentUtc;
    }

    public Guid UserId { get; private set; }

    public Guid DiscussionId { get; private set; }

    public string Contents { get; private set; }

    public DateTimeOffset DateSentUtc { get; private set; }

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

        Message message = new(Guid.NewGuid(), userId, discussionId, contents, dateSentUtc);

        return Result.Success(message);
    }

    public Result EditContents(string contents)
    {
        if (contents.Length > ContentsMaxLength)
        {
            return Result.Failure(MessageErrors.ContentsTooLong);
        }

        Contents = contents;

        return Result.Success();
    }
}
