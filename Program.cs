using System.IO.Abstractions;
using dotenv.net;
using Limbus_wordle_backend.Services;
using Limbus_wordle_backend.Services.BackgroundService;
using Limbus_wordle_backend.Services.WebScrapperServices;
using Limbus_wordle_backend.Util.Environment;

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
            policy.WithOrigins(EnvironmentVariables.frontendUrl);
        });
});

builder.WebHost.UseUrls(EnvironmentVariables.listenOn);

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();


app.Run();
