using Autofac;
using Autofac.Extensions.DependencyInjection;
using HotTowel.Core.Api;
using HotTowel.Core.Api.Bootstrap;

var builder = WebApplication.CreateBuilder(args);


var apiSettings = builder.Configuration.GetSection(typeof(HotTowelCoreApiSettings).Name).Get<HotTowelCoreApiSettings>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule<WebBootstrap>());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new WebBootstrap(apiSettings)));

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