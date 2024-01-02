using Application.Abstractions.Messaging;
using Domain.Roles;
using SharedKernel;

namespace Application.Roles.EditName;

internal sealed class EditNameCommandHandler(IRoleRepository roleRepository) : ICommandHandler<EditNameCommand>
{
    private readonly IRoleRepository roleRepository = roleRepository;

    public async Task<Result> Handle(EditNameCommand request, CancellationToken cancellationToken)
    {
        Role? role = await roleRepository.GetByIdAsync(request.RoleId, cancellationToken);

        if (role == null)
        {
            return Result.Failure(RoleErrors.NotFound);
        }

        if (await roleRepository.DuplicateRoleNamesInDiscussionAsync(request.Name, role.DiscussionId, cancellationToken))
        {
            return Result.Failure(RoleErrors.DuplicateRoleNamesInDiscussion);
        }

        Result result = role.EditName(request.Name);

        if (result.IsFailure)
        {
            return Result.Failure(result.Error);
        }

        roleRepository.Update(role);

        return Result.Success();
    }
}
