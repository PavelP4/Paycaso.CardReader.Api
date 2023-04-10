using Paycaso.CardReader.Application.Managers;
using Paycaso.CardReader.Application.Queues;

namespace Paycaso.CardReader.Api.HostedServices
{
    public class CardReaderHostedService : BackgroundService
    {
        private readonly CardReaderManager _cardReaderManager;
        private readonly CardReaderCommandQueue _queue;
        private readonly ILogger<CardReaderHostedService> _logger;

        public CardReaderHostedService(
            CardReaderManager cardReaderManager,
            CardReaderCommandQueue queue,
            ILogger<CardReaderHostedService> logger)
        {
            _cardReaderManager = cardReaderManager;
            _queue = queue;
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"The service is starting.");



            await base.StartAsync(cancellationToken);
            _logger.LogInformation($"The service is started.");
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Init the card reader.");

            try
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    var action = await _queue.DequeueOrWaitOne(stoppingToken);

                    action?.Invoke(_cardReaderManager, stoppingToken);
                }
            }
            catch (TaskCanceledException)
            {

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);
                
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"The service is stopping.");

            

            await base.StopAsync(cancellationToken);
            _logger.LogInformation($"The service is stopped.");
        }
    }
}
