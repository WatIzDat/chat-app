using Application.Discussions;
using Application.Discussions.CreateDiscussion;
using Application.Discussions.DeleteDiscussion;
using Application.Discussions.EditName;
using Application.Discussions.GetCreatedDiscussionsByUser;
using Application.Discussions.GetDiscussionById;
using Application.Discussions.GetJoinedDiscussionsByUser;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;
using Web.Api.Infrastructure.Filters;

namespace Web.Api.Controllers;

[Route("discussions")]
public sealed class DiscussionsController(ISender sender) : ApiController(sender)
{
    [Authorize]
    [HttpPost("create-discussion")]
    public async Task<IResult> CreateDiscussion(
        [FromBody] CreateDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [Authorize]
    [HttpDelete("delete-discussion")]
    public async Task<IResult> DeleteDiscussion(
        [FromQuery] Guid discussionId,
        CancellationToken cancellationToken)
    {
        DeleteDiscussionCommand command = new(discussionId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("edit-name")]
    public async Task<IResult> EditName(
        [FromBody] EditNameCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [GetUserIdFromAccessToken]
    [HttpGet("get-created-discussions-by-user")]
    public async Task<IResult> GetCreatedDiscussionsByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        GetCreatedDiscussionsByUserQuery query = new(userId);

        Result<List<DiscussionResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [Authorize]
    [HttpGet("get-discussion-by-id")]
    public async Task<IResult> GetDiscussionById(
        [FromQuery] Guid id,
        CancellationToken cancellationToken)
    {
        GetDiscussionByIdQuery query = new(id);

        Result<DiscussionResponse> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [Authorize]
    [GetUserIdFromAccessToken]
    [HttpGet("get-joined-discussions-by-user")]
    public async Task<IResult> GetJoinedDiscussionsByUser(
        Guid userId,
        CancellationToken cancellationToken)
    {
        GetJoinedDiscussionsByUserQuery query = new(userId);

        Result<List<DiscussionResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
