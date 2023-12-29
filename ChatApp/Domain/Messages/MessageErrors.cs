using SharedKernel;

namespace Domain.Messages;

public class MessageErrors
{
    public static readonly Error ContentsTooLong = new(
        "Message.ContentsTooLong",
        $"The contents cannot be longer than {Message.ContentsMaxLength} characters.");
}