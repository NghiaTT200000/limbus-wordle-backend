using Limbus_wordle_backend.Services.IdentityFile;

namespace Limbus_wordle_backend.Services.Background
{
    public class BackgroundResetDailyIdentityMode(IdentityFileService identityFileService) : IHostedService, IDisposable
    {
        private Timer? _timer;
        private IdentityFileService _identityFileService = identityFileService;

        public Task StartAsync(CancellationToken cancellationToken)
        {
            ScheduleNextRun();
            return Task.CompletedTask;
        }

        private async Task DoWork()
        {
            Console.WriteLine(DateTime.Today.ToString()+" Resetting daily identity");
            await _identityFileService.Reset();
            ScheduleNextRun();
        }

        private void ScheduleNextRun()
        {
            var now = DateTime.Now;
            var nextMidnight = DateTime.Today.AddDays(1);
            var timeUntilNextMidnight = nextMidnight - now;

            _timer = new Timer(async state => await DoWork(), null, timeUntilNextMidnight, Timeout.InfiniteTimeSpan);
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