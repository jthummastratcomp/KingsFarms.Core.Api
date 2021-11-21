using Autofac;
using Autofac.Extensions.DependencyInjection;
using HotTowel.Web.Bootstrap;
using Microsoft.VisualBasic.CompilerServices;

var builder = WebApplication.CreateBuilder(args);



var someSettings = builder.Configuration.GetSection(typeof(SomeSettings).Name).Get<SomeSettings>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule<WebBootstrap>());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule( new WebBootstrap(someSettings) ));

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => x.AddDefaultPolicy(builder => builder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

// Configure the HTTP request pipeline.
//if (app.Environment.IsDevelopment())
//{
    app.UseSwagger();
    app.UseSwaggerUI();
//}

app.UseAuthorization();

app.MapControllers();

app.UseCors();

app.Run();

public class SomeSettings
{
    public string seqLogUrl { get; set; }
    public string logFile { get; set; }
    public string cosmosDbUri { get; set; }
    public string cosmosDbKey { get; set; }
    public string cosmosDbContainer_Beds { get; set; }
    public string cosmosDbDatabaseId { get; set; }
    
    
    public string azStoreConnStr { get; set; }
    public string azStoreContName { get; set; }
    public string weeklyOrdersFile { get; set; }
    public string harvestFile { get; set; }
}