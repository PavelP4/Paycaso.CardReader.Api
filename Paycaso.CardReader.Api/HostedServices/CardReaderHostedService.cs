using Paycaso.CardReader.Application.Managers;

namespace Paycaso.CardReader.Api.HostedServices
{
    public class CardReaderHostedService : BackgroundService
    {
        private readonly ICardReaderManager _cardReaderManager;
        private readonly ILogger<CardReaderHostedService> _logger;

        public CardReaderHostedService(
            ICardReaderManager cardReaderManager,
            ILogger<CardReaderHostedService> logger)
        {
            _cardReaderManager = cardReaderManager;
            _logger = logger;
        }

        public override async Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"The service is starting.");
            await base.StartAsync(cancellationToken);
            _logger.LogInformation($"The service is started.");
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"Init the card reader.");

            try
            {
                
            }
            catch (TaskCanceledException)
            {
                // When the stopping token is canceled, for example, a call made from services.msc,
                // we shouldn't exit with a non-zero exit code. In other words, this is expected...
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "{Message}", ex.Message);

                // Terminates this process and returns an exit code to the operating system.
                // This is required to avoid the 'BackgroundServiceExceptionBehavior', which
                // performs one of two scenarios:
                // 1. When set to "Ignore": will do nothing at all, errors cause zombie services.
                // 2. When set to "StopHost": will cleanly stop the host, and log errors.
                //
                // In order for the Windows Service Management system to leverage configured
                // recovery options, we need to terminate the process with a non-zero exit code.
                Environment.Exit(1);
            }

            return Task.CompletedTask;
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation($"The service is stopping.");

            //

            await base.StopAsync(cancellationToken);
            _logger.LogInformation($"The service is stopped.");
        }
    }
}
