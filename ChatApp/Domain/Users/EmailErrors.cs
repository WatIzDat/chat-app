using SharedKernel;

namespace Domain.Users;

public static class EmailErrors
{
    public static readonly Error Empty = Error.Validation("Email.Empty", "Email was null or empty.");
    public static readonly Error InvalidFormat = Error.Validation("Email.InvalidFormat", "Email had an invalid format.");
}