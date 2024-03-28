using SharedKernel.Utility;
using SharedKernel;
using System.Collections.ObjectModel;

namespace Domain.Users;

public sealed record RolesList
{
    private RolesList(List<Guid> value)
    {
        Value = value.AsReadOnly();
    }

    public ReadOnlyCollection<Guid> Value { get; }

    public static Result<RolesList> Create(List<Guid> value)
    {
        Result result = Validate(value);

        if (result.IsFailure)
        {
            return Result.Failure<RolesList>(result.Error);
        }

        return Result.Success(new RolesList(value));
    }

    private static Result Validate(List<Guid> value)
    {
        if (ListUtility.HasDuplicates(value))
        {
            return Result.Failure(RolesListErrors.DuplicateRoles);
        }

        return Result.Success();
    }
}
