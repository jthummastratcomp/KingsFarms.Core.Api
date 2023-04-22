using System.Data;
using System.Reflection.Metadata;
using KingsFarms.Core.Api.Domain;

namespace KingsFarms.Core.Api.Application.Common.Interfaces;

public class ApplicationContext : IApplicationContext
{
    public IQueryable<Domain.Bed> Beds => BedsTable.Rows.Cast<Domain.Bed>().AsQueryable();

    public DataTable BedsTable { get; set; }


}

