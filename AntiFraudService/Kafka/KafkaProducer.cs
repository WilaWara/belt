using Confluent.Kafka;
using System.Threading.Tasks;

namespace AntiFraudService.Kafka;

public class KafkaProducer
{
    private readonly IProducer<Null, string> _producer;

    public KafkaProducer(string bootstrapServers)
    {
        var config = new ProducerConfig
        {
            BootstrapServers = bootstrapServers
        };

        _producer = new ProducerBuilder<Null, string>(config).Build();
    }

    public async Task ProduceAsync(string topic, string message)
    {
        try
        {
            var deliveryResult = await _producer.ProduceAsync(topic, new Message<Null, string> { Value = message });
            Console.WriteLine($"Mensaje enviado a tópico: {deliveryResult.TopicPartitionOffset}");
        }
        catch (ProduceException<Null, string> ex)
        {
            Console.WriteLine($"Error al enviar mensaje: {ex.Error.Reason}");
        }
    }
}