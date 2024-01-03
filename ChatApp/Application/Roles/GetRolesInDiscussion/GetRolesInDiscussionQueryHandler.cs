using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Roles.GetRolesInDiscussion;

internal sealed class GetRolesInDiscussionQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetRolesInDiscussionQuery, List<RoleResponse>>
{
    public async Task<Result<List<RoleResponse>>> Handle(GetRolesInDiscussionQuery request, CancellationToken cancellationToken)
    {
        List<RoleResponse> roles = await dbContext.Roles
            .Where(r => r.DiscussionId == request.DiscussionId)
            .Select(r => new RoleResponse
            {
                Id = r.Id,
                Name = r.Name,
                Permissions = r.Permissions.Select(p => p.Value).ToList()
            })
            .ToListAsync(cancellationToken);

        return Result.Success(roles);
    }
}

