namespace Domain.Users;

public interface IUserRepository
{
    void Insert(User user);

    void Update(User user);

    Task<User?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);

    Task<bool> UserExistsAsync(Guid id, CancellationToken cancellationToken = default);
}
