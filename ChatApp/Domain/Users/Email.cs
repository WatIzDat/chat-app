using SharedKernel;
using System.Net.Mail;

namespace Domain.Users;

public sealed record Email
{
    private Email(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<Email> Create(string value)
    {
        if (string.IsNullOrEmpty(value))
        {
            return Result.Failure<Email>(EmailErrors.Empty);
        }

        if (!IsValidFormat(value))
        {
            return Result.Failure<Email>(EmailErrors.InvalidFormat);
        }

        return Result.Success(new Email(value));
    }

    private static bool IsValidFormat(string value)
    {
        string trimmedEmail = value.Trim();

        if (string.IsNullOrEmpty(value) ||
            trimmedEmail.EndsWith('.') ||
            trimmedEmail.Length > 254) // 254 characters is RFC specification
        {
            return false;
        }

        try
        {
            MailAddress mailAddress = new(value);

            return mailAddress.Address == trimmedEmail;
        }
        catch
        {
            return false;
        }
    }
}