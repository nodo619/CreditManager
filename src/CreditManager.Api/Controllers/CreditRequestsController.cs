using CreditManager.Application.Common.Models;
using CreditManager.Application.Feature.CreditRequests.Commands.ApproveCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Commands.CancelCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Commands.CreateCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Commands.RejectCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Commands.UpdateCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Queries;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequest;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequests;
using CreditManager.Application.Feature.CreditRequests.Queries.GetCreditRequestsForCustomer;
using CreditManager.Application.Pagination;
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
    [Authorize]
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

    [HttpGet]
    [Authorize(Policy = "RequireCreditOfficerRole")]
    [ProducesResponseType(typeof(PaginatedList<CreditRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<CreditRequestDto>>> GetCreditRequests(
        [FromQuery] GetCreditRequestsQuery query)
    {
        var result = await Sender.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpGet("ForCustomer")]
    [Authorize]
    [ProducesResponseType(typeof(PaginatedList<CreditRequestDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<PaginatedList<CreditRequestDto>>> GetCreditRequestsForCustomer(
        [FromQuery] GetCreditRequestsForCustomerQuery query)
    {
        var result = await Sender.Send(query);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok(result.Value);
    }

    [HttpPost("{id}/approve")]
    [Authorize(Policy = "RequireCreditOfficerRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> ApproveCreditRequest(Guid id)
    {
        var result = await Sender.Send(new ApproveCreditRequestCommand(id));
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("{id}/reject")]
    [Authorize(Policy = "RequireCreditOfficerRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> RejectCreditRequest(Guid id)
    {
        var result = await Sender.Send(new RejectCreditRequestCommand(id));
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPost("{id}/cancel")]
    [Authorize(Policy = "RequireCreditOfficerRole")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> CancelCreditRequest(Guid id)
    {
        var result = await Sender.Send(new CancelCreditRequestCommand(id));
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }

    [HttpPut("{id}")]
    [Authorize]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<ActionResult> UpdateCreditRequest(Guid id, [FromBody] UpdateCreditRequestCommand command)
    {
        if (id != command.Id)
        {
            return BadRequest("Id mismatch");
        }

        var result = await Sender.Send(command);
        
        if (!result.IsSuccess)
        {
            return BadRequest(result.Error);
        }

        return Ok();
    }
}