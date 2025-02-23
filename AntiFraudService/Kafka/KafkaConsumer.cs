using AntiFraudService.Handlers;
using Confluent.Kafka;
using System;

namespace AntiFraudService.Kafka;

public class KafkaConsumer
{
    private readonly string _topic;
    private readonly TransactionHandler _handler;

    public KafkaConsumer(string topic, TransactionHandler handler)
    {
        _topic = topic;
        _handler = handler;
    }

    public void Consume()
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
                _handler.HandleAsync(consumeResult.Message.Value).Wait();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al consumir mensaje: {ex.Message}");
            }
        }
    }
}