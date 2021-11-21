using Autofac;
using Autofac.Extensions.DependencyInjection;
using Microsoft.VisualBasic.CompilerServices;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule<WebBootstrap>());

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.Run();


//public class WebBootstrap : Module
//{
//    protected override void Load(ContainerBuilder builder)
//    {
//        builder.RegisterType<WeeklyOrdersService>().As<IWeeklyOrdersService>();
//    }
//}

//public interface IWeeklyOrdersService
//{
//    List<SearchDto> GetInvoiceWeeksListForYear();
//}

//public class WeeklyOrdersService : IWeeklyOrdersService
//{
//    public List<SearchDto> GetInvoiceWeeksListForYear()
//    {
//        return new List<SearchDto>(){ new SearchDto(){Data = "hello", DataType = "Hello Dt", Id = "123"}}; //Utils.GetWeeksOfYear(DateTime.Today.Year);
//    }
//}

//public class SearchDto
//{
//    public string Id { get; set; }
//    public string Data { get; set; }
//    //public string SortOrderIndex { get; set; }
//    public string DataType { get; set; }
//}