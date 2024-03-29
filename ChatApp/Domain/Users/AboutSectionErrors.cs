using SharedKernel;

namespace Domain.Users;

public static class AboutSectionErrors
{
    public static readonly Error TooLong = Error.Validation(
        "AboutSection.TooLong",
        $"The about section cannot be longer than {AboutSection.MaxLength} characters.");
}