using System.IO.Abstractions;
using dotenv.net;
using Limbus_wordle.BackgroundTask;
using Limbus_wordle.util.WebScrapper;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<ScrapeIdentities>(); 
builder.Services.AddHostedService<BackgroundScrapeData>();
builder.Services.AddHostedService<BackgroundResetDailyIdentityMode>(); 
builder.Services.AddTransient<IFileSystem,FileSystem>();
builder.Services.AddDataProtection();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
                      policy  =>
                      {
                          policy.WithOrigins(Environment.GetEnvironmentVariable("FRONTEND_URL"));
                      });
});

builder.WebHost.UseUrls(Environment.GetEnvironmentVariable("LISTEN_ON"));

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseAuthorization();

app.MapControllers();

// app.UseWhen(ctx=>ctx.Request.Path.StartsWithSegments("/EndlessIdentityMode"),app=>
// {
//     app.UseDecryptGameMode<EndlessGameMode<Identity>>();
// });

// app.UseWhen(ctx=>ctx.Request.Path.StartsWithSegments("/DailyIdentityMode"),app=>
// {
//     app.UseDecryptGameMode<DailyGameMode<Identity>>();
// });

app.Run();
