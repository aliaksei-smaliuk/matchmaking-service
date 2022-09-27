using MatchmakingService.DataAccess.Kafka.Extensions;
using MatchmakingTestClient.BackgroundWorkers;
using MatchmakingTestClient.Configurations;
using MatchmakingTestClient.Hubs;
using MatchmakingTestClient.MessageProcessors;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllersWithViews();
builder.Services.AddSignalR();
builder.Services.AddHostedService<TestSignalRWorker>();
builder.Services.AddCors();
builder.Services.AddKafkaDataAccess(builder.Configuration.GetSection("Kafka"));
builder.Services.AddSingleton<ITimeoutPlayerMessageProcessor, TimeoutPlayerMessageProcessor>();
builder.Services.AddSingleton<IRoomCompletedMessageProcessor, RoomCompletedMessageProcessor>();
builder.Services.Configure<ClientOptions>(builder.Configuration.GetSection("Client"));

var app = builder.Build();

app.UseCors(x => x
    .AllowAnyMethod()
    .AllowAnyHeader()
    .SetIsOriginAllowed(origin => true) // allow any origin
    //.WithOrigins("https://localhost:44351")); // Allow only this origin can also have multiple origins separated with comma
    .AllowCredentials());

// TODO Disable for prod
// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
app.UseHsts();

app.UseStaticFiles();
app.UseRouting();


app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.MapFallbackToFile("index.html");
app.MapHub<MatchmakingHub>("/matchmakingHub");

app.Run();