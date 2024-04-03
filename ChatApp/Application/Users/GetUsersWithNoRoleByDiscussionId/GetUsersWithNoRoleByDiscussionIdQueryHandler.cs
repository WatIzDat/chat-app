using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetUsersWithNoRoleByDiscussionId;

internal sealed class GetUsersWithNoRoleByDiscussionIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUsersWithNoRoleByDiscussionIdQuery, List<UserResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<UserResponse>>> Handle(GetUsersWithNoRoleByDiscussionIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> usersInDiscussion = dbContext.Users
            .FromSql(
                $"""
                SELECT *
                FROM "user"
                WHERE {request.DiscussionId} = ANY("user".discussions) AND NOT EXISTS(
                   	SELECT 1
                   	FROM "role"
                   	WHERE "role".id = ANY("user".roles)
                	AND "role".discussion_id = {request.DiscussionId}
                )
                """);

        IQueryable<UserResponse> mappedUsers = usersInDiscussion
            .Select(u => new UserResponse
            {
                Id = u.IsDeleted ? Guid.Empty : u.Id,
                Username = u.IsDeleted ? "Deleted User" : u.Username,
                Email = u.IsDeleted ? "" : u.Email.Value,
                DateCreatedUtc = u.IsDeleted ? DateTimeOffset.MinValue : u.DateCreatedUtc,
                AboutSection = u.IsDeleted ? "" : u.AboutSection.Value
            });

        // TODO: implement pagination later

        //IQueryable<UserResponse> paginatedUsers = mappedUsers
        //    .OrderBy(u => u.DateCreatedUtc)
        //    .ThenBy(u => u.Id)
        //    .Where(u => u.DateCreatedUtc > request.LastDateCreatedUtc
        //            || (u.DateCreatedUtc == request.LastDateCreatedUtc && u.Id > request.LastUserId))
        //    .Take(request.Limit);

        List<UserResponse> finalUsersResult = await mappedUsers.ToListAsync(cancellationToken);

        return Result.Success(finalUsersResult);
    }
}
