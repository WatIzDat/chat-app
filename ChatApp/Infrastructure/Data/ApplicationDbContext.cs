using MediatR;
using Microsoft.EntityFrameworkCore;
using Application.Abstractions.Data;
using Domain.Bans;
using Domain.Discussions;
using Domain.Messages;
using Domain.Roles;
using Domain.Users;
using SharedKernel;

namespace Infrastructure.Data;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IPublisher publisher)
    : DbContext(options), IApplicationDbContext
{
    private readonly IPublisher publisher = publisher;

    public DbSet<Ban> Bans { get; set; }

    public DbSet<Discussion> Discussions { get; set; }

    public DbSet<Message> Messages { get; set; }

    public DbSet<Role> Roles { get; set; }

    public DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        int result = await base.SaveChangesAsync(cancellationToken);

        await PublishDomainEventsAsync();

        return result;
    }

    private async Task PublishDomainEventsAsync()
    {
        List<IDomainEvent> domainEvents = ChangeTracker
            .Entries<Entity>()
            .Select(e => e.Entity)
            .SelectMany(e => 
            {
                List<IDomainEvent> domainEvents = e.DomainEvents;

                e.ClearDomainEvents();

                return domainEvents;
            })
            .ToList();

        foreach (IDomainEvent domainEvent in domainEvents)
        {
            await publisher.Publish(domainEvent);
        }
    }
}
