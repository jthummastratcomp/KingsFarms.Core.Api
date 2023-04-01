using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Data.Repositories;

public class CustomerRepository : Repository<Customer>
{
    public CustomerRepository(KingsFarmsDbContext context) : base(context)
    {
    }

    public override Customer Update(Customer entity)
    {
        var customer = Get(entity.Id);
        if (customer == null) return entity;

        customer.City = entity.City;

        return base.Update(customer);
    }
}