using System.Data;
using AutoMapper;
using MediatR;
using AutoMapper.QueryableExtensions;
using KingsFarms.Core.Api.Application.Common.Interfaces;

namespace KingsFarms.Core.Api.Application.Beds;

public record GetBedsQuery : IRequest<BedsVm>;

public class GetBedsQueryHandler : IRequestHandler<GetBedsQuery, BedsVm>
{
    private readonly IApplicationContext _context;
    private readonly IMapper _mapper;
    private readonly IHarvestContext _harvestContext;

    public GetBedsQueryHandler(IApplicationContext context, IMapper mapper, IHarvestContext harvestContext)
    {
        _context = context;
        _mapper = mapper;
        _harvestContext = harvestContext;
    }
    public async Task<BedsVm> Handle(GetBedsQuery request, CancellationToken cancellationToken)
    {
        //return new BedsVm { Week = "bed 1" };
        var list = _harvestContext.GetBedsInfo();
        //return _context.Beds.ProjectTo<BedsVm>(_mapper.ConfigurationProvider).SingleOrDefault();

        return new BedsVm();
    }
}