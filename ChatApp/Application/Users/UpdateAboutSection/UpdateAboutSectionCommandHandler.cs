using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.UpdateAboutSection;

internal sealed class UpdateAboutSectionCommandHandler(IUserRepository userRepository)
    : ICommandHandler<UpdateAboutSectionCommand>
{
    private readonly IUserRepository userRepository = userRepository;

    public async Task<Result> Handle(UpdateAboutSectionCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        Result<AboutSection> aboutSectionResult = AboutSection.Create(request.AboutSection);

        if (aboutSectionResult.IsFailure)
        {
            return Result.Failure(aboutSectionResult.Error);
        }

        AboutSection aboutSection = aboutSectionResult.Value;

        user.UpdateAboutSection(aboutSection);

        userRepository.Update(user);

        return Result.Success();
    }
}
