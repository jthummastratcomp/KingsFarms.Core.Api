using System.Reflection;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using KingsFarms.Core.Api;
using KingsFarms.Core.Api.Bootstrap;
using KingsFarms.Core.Api.Data.Db;
using KingsFarms.Core.Api.Data.memory;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


var apiSettings = builder.Configuration.GetSection(typeof(KingsFarmsCoreApiSettings).Name).Get<KingsFarmsCoreApiSettings>();

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
//builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule<WebBootstrap>());
builder.Host.ConfigureContainer<ContainerBuilder>(b => b.RegisterModule(new WebBootstrap(apiSettings)));

// Add services to the container.
builder.Services.AddDbContext<KingsFarmsDbContext>(opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("KingsFarmsDb")));
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(x => x.AddDefaultPolicy(policyBuilder => policyBuilder.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod()));

builder.Services.AddLazyCache();


//builder.Services.AddMediatR(typeof(Program));
builder.Services.AddMediatR(cfg => { cfg.RegisterServicesFromAssemblies(Assembly.GetAssembly(typeof(Program))!); });
builder.Services.AddDbContext<ContactsContext>(options => options.UseInMemoryDatabase("Contacts"));


var app = builder.Build();

//don't do thi sis production
//await EnsureDatabaseIsMigrated(app.Services);

//async Task EnsureDatabaseIsMigrated(IServiceProvider appServices)
//{
//    using var scope = appServices.CreateScope();
//    await using var ctx = scope.ServiceProvider.GetService<KingsFarmsDbContext>();
//    if (ctx is not null)
//    {
//        await ctx.Database.MigrateAsync();
//    }
//}

using var scope = app.Services.CreateScope();
using var ctx = scope.ServiceProvider.GetService<ContactsContext>();
ctx?.Database.EnsureCreated();


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