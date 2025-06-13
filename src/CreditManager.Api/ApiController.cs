using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace CreditManager.Api;

public class ApiController : ControllerBase
{
    protected readonly ISender Sender;
    
    protected ApiController(ISender sender) => Sender = sender;
}