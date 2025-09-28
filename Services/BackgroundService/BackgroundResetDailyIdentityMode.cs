namespace Limbus_wordle_backend.Services.BackgroundService
{
    public class BackgroundResetDailyIdentityMode(DailyIdentityFileService dailyIdentityFileService) : IHostedService, IDisposable
    {
        private Timer? _timer;
        private DailyIdentityFileService _dailyIdentityFileService = dailyIdentityFileService;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            await DoWork();
        }

        private async Task DoWork()
        {
            Console.WriteLine(DateTime.Today.ToString()+" Resetting daily identity");
            await _dailyIdentityFileService.Reset();
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