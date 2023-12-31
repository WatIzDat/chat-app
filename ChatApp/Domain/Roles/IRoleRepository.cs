namespace Domain.Roles;

public interface IRoleRepository
{
    Task<bool> RoleExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
