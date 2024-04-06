using Application.Bans.UnbanUser;
using Domain.Bans;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Bans;

public class UnbanUserCommandTests
{
    private static readonly UnbanUserCommand Command = new(Guid.NewGuid());

    private static readonly Ban Ban = Ban.Create(
        Command.UserId,
        Guid.NewGuid(),
        DateTimeOffset.UtcNow,
        BanDetails.CreateTemporaryBan(DateTimeOffset.UtcNow, DateTime.UtcNow.AddDays(1)).Value);

    private readonly UnbanUserCommandHandler commandHandler;

    private readonly IBanRepository banRepositoryMock;

    public UnbanUserCommandTests()
    {
        banRepositoryMock = Substitute.For<IBanRepository>();

        commandHandler = new UnbanUserCommandHandler(banRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        banRepositoryMock.GetByUserIdAsync(Arg.Is(Command.UserId)).Returns(Ban);

        Result result = await commandHandler.Handle(Command, default);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnBanNotFoundByUserId_WhenGetByUserIdAsyncReturnsNull()
    {
        banRepositoryMock.GetByUserIdAsync(Arg.Is(Command.UserId)).ReturnsNull();

        Result result = await commandHandler.Handle(Command, default);

        result.Error.Should().Be(BanErrors.NotFoundByUserId);
    }
}
