using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Discussions.GetCreatedDiscussionsByUser;

internal sealed class GetCreatedDiscussionsByUser(IApplicationDbContext dbContext)
    : IQueryHandler<GetCreatedDiscussionsByUserQuery, List<DiscussionResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<DiscussionResponse>>> Handle(GetCreatedDiscussionsByUserQuery request, CancellationToken cancellationToken)
    {
        List<DiscussionResponse> discussions = await dbContext.Discussions
            .Where(d => d.UserCreatedBy == request.UserId)
            .Select(d => new DiscussionResponse
            {
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync(cancellationToken);

        return Result.Success(discussions);
    }
}
