using SharedKernel;

namespace Domain.Users;

public static class AboutSectionErrors
{
    public static readonly Error TooLong = new(
        "AboutSection.TooLong",
        "The about section cannot be longer than 200 characters.");
}