using KingsFarms.Core.Api.Data.memory;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace KingsFarms.Core.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ContactsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ContactsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{Id}")]
    public async Task<Contact> GetContact([FromRoute] Query query)
    {
        return await _mediator.Send(query);
    }

    public class Query : IRequest<Contact>
    {
        public int Id { get; set; }
    }

    public class ContactHandler : IRequestHandler<Query, Contact>
    {
        private readonly ContactsContext _context;

        public ContactHandler(ContactsContext context)
        {
            _context = context;
        }

        public Task<Contact?> Handle(Query request, CancellationToken cancellationToken)
        {
            return _context.Contacts.Where(x => x.Id == request.Id).SingleOrDefaultAsync();
        }
    }
}