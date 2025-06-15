using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Queries;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize]
    public async Task<ActionResult<Guid>> CreateCreditRequest(
        [FromBody] CreateCreditRequestCommand command)
    {
        var id = await Sender.Send(command);
        return Ok(id);
    }

    [HttpGet("/{id}")]
    [ProducesResponseType(typeof(CreditRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreditRequestDto>> GetCreditRequest(Guid id)
    {
        try
        {
            var creditRequest = await Sender.Send(new GetCreditRequestQuery(id));
            return Ok(creditRequest);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpGet("ForCustomer")]
    [ProducesResponseType(typeof(IEnumerable<CreditRequestDto>), StatusCodes.Status200OK)]
    public async Task<ActionResult<IEnumerable<CreditRequestDto>>> GetCreditRequestsForCustomer()
    {
        var creditRequests = await Sender.Send(new GetCreditRequestsForCustomerQuery());
        return Ok(creditRequests);
    }
}