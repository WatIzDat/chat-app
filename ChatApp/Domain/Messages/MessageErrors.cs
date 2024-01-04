﻿using SharedKernel;

namespace Domain.Messages;

public class MessageErrors
{
    public static readonly Error ContentsTooLong = new(
        "Message.ContentsTooLong",
        $"The contents cannot be longer than {Message.ContentsMaxLength} characters.");

    public static readonly Error NotFound = new(
        "Message.NotFound",
        "The message with the requested ID was not found.");
}
