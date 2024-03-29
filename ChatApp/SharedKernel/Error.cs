namespace SharedKernel;

public sealed record Error
{
    public static readonly Error None = new(string.Empty, string.Empty, ErrorType.None);

    private Error(string code, string description, ErrorType errorType)
    {
        Code = code;
        Description = description;
        Type = errorType;
    }

    public string Code { get;  }

    public string Description { get; }

    public ErrorType Type { get; }

    public static Error Failure(string code, string description)
    {
        return new(code, description, ErrorType.Failure);
    }

    public static Error NotFound(string code, string description)
    {
        return new(code, description, ErrorType.NotFound);
    }

    public static Error Validation(string code, string description)
    {
        return new(code, description, ErrorType.Validation);
    }

    public static Error Conflict(string code, string description)
    {
        return new(code, description, ErrorType.Conflict);
    }
}

public enum ErrorType
{
    None,
    Failure,
    Validation,
    NotFound,
    Conflict
}
