using System.IO.Abstractions;
using dotenv.net;
using Limbus_wordle_backend.Hubs;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Services.BackgroundService;
using Limbus_wordle_backend.Services.WebScrapperServices;
using Limbus_wordle_backend.Util.Environment;
using Microsoft.AspNetCore.SignalR;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<DailyIdentityFileService>(); 
builder.Services.AddTransient<IdentityFileService>();
builder.Services.AddTransient<ScrapeIdentitiesService>();
builder.Services.AddHostedService<BackgroundScrapeData>();
builder.Services.AddHostedService<BackgroundResetDailyIdentityMode>(); 
builder.Services.AddTransient<IFileSystem,FileSystem>();
builder.Services.AddDataProtection();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
    policy  =>
        {
            policy.WithOrigins(EnvironmentVariables.frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowCredentials();
        });
});

builder.Services.AddSignalR(options =>
{
    options.AddFilter<GameAuthHubFilter>();
});

builder.WebHost.UseUrls(EnvironmentVariables.listenOn);

builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseDefaultFiles();

app.UseStaticFiles();

app.UseAuthorization();

app.MapControllers();

app.UseExceptionHandler();


app.Run();
