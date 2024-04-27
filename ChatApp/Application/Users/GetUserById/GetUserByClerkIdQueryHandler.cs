using Application.Abstractions.Data;
using Application.Abstractions.Messaging;
using Domain.Users;
using Microsoft.EntityFrameworkCore;
using SharedKernel;

namespace Application.Users.GetUserById;

internal sealed class GetUserByClerkIdQueryHandler(IApplicationDbContext dbContext)
    : IQueryHandler<GetUserByClerkIdQuery, UserResponse>
{
    private readonly IApplicationDbContext dbContext = dbContext;

    public async Task<Result<UserResponse>> Handle(GetUserByClerkIdQuery request, CancellationToken cancellationToken)
    {
        UserResponse? user = await dbContext.Users
            .AsNoTracking()
            .Where(u => u.ClerkId == request.ClerkId && u.IsDeleted == false)
            .Select(u => new UserResponse
            {
                Id = u.Id,
                AboutSection = u.AboutSection.Value,
                DateCreatedUtc = u.DateCreatedUtc,
                Email = u.Email.Value,
                Username = u.Username
            })
            .FirstOrDefaultAsync(cancellationToken);

        if (user == null)
        {
            return Result.Failure<UserResponse>(UserErrors.NotFound);
        }

        return Result.Success(user);
    }
}
