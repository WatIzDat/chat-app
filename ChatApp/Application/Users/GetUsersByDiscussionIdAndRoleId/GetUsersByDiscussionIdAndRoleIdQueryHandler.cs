using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Roles;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetUsersByDiscussionIdAndRoleId;

internal sealed class GetUsersByDiscussionIdAndRoleIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUsersByDiscussionIdAndRoleIdQuery, List<UserResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<UserResponse>>> Handle(GetUsersByDiscussionIdAndRoleIdQuery request, CancellationToken cancellationToken)
    {
        Role? role = await dbContext.Roles
            .Where(r => r.Id == request.RoleId)
            .FirstOrDefaultAsync(cancellationToken);

        if (role == null)
        {
            return Result.Failure<List<UserResponse>>(RoleErrors.NotFound);
        }

        if (role.DiscussionId != request.DiscussionId)
        {
            return Result.Failure<List<UserResponse>>(RoleErrors.NotInDiscussion);
        }

        List<UserResponse> users = await dbContext.Users
            .FromSql(
                $"""
                SELECT *
                FROM "user"
                WHERE {request.DiscussionId} = ANY("user".discussions)
                AND {request.RoleId} = ANY("user".roles)
                """)
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
