using SharedKernel;

namespace Domain.Users;

public static class EmailErrors
{
    public static readonly Error Empty = new("Email.Empty", "Email was null or empty.");
    public static readonly Error InvalidFormat = new("Email.InvalidFormat", "Email had an invalid format.");
}