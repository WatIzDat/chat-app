using Application.Bans.BanUserPermanently;
using Application.Bans.BanUserTemporarily;
using Application.Bans.UnbanUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;

namespace Web.Api.Controllers;

[Route("bans")]
public class BansController(ISender sender) : ApiController(sender)
{
    [HttpPost("ban-user-permanently")]
    public async Task<IResult> BanUserPermanently(
        [FromBody] BanUserPermanentlyCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost("ban-user-temporarily")]
    public async Task<IResult> BanUserTemporarily(
        [FromBody] BanUserTemporarilyCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }

    [HttpPost("unban-user")]
    public async Task<IResult> UnbanUser(
        [FromBody] UnbanUserCommand command,
        CancellationToken cancellationToken)
    {
        Result result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok() : result.ToProblemDetails();
    }
}
