using Application.Abstractions.Messaging;
using Domain.Roles;
using SharedKernel;

namespace Application.Roles.CreateRole;

internal sealed class CreateRoleCommandHandler(IRoleRepository roleRepository) : ICommandHandler<CreateRoleCommand, Guid>
{
    private readonly IRoleRepository roleRepository = roleRepository;

    public async Task<Result<Guid>> Handle(CreateRoleCommand request, CancellationToken cancellationToken)
    {
        // TODO: Add check for discussion exists

        if (await roleRepository.DuplicateRoleNamesInDiscussionAsync(request.Name, request.DiscussionId, cancellationToken))
        {
            return Result.Failure<Guid>(RoleErrors.DuplicateRoleNamesInDiscussion);
        }

        List<Permission> permissions = [];

        foreach (string permissionString in request.Permissions)
        {
            Result<Permission> permissionResult = Permission.Create(permissionString);

            if (permissionResult.IsFailure)
            {
                return Result.Failure<Guid>(permissionResult.Error);
            }

            Permission permission = permissionResult.Value;

            permissions.Add(permission);
        }

        Result<Role> roleResult = Role.Create(
            request.DiscussionId,
            request.Name,
            permissions);

        if (roleResult.IsFailure)
        {
            return Result.Failure<Guid>(roleResult.Error);
        }

        Role role = roleResult.Value;

        roleRepository.Insert(role);

        return Result.Success(role.Id);
    }
}
