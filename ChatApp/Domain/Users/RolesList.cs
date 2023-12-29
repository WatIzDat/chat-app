using SharedKernel.Utility;
using SharedKernel;
using System.Collections.ObjectModel;

namespace Domain.Users;

public sealed class RolesList
{
    private RolesList(List<Guid> value)
    {
        Value = value.AsReadOnly();
    }

    public ReadOnlyCollection<Guid> Value { get; }

    // Uses discussion list to ensure valid state
    public static Result<RolesList> Create(List<Guid> value, DiscussionsList discussions)
    {
        Result result = Validate(value, discussions);

        if (result.IsFailure)
        {
            return Result.Failure<RolesList>(result.Error);
        }

        return Result.Success(new RolesList(value));
    }

    private static Result Validate(List<Guid> value, DiscussionsList discussions)
    {
        if (value.Count > discussions.Value.Count)
        {
            return Result.Failure(RolesListErrors.TooManyRoles);
        }

        if (ListUtility.HasDuplicates(value))
        {
            return Result.Failure(RolesListErrors.DuplicateRoles);
        }

        return Result.Success();
    }
}
