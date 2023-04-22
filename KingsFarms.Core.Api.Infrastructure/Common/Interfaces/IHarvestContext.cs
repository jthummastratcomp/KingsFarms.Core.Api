using KingsFarms.Core.Api.Domain;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public interface IHarvestContext
{
    public List<Domain.Bed> GetBedsInfo();
}