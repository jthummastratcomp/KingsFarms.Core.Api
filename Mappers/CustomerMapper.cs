using AutoMapper;
using KingsFarms.Core.Api.Controllers;
using KingsFarms.Core.Api.Data.Domain;

namespace KingsFarms.Core.Api.Mappers;

public class CustomerMapper : ICustomerMapper
{
    private readonly MapperConfiguration _config;

    public CustomerMapper()
    {
        _config = Map.Initialize();
    }

    public Customer MapCustomerDbViewModelToCustomer(CustomerDbViewModel vm)
    {
        return _config.CreateMapper().Map<CustomerDbViewModel, Customer>(vm);
    }

    public CustomerDbViewModel MapCustomerToCustomerDBViewModel(Customer customer)
    {
        return _config.CreateMapper().Map<Customer, CustomerDbViewModel>(customer);
    }

    public List<CustomerDbViewModel> MapCustomerListToCustomerDbViewModelList(List<Customer> list)
    {
        return _config.CreateMapper().Map<List<Customer>, List<CustomerDbViewModel>>(list);
    }

    private abstract class Map
    {
        public static MapperConfiguration Initialize()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.CreateMap<CustomerDbViewModel, Customer>()
                    .ForMember(d => d.Key, opt => opt.MapFrom(s => s.CustomerKey))
                    .ForMember(d => d.StoreName, opt => opt.MapFrom(s => s.StoreName))
                    .ForMember(d => d.ContactName, opt => opt.MapFrom(s => s.ContactName))
                    .ForMember(d => d.Address, opt => opt.MapFrom(s => s.Street))
                    .ForMember(d => d.City, opt => opt.MapFrom(s => s.City))
                    .ForMember(d => d.State, opt => opt.MapFrom(s => s.State))
                    .ForMember(d => d.Zip, opt => opt.MapFrom(s => s.Zip))
                    .ForMember(d => d.ContactPhone, opt => opt.MapFrom(s => s.Phone))
                    .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                    .ForMember(d => d.Name, opt => opt.Ignore())
                    .ForMember(d => d.StorePhone, opt => opt.Ignore())
                    .ForMember(d => d.OtherPhone, opt => opt.Ignore())
                    //.ForMember(d => d.Invoices, opt => opt.Ignore())
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id))
                    ;

                cfg.CreateMap<Customer, CustomerDbViewModel>()
                    .ForMember(d => d.CustomerKey, opt => opt.MapFrom(s => s.Key))
                    .ForMember(d => d.StoreName, opt => opt.MapFrom(s => s.StoreName))
                    .ForMember(d => d.ContactName, opt => opt.MapFrom(s => s.ContactName))
                    .ForMember(d => d.Street, opt => opt.MapFrom(s => s.Address))
                    .ForMember(d => d.City, opt => opt.MapFrom(s => s.City))
                    .ForMember(d => d.State, opt => opt.MapFrom(s => s.State))
                    .ForMember(d => d.Zip, opt => opt.MapFrom(s => s.Zip))
                    .ForMember(d => d.Phone, opt => opt.MapFrom(s => s.ContactPhone))
                    .ForMember(d => d.Email, opt => opt.MapFrom(s => s.Email))
                    .ForMember(d => d.Id, opt => opt.MapFrom(s => s.Id));
            });
            config.AssertConfigurationIsValid();
            return config;
        }
    }
}