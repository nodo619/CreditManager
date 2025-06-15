using CreditManager.Application.Common.Models;
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
    [ProducesResponseType(typeof(Guid), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<Guid>> CreateCreditRequest(
        [FromBody] CreateCreditRequestCommand command)
    {
        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("/{id}")]
    [ProducesResponseType(typeof(CreditRequestDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult<CreditRequestDto>> GetCreditRequest(Guid id)
    {
        var result = await Sender.Send(new GetCreditRequestQuery(id));
        
        if (!result.IsSuccess)
        {
            return NotFound(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("ForCustomer")]
    [ProducesResponseType(typeof(IEnumerable<CreditRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<IEnumerable<CreditRequestDto>>> GetCreditRequestsForCustomer()
    {
        var result = await Sender.Send(new GetCreditRequestsForCustomerQuery());
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }
}