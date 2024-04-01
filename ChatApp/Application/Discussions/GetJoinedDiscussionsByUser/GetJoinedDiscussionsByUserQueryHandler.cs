using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Collections.ObjectModel;

namespace Application.Discussions.GetJoinedDiscussionsByUser;

internal sealed class GetJoinedDiscussionsByUserQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetJoinedDiscussionsByUserQuery, List<DiscussionResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<DiscussionResponse>>> Handle(GetJoinedDiscussionsByUserQuery request, CancellationToken cancellationToken)
    {
        ReadOnlyCollection<Guid>? discussionIds = await dbContext.Users
            .Where(u => u.Id == request.UserId && u.IsDeleted == false)
            .Select(u => u.Discussions.Value)
            .FirstOrDefaultAsync(cancellationToken);

        if (discussionIds == null)
        {
            return Result.Failure<List<DiscussionResponse>>(UserErrors.NotFound);
        }

        List<DiscussionResponse> discussions = [];

        foreach (Guid id in discussionIds)
        {
            DiscussionResponse discussion = await dbContext.Discussions
                .Where(d => d.Id == id)
                .Select(d => new DiscussionResponse
                {
                    Id = d.Id,
                    Name = d.Name
                })
                .FirstAsync(cancellationToken);

            discussions.Add(discussion);
        }

        return Result.Success(discussions);
    }
}
