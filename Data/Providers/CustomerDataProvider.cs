using KingsFarms.Core.Api.Data.Domain;
using KingsFarms.Core.Api.Data.Repositories;

namespace KingsFarms.Core.Api.Data.Providers
{
    public class CustomerDataProvider : BaseDataProvider<Customer>, ICustomerDataProvider
    {
        private readonly IRepository<Customer> _customerRepository;

        public CustomerDataProvider(IRepository<Customer> customerRepository, IUnitOfWork unitOfWork) : base(customerRepository, unitOfWork)
        {
            _customerRepository = customerRepository;
        }

        public Customer? GetCustomer(int id)
        {
            return _customerRepository.GetById(id);
        }

        public List<Customer> GetCustomers()
        {
          return  _customerRepository.GetAll().ToList();
        }
    }
}
