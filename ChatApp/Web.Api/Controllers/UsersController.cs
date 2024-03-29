using Application.Users.RegisterUser;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using SharedKernel;
using Web.Api.Abstractions;
using Web.Api.Extensions;

namespace Web.Api.Controllers;

[Route("[controller]")]
public sealed class UsersController(ISender sender) : ApiController(sender)
{
    [HttpPost]
    public async Task<IResult> RegisterUser(
        [FromBody] RegisterUserCommand command,
        CancellationToken cancellationToken)
    {
        Result<Guid> result = await Sender.Send(command, cancellationToken);

        return result.IsSuccess ? Results.Ok(result.Value) : result.ToProblemDetails();
    }
}
