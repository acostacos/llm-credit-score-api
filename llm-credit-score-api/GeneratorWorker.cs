using llm_credit_score_api.Services.Interfaces;

namespace llm_credit_score_api
{
    public class GeneratorWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<GeneratorWorker> _logger;

        public GeneratorWorker(IServiceProvider serviceProvider, ILogger<GeneratorWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                if (_logger.IsEnabled(LogLevel.Information))
                {
                    _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);
                }

                await DoWork(stoppingToken);

                await Task.Delay(1000, stoppingToken);
            }
        }

        private async Task DoWork(CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var generatorService = scope.ServiceProvider.GetRequiredService<IGeneratorService>();

                var queuedTasks = generatorService.GetQueuedTasks();
                if (queuedTasks.Count() > 0)
                {
                    var task = queuedTasks.First();
                    await generatorService.GenerateReport(task.TaskId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
