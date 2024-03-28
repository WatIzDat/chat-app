using Application.Abstractions.Messaging;
using Domain.Users;
using SharedKernel;

namespace Application.Users.RegisterUser;

internal sealed class RegisterUserCommandHandler(
    IUserRepository userRepository,
    IDateTimeOffsetProvider dateTimeOffsetProvider)
    : ICommandHandler<RegisterUserCommand, Guid>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IDateTimeOffsetProvider dateTimeOffsetProvider = dateTimeOffsetProvider;

    public async Task<Result<Guid>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        Result<Email> emailResult = Email.Create(request.Email);

        if (emailResult.IsFailure)
        {
            return Result.Failure<Guid>(emailResult.Error);
        }

        Email email = emailResult.Value;

        if (!await userRepository.IsEmailUniqueAsync(email, cancellationToken))
        {
            return Result.Failure<Guid>(UserErrors.EmailNotUnique);
        }


        Result<AboutSection> aboutSectionResult = AboutSection.Create("");

        if (aboutSectionResult.IsFailure)
        {
            return Result.Failure<Guid>(aboutSectionResult.Error);
        }

        AboutSection aboutSection = aboutSectionResult.Value;


        Result<DiscussionsList> discussionsResult = DiscussionsList.Create([]);

        if (discussionsResult.IsFailure)
        {
            return Result.Failure<Guid>(discussionsResult.Error);
        }

        DiscussionsList discussions = discussionsResult.Value;


        Result<RolesList> rolesResult = RolesList.Create([]);

        if (rolesResult.IsFailure)
        {
            return Result.Failure<Guid>(rolesResult.Error);
        }

        RolesList roles = rolesResult.Value;


        // TODO: Check username is unique

        Result<User> userResult = User.Create(
            request.Username,
            email,
            dateTimeOffsetProvider.UtcNow,
            aboutSection,
            discussions,
            roles);

        if (userResult.IsFailure)
        {
            return Result.Failure<Guid>(userResult.Error);
        }

        User user = userResult.Value;

        userRepository.Insert(user);

        return Result.Success(user.Id);
    }
}
