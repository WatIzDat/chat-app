using Application.Messages;
using Application.Messages.DeleteMessage;
using Application.Messages.EditContents;
using Application.Messages.GetMessagesInDiscussion;
using Application.Messages.SendMessage;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;
using Web.Api.Infrastructure.Filters;

namespace Web.Api.Controllers;

[Route("messages")]
public class MessagesController(ISender sender) : ApiController(sender)
{
    [HttpDelete("delete-message")]
    public async Task<IResult> DeleteMessage(
         [FromQuery] Guid messageId,
         CancellationToken cancellationToken)
    {
        DeleteMessageCommand command = new(messageId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("edit-contents")]
    public async Task<IResult> EditContents(
        [FromBody] EditContentsCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpGet("get-messages-in-discussion")]
    public async Task<IResult> GetMessagesInDiscussion(
        [FromQuery] Guid discussionId,
        [FromQuery] DateTimeOffset lastDateSentUtc,
        [FromQuery] Guid lastMessageId,
        [FromQuery] int limit,
        CancellationToken cancellationToken)
    {
        GetMessagesInDiscussionQuery query = new(
            discussionId,
            lastDateSentUtc,
            lastMessageId,
            limit);

        Result<List<MessageResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [Authorize]
    [GetUserIdFromAccessToken]
    [HttpPost("send-message")]
    public async Task<IResult> SendMessage(
        [FromBody] SendMessageCommand command,
        CancellationToken cancellationToken)
    {
        if (command.UserId != (Guid)HttpContext.Items[Constants.UserIdKey]!)
        {
            return Results.Forbid();
        }
        
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
