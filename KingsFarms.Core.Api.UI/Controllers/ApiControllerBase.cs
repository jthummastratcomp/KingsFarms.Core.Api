using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.UI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ApiControllerBase : ControllerBase
{
    private ISender? _mediator;

    protected ISender Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<ISender>();
}