using Autofac;
using Autofac.Extensions.DependencyInjection;
using KingsFarms.Core.Api.UI;

var builder = WebApplication.CreateBuilder(args);

var apiSettings = builder.Configuration.GetSection(nameof(AppSettings)).Get<AppSettings>();

// Add services to the container.
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new ConfigureUIServices(apiSettings)));


builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();