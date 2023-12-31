using Application.Abstractions.Messaging;
using Domain.Roles;
using Domain.Users;
using SharedKernel;

namespace Application.Users.AddRole;

internal sealed class AddRoleCommandHandler(
    IUserRepository userRepository,
    IRoleRepository roleRepository)
    : ICommandHandler<AddRoleCommand>
{
    private readonly IUserRepository userRepository = userRepository;
    private readonly IRoleRepository roleRepository = roleRepository;

    public async Task<Result> Handle(AddRoleCommand request, CancellationToken cancellationToken)
    {
        User? user = await userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user == null)
        {
            return Result.Failure(UserErrors.NotFound);
        }

        if (!await roleRepository.RoleExistsAsync(request.RoleId, cancellationToken))
        {
            return Result.Failure(RoleErrors.NotFound);
        }

        Result result = user.AddRole(request.RoleId);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        userRepository.Update(user);

        return Result.Success();
    }
}
