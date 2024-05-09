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
using Clerk.Net.Client;
using Clerk.Net.Client.Users.Item;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;
using Web.Api.Mappings;
using Web.Api.Utility;

namespace Web.Api.Controllers;

[Route("users")]
public sealed class UsersController(ISender sender, ClerkApiClient clerkApiClient) : ApiController(sender)
{
    private readonly ClerkApiClient clerkApiClient = clerkApiClient;

    [Authorize]
    [HttpPost("add-role")]
    public async Task<IResult> AddRole(
        [FromBody] AddRoleCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("change-email")]
    public async Task<IResult> ChangeEmail(
        [FromBody] ChangeEmailCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("change-username")]
    public async Task<IResult> ChangeUsername(
        [FromBody] ChangeUsernameCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpDelete("delete-user")]
    public async Task<IResult> DeleteUser(
        [FromQuery] Guid userId,
        CancellationToken cancellationToken)
    {
        DeleteUserCommand command = new(userId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpDelete("delete-user-by-clerk-id")]
    public async Task<IResult> DeleteUserByClerkId(
        [FromQuery] string clerkId,
        CancellationToken cancellationToken)
    {
        DeleteUserByClerkIdCommand command = new(clerkId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("/webhook/delete-user")]
    public async Task<IResult> DeleteUserWebhook(CancellationToken cancellationToken)
    {
        return await WebhookUtility.VerifyWebhookAsync(
            HttpContext,
            Request,
            "WebhookSecrets:DeleteUserSecret",
            onSuccess: async body =>
            {
                DeleteUserWebhookMapping user = JsonConvert.DeserializeObject<DeleteUserWebhookMapping>(body)!;

                DeleteUserByClerkIdCommand command = new(user.Data.Id);

                Result result = await Sender.Send(command, cancellationToken);

                return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
            },
            cancellationToken);
    }

    [Authorize]
    [HttpGet("get-user-by-clerk-id")]
    public async Task<IResult> GetUserByClerkId(
        [FromQuery] string clerkId,
        CancellationToken cancellationToken)
    {
        GetUserByClerkIdQuery query = new(clerkId);

        Result<UserResponse> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [Authorize]
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

    [Authorize]
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

    [Authorize]
    [HttpPost("join-discussion")]
    public async Task<IResult> JoinDiscussion(
        [FromBody] JoinDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("leave-discussion")]
    public async Task<IResult> LeaveDiscussion(
        [FromBody] LeaveDiscussionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("register-user")]
    public async Task<IResult> RegisterUser(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost("/webhook/register-user")]
    public async Task<IResult> RegisterUserWebhook(CancellationToken cancellationToken)
    {
        return await WebhookUtility.VerifyWebhookAsync(
            HttpContext,
            Request,
            "WebhookSecrets:RegisterUserSecret",
            onSuccess: async body =>
            {
                RegisterUserWebhookMapping user = JsonConvert.DeserializeObject<RegisterUserWebhookMapping>(body)!;

                RegisterUserCommand command = new(
                    user.Data.Username,
                    user.Data.EmailAddresses.First(e => e.Id == user.Data.PrimaryEmailAddressId).EmailAddress,
                    user.Data.Id);

                Result<Guid> result = await Sender.Send(command, cancellationToken);

                if (result.IsFailure)
                {
                    return result.ToProblemDetails();
                }

                WithUser_PatchRequestBody patchRequestBody = new()
                {
                    ExternalId = result.Value.ToString()
                };

                await clerkApiClient.Users[user.Data.Id].PatchAsync(patchRequestBody, cancellationToken: cancellationToken);

                return Results.Ok(result.Value);
            },
            cancellationToken);
    }

    [Authorize]
    [HttpPost("remove-role")]
    public async Task<IResult> RemoveRole(
        [FromBody] RemoveRoleCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [Authorize]
    [HttpPost("update-about-section")]
    public async Task<IResult> UpdateAboutSection(
        [FromBody] UpdateAboutSectionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }
}
