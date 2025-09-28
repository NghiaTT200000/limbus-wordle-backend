


using Limbus_wordle_backend.Services.WebScrapperServices;

namespace Limbus_wordle_backend.Services.BackgroundService
{
    public class BackgroundScrapeData(ScrapeIdentitiesService scrapeIdentitiesService) : IHostedService, IDisposable
    {
        private readonly ScrapeIdentitiesService _scrapeIdentitiesService = scrapeIdentitiesService;
        private Timer? _timer;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _timer = new Timer(async state=> await DoWork(), null, TimeSpan.Zero, TimeSpan.FromMinutes(60*24));
            return Task.CompletedTask;
        }

        private async Task DoWork()
        {
            try
            {
                await _scrapeIdentitiesService.ScrapAsync();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _timer?.Change(Timeout.Infinite, 0);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}