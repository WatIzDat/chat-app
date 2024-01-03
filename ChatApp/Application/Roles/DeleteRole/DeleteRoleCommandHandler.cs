using Application.Abstractions.Messaging;
using Domain.Roles;
using SharedKernel;

namespace Application.Roles.DeleteRole;

internal sealed class DeleteRoleCommandHandler(IRoleRepository roleRepository) : ICommandHandler<DeleteRoleCommand>
{
    private readonly IRoleRepository roleRepository = roleRepository;

    public async Task<Result> Handle(DeleteRoleCommand request, CancellationToken cancellationToken)
    {
        Role? role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            return Result.Failure(RoleErrors.NotFound);
        }

        roleRepository.Delete(role);

        return Result.Success();
    }
}

