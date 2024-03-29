using SharedKernel;

namespace Domain.Messages;

public class MessageErrors
{
    public static readonly Error ContentsTooLong = Error.Validation(
        "Message.ContentsTooLong",
        $"The contents cannot be longer than {Message.ContentsMaxLength} characters.");

    public static readonly Error NotFound = Error.NotFound(
        "Message.NotFound",
        "The message with the requested ID was not found.");
}
