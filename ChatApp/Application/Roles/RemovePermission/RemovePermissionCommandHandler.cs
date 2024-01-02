using Application.Abstractions.Messaging;
using Domain.Roles;
using SharedKernel;

namespace Application.Roles.RemovePermission;

internal sealed class RemovePermissionCommandHandler(IRoleRepository roleRepository) : ICommandHandler<RemovePermissionCommand>
{
    private readonly IRoleRepository roleRepository = roleRepository;

    public async Task<Result> Handle(RemovePermissionCommand request, CancellationToken cancellationToken)
    {
        Role? role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            return Result.Failure(RoleErrors.NotFound);
        }

        Result<Permission> permissionResult = Permission.Create(request.Permission);

        if (permissionResult.IsFailure)
        {
            return Result.Failure(permissionResult.Error);
        }

        Permission permission = permissionResult.Value;

        Result result = role.RemovePermission(permission);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        roleRepository.Update(role);

        return Result.Success();
    }
}

