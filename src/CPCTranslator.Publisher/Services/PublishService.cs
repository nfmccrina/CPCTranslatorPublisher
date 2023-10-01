using Azure.Messaging.ServiceBus;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace CPCTranslator.Publisher.Services
{
    public class PublishService : BackgroundService
    {
        private readonly IConfiguration configuration;
        private ServiceBusClient? client;
        private ServiceBusSender? sender;

        public PublishService(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            client = new ServiceBusClient(configuration["ServiceBusSendConnectionString"]);
            sender = client.CreateSender(configuration["QueueName"]);
        }

        public override async Task StopAsync(CancellationToken cancellationToken)
        {
            if (client != null)
            {
                await client.DisposeAsync();
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var nextTranslationUnit = Console.ReadLine();

                if (string.IsNullOrEmpty(nextTranslationUnit) || !nextTranslationUnit.StartsWith("TRANSLATED: "))
                {
                    continue;
                }

                if (sender == null)
                {
                    break;
                }

                var message = new ServiceBusMessage(nextTranslationUnit.Replace("TRANSLATED: ", ""));
                await sender.SendMessageAsync(message);    
            }
        }
    }
}