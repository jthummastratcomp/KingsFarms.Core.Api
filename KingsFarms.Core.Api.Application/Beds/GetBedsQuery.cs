using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;

namespace KingsFarms.Core.Api.Application.Beds
{
    public record GetBedsQuery : IRequest<BedsVm>;

    public class GetBedsQueryHandler : IRequestHandler<GetBedsQuery, BedsVm>
    {
        public async Task<BedsVm> Handle(GetBedsQuery request, CancellationToken cancellationToken)
        {
            return new BedsVm(){Name = "bed 1"};
        }
    }
}
