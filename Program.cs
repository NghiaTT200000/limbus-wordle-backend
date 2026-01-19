using System.IO.Abstractions;
using dotenv.net;
using Limbus_wordle_backend.Middleware;
using Limbus_wordle_backend.Repositories;
using Limbus_wordle_backend.Services.IdentityFile;
using Limbus_wordle_backend.Services.Background;
using Limbus_wordle_backend.Services.Scraping;
using Limbus_wordle_backend.Util.Environment;

DotEnv.Load();
var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSingleton<IdentityFileService>(); 
builder.Services.AddTransient<ScrapeIdentitiesService>();
builder.Services.AddHostedService<BackgroundScrapeData>();
builder.Services.AddHostedService<BackgroundResetDailyIdentityMode>(); 
builder.Services.AddTransient<IFileSystem,FileSystem>();
builder.Services.AddScoped<IdentityFileRepository>();
builder.Services.AddDataProtection();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: "AllowOrigin",
    policy  =>
        {
            policy.WithOrigins(EnvironmentVariables.frontendUrl)
                .AllowAnyHeader()
                .AllowAnyMethod()
                .AllowAnyOrigin();
        });
});

builder.WebHost.UseUrls(EnvironmentVariables.listenOn);

var app = builder.Build();


app.UseHttpsRedirection();

app.UseRouting();

app.UseCors("AllowOrigin");

app.UseMiddleware<GlobalExceptionHandler>();

app.UseAuthorization();

app.MapControllers();


app.Run();
