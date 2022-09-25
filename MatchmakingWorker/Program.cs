using MatchmakingWorker.BackgroundWorkers;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHostedService<Worker>();

var app = builder.Build();

// TODO disable on prod
app.UseSwagger();
app.UseSwaggerUI();

app.MapControllers();

app.Run();