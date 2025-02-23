using Confluent.Kafka;
using System.Text.Json;
using TransactionService.Core.Domain.Interfaces;
using TransactionService.Core.Domain.Entities;

namespace TransactionService.Infrastructure.Comunicaction
{
    public class TransactionCommunicationProducer : ITransactionCommunicationProducer
    {
        public async Task SendTransactionForVerification(TransactionForValidationDTO transaction, string topic)
        {
            var config = new ProducerConfig { BootstrapServers = "localhost:9092" };

            using var producer = new ProducerBuilder<Null, string>(config).Build();
            var transactionJson = JsonSerializer.Serialize(transaction);
            await producer.ProduceAsync(topic, new Message<Null, string>
            {
                Value = transactionJson
            });
        }
    }

    public class TransactionCommunicationConsumer : IHostedService
    {
        private readonly string _topic;
        private readonly IServiceProvider _serviceProvider;

        public TransactionCommunicationConsumer(string topic, IServiceProvider serviceProvider)
        {
            _topic = topic;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() => Consume(), cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            return Task.CompletedTask;
        }

        private void Consume()
        {
            var config = new ConsumerConfig
            {
                BootstrapServers = "localhost:9092",
                GroupId = "anti-fraud-group",
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
            consumer.Subscribe(_topic);

            while (true)
            {
                try
                {
                    var consumeResult = consumer.Consume();
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var transactionRepository = scope.ServiceProvider.GetRequiredService<ITransactionRepository>();
                        var transaction = JsonSerializer.Deserialize<TransactionValidationResult>(consumeResult.Message.Value);

                        transactionRepository.Update(transaction.TransactionExternalId, transaction.Status).Wait();
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al consumir mensaje: {ex.Message}");
                }
            }
        }
    }
}
