using CreditManager.Identity.Models;
using CreditManager.Identity.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CreditManager.Identity.Controllers;

[ApiController]
[Route("Account")]
public class AccountController : ControllerBase
{
    private readonly IIdentityService _identityService;

    public AccountController(IIdentityService identityService)
    {
        _identityService = identityService;
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<LoginResponseModel>> Login([FromBody]LoginRequestModel request, CancellationToken cancellationToken)
    {
        var result = await _identityService.Authenticate(request, cancellationToken);

        if (result is null)
        {
            return Unauthorized();
        }
        
        return Ok(result);
    }
    
    [AllowAnonymous]
    [HttpPost("Register")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<Guid?>> Register(RegisterRequestModel request, CancellationToken cancellationToken)
    {
        var result = await _identityService.RegisterUser(request, cancellationToken);

        if (result.FailureMessage is not null)
        {
            return BadRequest(result.FailureMessage);
        }
        
        return Ok(result.UserId);
    }
}