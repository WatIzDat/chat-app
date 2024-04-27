using Application.Abstractions.Messaging;
using Domain.Users;

namespace Application.Users.GetUserByClerkId;

public sealed record GetUserByClerkIdQuery(string ClerkId) : IQuery<UserResponse>;
