﻿using Domain.Bans;
using Domain.Messages;
using Domain.Roles;
using Domain.Users;
using SharedKernel;

namespace Domain.Discussions;

public sealed class Discussion : Entity
{
    public const int NameMaxLength = 50;

    // Private parameterless constructor for EF
    private Discussion()
    {
    }

    private Discussion(
        Guid id,
        Guid userCreatedBy,
        string name,
        DateTimeOffset dateCreatedUtc,
        bool isDeleted)
        : base(id)
    {
        UserCreatedBy = userCreatedBy;
        Name = name;
        DateCreatedUtc = dateCreatedUtc;
        IsDeleted = isDeleted;
    }

    public Guid UserCreatedBy { get; private set; }

    public string Name { get; private set; }

    public DateTimeOffset DateCreatedUtc { get; private set; }

    public bool IsDeleted { get; private set; }

    // Navigation property for EF
    public ICollection<Role> RolesNavigation { get; set; } = null!;

    // Navigation property for EF
    public ICollection<Message> MessagesNavigation { get; set; } = null!;

    // Navigation property for EF
    public User UserCreatedByNavigation { get; set; } = null!;

    // Navigation property for EF
    public ICollection<Ban> BansNavigation { get; set; } = null!;

    public static Result<Discussion> Create(
        Guid userCreatedBy,
        string name,
        DateTimeOffset dateCreatedUtc)
    {
        if (name.Length > NameMaxLength)
        {
            return Result.Failure<Discussion>(DiscussionErrors.NameTooLong);
        }

        Discussion discussion = new(Guid.NewGuid(), userCreatedBy, name, dateCreatedUtc, isDeleted: false);

        return Result.Success(discussion);
    }

    public Result EditName(string name)
    {
        if (name.Length > NameMaxLength)
        {
            return Result.Failure(DiscussionErrors.NameTooLong);
        }

        Name = name;

        return Result.Success();
    }

    public void Delete()
    {
        IsDeleted = true;
    }
}
