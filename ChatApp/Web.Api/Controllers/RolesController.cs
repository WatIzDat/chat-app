using Application.Roles;
using Application.Roles.AddPermission;
using Application.Roles.CreateRole;
using Application.Roles.DeleteRole;
using Application.Roles.GetRolesInDiscussion;
using Application.Roles.RemovePermission;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;

namespace Web.Api.Controllers;

[Route("roles")]
public class RolesController(ISender sender) : ApiController(sender)
{
    [HttpPost("add-permission")]
    public async Task<IResult> AddPermission(
        [FromBody] AddPermissionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpPost("create-role")]
    public async Task<IResult> CreateRole(
        [FromBody] CreateRoleCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpDelete("delete-role")]
    public async Task<IResult> DeleteRole(
        [FromQuery] Guid roleId,
        CancellationToken cancellationToken)
    {
        DeleteRoleCommand command = new(roleId);

        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }

    [HttpGet("get-roles-in-discussion")]
    public async Task<IResult> GetRolesInDiscussion(
        [FromQuery] Guid discussionId,
        CancellationToken cancellationToken)
    {
        GetRolesInDiscussionQuery query = new(discussionId);

        Result<List<RoleResponse>> result = await Sender.Send(query, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost("remove-permission")]
    public async Task<IResult> RemovePermission(
        [FromBody] RemovePermissionCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }
}
