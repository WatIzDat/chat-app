namespace Domain.Users;

public interface IUserRepository
{
    void Insert(User user);

    Task<bool> IsEmailUniqueAsync(Email email, CancellationToken cancellationToken = default);
}
