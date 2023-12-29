using SharedKernel;

namespace Domain.Users;

public sealed class AboutSection
{
    private AboutSection(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<AboutSection> Create(string value)
    {
        value = value.ReplaceLineEndings("\r\n");

        if (value.Length > 200)
        {
            return Result.Failure<AboutSection>(AboutSectionErrors.TooLong);
        }

        return Result.Success(new AboutSection(value));
    }
}
