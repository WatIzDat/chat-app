using Application.Discussions.EditName;
using Domain.Discussions;
using NSubstitute.ReturnsExtensions;
using SharedKernel;

namespace Application.UnitTests.Discussions;

public class EditNameCommandTests
{
    private static readonly Discussion Discussion = Discussion.Create(
        Guid.NewGuid(), "Test", DateTimeOffset.UtcNow).Value;

    private readonly EditNameCommandHandler commandHandler;
    private readonly IDiscussionRepository discussionRepositoryMock;

    public EditNameCommandTests()
    {
        discussionRepositoryMock = Substitute.For<IDiscussionRepository>();

        commandHandler = new EditNameCommandHandler(discussionRepositoryMock);
    }

    [Fact]
    public async Task Handle_Should_ReturnSuccess()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        EditNameCommand command = new(discussion.Id, "hello");

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).Returns(discussion);

        Result result = await commandHandler.Handle(command, default);

        result.IsSuccess.Should().BeTrue();

        discussion.Name.Should().Be("hello");
    }

    [Fact]
    public async Task Handle_Should_ReturnDiscussionNotFound_WhenGetByIdAsyncReturnsNull()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        EditNameCommand command = new(discussion.Id, "hello");

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).ReturnsNull();

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(DiscussionErrors.NotFound);
    }

    [Fact]
    public async Task Handle_Should_ReturnNameTooLong_WhenNameIsLongerThanMaxLength()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        EditNameCommand command = new(
            discussion.Id, "thisnameiswaytoolongaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).Returns(discussion);

        Result result = await commandHandler.Handle(command, default);

        result.Error.Should().Be(DiscussionErrors.NameTooLong);
    }

    [Fact]
    public async Task Handle_Should_CallDiscussionRepositoryUpdate()
    {
        Discussion discussion = Discussion.Create(
            Discussion.UserCreatedBy,
            Discussion.Name,
            Discussion.DateCreatedUtc).Value;

        EditNameCommand command = new(discussion.Id, "hello");

        discussionRepositoryMock.GetByIdAsync(Arg.Is(command.DiscussionId)).Returns(discussion);

        Result result = await commandHandler.Handle(command, default);

        discussionRepositoryMock
            .Received(1)
            .Update(Arg.Is<Discussion>(d => d.Id == discussion.Id));
    }
}
