namespace Domain.Roles;

public sealed record Permission
{
    public Permission(string value)
    {
        Value = value;
    }

    public string Value { get; }
}