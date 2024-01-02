using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetUsersWithNoRoleByDiscussionId;

internal sealed class GetUsersWithNoRoleByDiscussionIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUsersWithNoRoleByDiscussionIdQuery, List<UserResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<UserResponse>>> Handle(GetUsersWithNoRoleByDiscussionIdQuery request, CancellationToken cancellationToken)
    {
        List<UserResponse> users = await dbContext.Users
            .Where(u => u.Discussions.Value.Contains(request.DiscussionId))
            .Select(u => new UserResponse
            {
                Id = u.IsDeleted ? Guid.Empty : u.Id,
                Username = u.IsDeleted ? "Deleted User" : u.Username,
                Email = u.IsDeleted ? "" : u.Email.Value,
                DateCreatedUtc = u.IsDeleted ? DateTimeOffset.MinValue : u.DateCreatedUtc,
                AboutSection = u.IsDeleted ? "" : u.AboutSection.Value
            })
            // Pagination
            .OrderBy(u => u.DateCreatedUtc)
            .ThenBy(u => u.Id)
            .Where(u => u.DateCreatedUtc > request.LastDateCreatedUtc
                    || (u.DateCreatedUtc == request.LastDateCreatedUtc && u.Id > request.LastUserId))
            .Take(request.Limit)
            .ToListAsync(cancellationToken);

        return Result.Success(users);
    }
}
