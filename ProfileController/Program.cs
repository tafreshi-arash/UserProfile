using Microsoft.EntityFrameworkCore;
using Persistence;
using StackExchange.Redis;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Chaching
builder.Services.AddSingleton<IConnectionMultiplexer>(sp =>
{
    var configuration = builder.Configuration.GetValue<string>("RedisConnection") ?? "localhost:6379";
    return ConnectionMultiplexer.Connect(configuration);
});

builder.Services.AddScoped<ICacheService, RedisCacheService>();

// Application
builder.Services.AddScoped<IUserProfileManagement, UserProfileManagement>();

// DataBase
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("SqlDbConnection")));

builder.Services.AddScoped<IUserRepository, UserRepository>();




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
