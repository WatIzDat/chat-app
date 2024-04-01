using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;
using System.Collections.ObjectModel;
using System.Linq.Expressions;

namespace Application.Users.GetUsersWithNoRoleByDiscussionId;

internal sealed class GetUsersWithNoRoleByDiscussionIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUsersWithNoRoleByDiscussionIdQuery, List<UserResponse>>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<List<UserResponse>>> Handle(GetUsersWithNoRoleByDiscussionIdQuery request, CancellationToken cancellationToken)
    {
        IQueryable<User> usersInDiscussion = dbContext.Users
            .Where(u => u.Discussions.Value.Any(d => request.DiscussionId == d));

        IQueryable<User> usersInDiscussionWithNoRole = usersInDiscussion
            .Where(u => u.Roles.Value.All(id => dbContext.Roles.Where(r => r.Id == id).Any(r => r.DiscussionId != request.DiscussionId)));

        IQueryable<UserResponse> mappedUsers = usersInDiscussionWithNoRole
            .Select(u => new UserResponse
            {
                Id = u.IsDeleted ? Guid.Empty : u.Id,
                Username = u.IsDeleted ? "Deleted User" : u.Username,
                Email = u.IsDeleted ? "" : u.Email.Value,
                DateCreatedUtc = u.IsDeleted ? DateTimeOffset.MinValue : u.DateCreatedUtc,
                AboutSection = u.IsDeleted ? "" : u.AboutSection.Value
            });

        IQueryable<UserResponse> paginatedUsers = mappedUsers
            .OrderBy(u => u.DateCreatedUtc)
            .ThenBy(u => u.Id)
            .Where(u => u.DateCreatedUtc > request.LastDateCreatedUtc
                    || (u.DateCreatedUtc == request.LastDateCreatedUtc && u.Id > request.LastUserId))
            .Take(request.Limit);

        List<UserResponse> finalUsersResult = await paginatedUsers.ToListAsync(cancellationToken);

        //Console.WriteLine("test");

        //List<UserResponse> finalUsersResult = await dbContext.Users
        //    .Where(u => u.Discussions.Value.Any(d => d == request.DiscussionId))
        //    .Where(u => u.Roles.Value.All(id => dbContext.Roles.Where(r => r.Id == id).Any(r => r.DiscussionId != request.DiscussionId)))
        //    .Select(u => new UserResponse
        //    {
        //        Id = u.IsDeleted ? Guid.Empty : u.Id,
        //        Username = u.IsDeleted ? "Deleted User" : u.Username,
        //        Email = u.IsDeleted ? "" : u.Email.Value,
        //        DateCreatedUtc = u.IsDeleted ? DateTimeOffset.MinValue : u.DateCreatedUtc,
        //        AboutSection = u.IsDeleted ? "" : u.AboutSection.Value
        //    })
        //    .OrderBy(u => u.DateCreatedUtc)
        //    .ThenBy(u => u.Id)
        //    .Where(u => u.DateCreatedUtc > request.LastDateCreatedUtc
        //            || (u.DateCreatedUtc == request.LastDateCreatedUtc && u.Id > request.LastUserId))
        //    .Take(request.Limit)
        //    .ToListAsync(cancellationToken);

        return Result.Success(finalUsersResult);
    }

    public static IQueryable<T> WhereAnyMatch<T, V>(IQueryable<T> source, IEnumerable<V> values, Expression<Func<T, V, bool>> match)
    {
        var parameter = match.Parameters[0];
        var body = values
            // the easiest way to let EF Core use parameter in the SQL query rather than literal value
            .Select(value => ((Expression<Func<V>>)(() => value)).Body)
            .Select(value => Expression.Invoke(match, parameter, value))
            .Aggregate<Expression>(Expression.OrElse);
        var predicate = Expression.Lambda<Func<T, bool>>(body, parameter);
        return source.Where(predicate);
    }
}
