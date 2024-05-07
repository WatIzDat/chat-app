using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Discussions;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Discussions.GetDiscussionById;

internal sealed class GetDiscussionByIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetDiscussionByIdQuery, DiscussionResponse>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<DiscussionResponse>> Handle(GetDiscussionByIdQuery request, CancellationToken cancellationToken)
    {
        DiscussionResponse? discussion = await dbContext.Discussions
            .AsNoTracking()
            .Where(d => d.Id == request.Id && d.IsDeleted == false)
            .Select(d => new DiscussionResponse
            {
                Id = d.Id,
                Name = d.Name
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (discussion == null)
        {
            return Result.Failure<DiscussionResponse>(DiscussionErrors.NotFound);
        }

        return Result.Success(discussion);
    }
}
