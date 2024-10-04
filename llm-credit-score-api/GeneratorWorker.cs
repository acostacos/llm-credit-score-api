using llm_credit_score_api.Constants;
using llm_credit_score_api.Messages;
using llm_credit_score_api.Models;
using llm_credit_score_api.Services.Interfaces;
using System.Threading.Channels;

namespace llm_credit_score_api
{
    public class GeneratorWorker : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ChannelReader<AppTask> _channel;
        private readonly ILogger<GeneratorWorker> _logger;

        public GeneratorWorker(IServiceProvider serviceProvider, ChannelReader<AppTask> channel, ILogger<GeneratorWorker> logger)
        {
            _serviceProvider = serviceProvider;
            _channel = channel;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await foreach (var task in _channel.ReadAllAsync(stoppingToken))
            {
                try
                {
                    await DoWork(task, stoppingToken);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex.Message);
                }
            }
        }

        private async Task DoWork(AppTask task, CancellationToken stoppingToken)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();

                if (task.TaskKey == TaskKey.GenerateReport)
                {
                    var generatorService = scope.ServiceProvider.GetRequiredService<IGeneratorService>();
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
