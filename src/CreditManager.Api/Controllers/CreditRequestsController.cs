using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditManager.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CreditRequestsController : ApiController
{
    public CreditRequestsController(ISender sender) : base(sender)
    {
        
    }

    [HttpPost]
    public async Task<ActionResult<Guid>> CreateCreditRequest(
        [FromBody] CreateCreditRequestCommand command)
    {
        var id = await Sender.Send(command);
        return Ok(id);
    }
}