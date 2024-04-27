using Application.Abstractions.Messaging;
using Domain.Users;

namespace Application.Users.GetUserById;

public sealed record GetUserByClerkIdQuery(string ClerkId) : IQuery<UserResponse>;
