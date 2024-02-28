using API;
using API.Extensions;
using Infrastructure.Db;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ResolveIoC();

var configBuilder = new ConfigurationBuilder();
configBuilder.AddJsonFile("appsettings.json");
var configuration = configBuilder.Build();

builder.Services.AddDbContextPool<RepositoryDbContext>(x =>
{
    var connectionString = configuration.GetConnectionString("DataBase");
    x.UseSqlServer(connectionString, b => b.MigrationsAssembly("Infrastructure"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.MigrateDatabase();

app.Run();
