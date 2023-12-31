using Application.Abstractions.Messaging;

namespace Application.Users.UpdateAboutSection;

public sealed record UpdateAboutSectionCommand(Guid UserId, string AboutSection) : ICommand;
