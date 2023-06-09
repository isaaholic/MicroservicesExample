using Microsoft.EntityFrameworkCore;
using PlatformService.Data;
using PlatformService.Data.Concretes;
using PlatformService.Data.Interfaces;
using PlatformService.SyncDataServices.Http;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

Console.WriteLine("---> Using SqlServer Db");
builder.Services.AddDbContext<AppDbContext>(
    option => option.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
    );

if (builder.Environment.IsProduction())
{
    Console.WriteLine("---> Using SqlServer Db");
    builder.Services.AddDbContext<AppDbContext>(
        option => option.UseSqlServer(builder.Configuration.GetConnectionString("PlatformsConn"))
        );
}
else
{
    Console.WriteLine("---> Using InMem Db");
    builder.Services.AddDbContext<AppDbContext>(options => options.UseInMemoryDatabase("InMem"));
}


builder.Services.AddScoped<IPlatformRepository, PlatformRepository>();

builder.Services.AddHttpClient<ICommandDataClient, HttpCommandDataClient>();

var app = builder.Build();

PrepDb.PrepPopulation(app, app.Environment.IsProduction());

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
