using KingsFarms.Core.Api.Application.Beds;
using Microsoft.AspNetCore.Mvc;

namespace KingsFarms.Core.Api.UI.Controllers;

public class BedsController : ApiControllerBase
{
    [HttpGet]
    public async Task<ActionResult<BedsVm>> Get()
    {
        return await Mediator.Send(new GetBedsQuery());
    }
}