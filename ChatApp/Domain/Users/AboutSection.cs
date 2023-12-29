using SharedKernel;

namespace Domain.Users;

public sealed record AboutSection
{
    public const int MaxLength = 200;

    private AboutSection(string value)
    {
        Value = value;
    }

    public string Value { get; }

    public static Result<AboutSection> Create(string value)
    {
        value = value.ReplaceLineEndings("\r\n");

        if (value.Length > MaxLength)
        {
            return Result.Failure<AboutSection>(AboutSectionErrors.TooLong);
        }

        return Result.Success(new AboutSection(value));
    }
}
