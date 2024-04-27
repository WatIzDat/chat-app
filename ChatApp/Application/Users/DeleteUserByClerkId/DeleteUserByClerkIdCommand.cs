using Application.Abstractions.Messaging;

namespace Application.Users.DeleteUserByClerkId;

public sealed record DeleteUserByClerkIdCommand(string ClerkId) : ICommand;
