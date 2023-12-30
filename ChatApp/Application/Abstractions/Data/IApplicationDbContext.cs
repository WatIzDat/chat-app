using Domain.Bans;
using Domain.Discussions;
using Domain.Messages;
using Domain.Roles;
using Domain.Users;
using Microsoft.EntityFrameworkCore;

namespace Application.Abstractions.Data;

public interface IApplicationDbContext
{
    DbSet<Ban> Bans { get; set; }

    DbSet<Discussion> Discussions { get; set; }

    DbSet<Message> Messages { get; set; }

    DbSet<Role> Roles { get; set; }

    DbSet<User> Users { get; set; }
}
