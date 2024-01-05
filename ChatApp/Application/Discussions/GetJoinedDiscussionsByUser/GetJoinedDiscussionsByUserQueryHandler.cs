using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Discussions.GetJoinedDiscussionsByUser;

internal sealed class GetJoinedDiscussionsByUserQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetJoinedDiscussionsByUserQuery, List<DiscussionResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<DiscussionResponse>>> Handle(GetJoinedDiscussionsByUserQuery request, CancellationToken cancellationToken)
    {
        List<DiscussionResponse> discussions = await dbContext.Users
            .Where(u => u.Id == request.UserId)
            .Select(u => u.Discussions.Value.Select(id => dbContext.Discussions.Where(d => d.Id == id)))
            .First()
            .First()
            .Select(d => new DiscussionResponse
            {
                Id = d.Id,
                Name = d.Name
            })
            .ToListAsync(cancellationToken);

        return Result.Success(discussions);
    }
}

