using System.Data;
using KingsFarms.Core.Api.Domain;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public interface IApplicationContext
{
    public IQueryable<Domain.Bed> Beds { get; }

    public DataTable BedsTable { get; set; }
}