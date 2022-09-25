using MatchmakingService.DataAccess.Redis.Extensions;
using MatchmakingService.Domain.Extensions;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Internal;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.TryAddSingleton<ISystemClock, SystemClock>();

builder.Services
    .AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies())
    .AddRedisDataAccess(builder.Configuration.GetSection("Redis"))
    .AddDomainAccess(builder.Configuration);

var app = builder.Build();

// TODO Disable for prod
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();