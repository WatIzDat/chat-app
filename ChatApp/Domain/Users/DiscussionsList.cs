using SharedKernel;
using SharedKernel.Utility;
using System.Collections.ObjectModel;

namespace Domain.Users;

public sealed class DiscussionsList
{
    public const int MaxLength = 100;

    private DiscussionsList(List<Guid> value)
    {
        Value = value.AsReadOnly();
    }

    public ReadOnlyCollection<Guid> Value { get; }
    
    public static Result<DiscussionsList> Create(List<Guid> value)
    {
        Result result = Validate(value);

        if (result.IsFailure)
        {
            return Result.Failure<DiscussionsList>(result.Error);
        }

        return Result.Success(new DiscussionsList(value));
    }

    private static Result Validate(List<Guid> value)
    {
        if (value.Count > MaxLength)
        {
            return Result.Failure(DiscussionsListErrors.TooManyDiscussions);
        }

        if (ListUtility.HasDuplicates(value))
        {
            return Result.Failure(DiscussionsListErrors.DuplicateDiscussions);
        }

        return Result.Success();
    }
}
