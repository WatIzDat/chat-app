using Application.Users;
using Application.Users.AddRole;
using Application.Users.ChangeEmail;
using Application.Users.ChangeUsername;
using Application.Users.DeleteUser;
using Application.Users.DeleteUserByClerkId;
using Application.Users.GetUserByClerkId;
using Application.Users.GetUsersByDiscussionIdAndRoleId;
using Application.Users.GetUsersWithNoRoleByDiscussionId;
using Application.Users.JoinDiscussion;
using Application.Users.LeaveDiscussion;
using Application.Users.RegisterUser;
using Application.Users.RemoveRole;
using Application.Users.UpdateAboutSection;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;

namespace Web.Api.Controllers;

[Route("users")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost("add-role")]
    public async Task<IResult> AddRole(
        [FromBody] AddRoleCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("change-email")]
    public async Task<IResult> ChangeEmail(
        [FromBody] ChangeEmailCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("change-username")]
    public async Task<IResult> ChangeUsername(
        [FromBody] ChangeUsernameCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpDelete("delete-user")]
    public async Task<IResult> DeleteUser(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        DeleteUserCommand command = new(userId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpDelete("delete-user-by-clerk-id")]
    public async Task<IResult> DeleteUserByClerkId(
        [FromQuery] string clerkId,
        CancellationToken cancellationToken)
    {
        DeleteUserByClerkIdCommand command = new(clerkId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpGet("get-user-by-clerk-id")]
    public async Task<IResult> GetUserByClerkId(
        [FromQuery] string clerkId,
        CancellationToken cancellationToken)
    {
        GetUserByClerkIdQuery query = new(clerkId);

        Result<UserResponse> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet("get-users-by-discussion-id-and-role-id")]
    public async Task<IResult> GetUsersByDiscussionIdAndRoleId(
        [FromQuery] Guid discussionId,
        [FromQuery] Guid roleId,
        [FromQuery] DateTimeOffset lastDateCreatedUtc,
        [FromQuery] Guid lastUserId,
        [FromQuery] int limit,
        CancellationToken cancellationToken)
    {
        GetUsersByDiscussionIdAndRoleIdQuery query = new(
            discussionId,
            roleId,
            lastDateCreatedUtc,
            lastUserId,
            limit);

        Result<List<UserResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpGet("get-users-with-no-role-by-discussion-id")]
    public async Task<IResult> GetUsersWithNoRoleByDiscussionId(
        [FromQuery] Guid discussionId,
        [FromQuery] DateTimeOffset lastDateCreatedUtc,
        [FromQuery] Guid lastUserId,
        [FromQuery] int limit,
        CancellationToken cancellationToken)
    {
        GetUsersWithNoRoleByDiscussionIdQuery query = new(
            discussionId,
            lastDateCreatedUtc,
            lastUserId,
            limit);

        Result<List<UserResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }


    [HttpPost("join-discussion")]
    public async Task<IResult> JoinDiscussion(
        [FromBody] JoinDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("leave-discussion")]
    public async Task<IResult> LeaveDiscussion(
        [FromBody] LeaveDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("register-user")]
    public async Task<IResult> RegisterUser(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost("remove-role")]
    public async Task<IResult> RemoveRole(
        [FromBody] RemoveRoleCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("update-about-section")]
    public async Task<IResult> UpdateAboutSection(
        [FromBody] UpdateAboutSectionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }
}
