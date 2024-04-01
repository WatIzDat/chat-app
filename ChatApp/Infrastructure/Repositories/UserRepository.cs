using Domain.Users;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories;

public sealed class UserRepository(ApplicationDbContext dbContext) : IUserRepository
{
    private readonly ApplicationDbContext dbContext = dbContext;

    public async Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        User? user = await dbContext.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(u => u.Id == id, cancellationToken);

        List<Guid> discussions = dbContext.Database.SqlQuery<Guid>(
                    $"""
                    SELECT discussion_id
                    FROM user_to_joined_discussion
                    WHERE user_id = {user.Id}
                    """).ToList();

        User finalUser = User.Create(
                user.Username,
                user.Email,
                user.DateCreatedUtc,
                user.AboutSection,
                DiscussionsList.Create(discussions).Value,
                null!
                ).Value;

        return finalUser;

        //return await dbContext.Users
        //    .FromSql(
        //        $"""
        //            SELECT user.id, user.username, user.email,
        //                   user.date_created_utc, user.about_section, user.is_deleted,
        //                   array_agg(discussion_id) AS FROM user JOIN user_to_joined_discussion ON id = user_id
        //            """)
    }

    public void Insert(User user)
    {
        dbContext.Set<User>().Add(user);
    }

    public async Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default)
    {
        bool exists = await dbContext.Users
            .AnyAsync(u => u.Email == email && u.IsDeleted == false, cancellationToken);

        // Unique means not existing, so negation is done
        return !exists;
    }

    public async Task<bool> IsUsernameUniqueAsync(string username, CancellationToken cancellationToken = default)
    {
        bool exists = await dbContext.Users
            .AnyAsync(u => u.Username == username && u.IsDeleted == false, cancellationToken);

        // Unique means not existing, so negation is done
        return !exists;
    }

    public void Update(User user)
    {
        dbContext.Set<User>().Update(user);

        foreach (Guid discussionId in user.Discussions.Value)
        {
            dbContext.Set<User>()
                .FromSql($"INSERT INTO user_to_joined_discussion (discussion_id, user_id) VALUES ('{discussionId}', '{user.Id}') ON CONFLICT DO NOTHING");
        }
    }

    public async Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await dbContext.Users
            .AnyAsync(u => u.Id == id, cancellationToken);
    }
}
