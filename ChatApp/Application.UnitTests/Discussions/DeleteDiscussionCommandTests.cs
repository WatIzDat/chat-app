using Application.Discussions.DeleteDiscussion;
using Domain.Discussions;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Discussions;

public class DeleteDiscussionCommandTests
{
    private static readonly Discussion Discussion = Discussion.Create(
        Guid.NewGuid(), "Test", DateTimeOffset.UtcNow).Value;

    private readonly DeleteDiscussionCommandHandler commandHandler;
    private readonly IDiscussionRepository discussionRepositoryMock;

    public DeleteDiscussionCommandTests()
    {
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();

        commandHandler = new DeleteDiscussionCommandHandler(discussionRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        DeleteDiscussionCommand command = new(discussion.Id);

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).Returns(discussion);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        discussion.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        DeleteDiscussionCommand command = new(discussion.Id);

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(DiscussionErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_CallDiscussionRepositoryUpdate()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        DeleteDiscussionCommand command = new(discussion.Id);

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).Returns(discussion);

        Result result = await commandHandler.Handle(command, default);

        discussionRepositoryMock
            .Received(1)
            .Update(Arg.Is<Discussion>(d => d.Id == discussion.Id));
    }
}
