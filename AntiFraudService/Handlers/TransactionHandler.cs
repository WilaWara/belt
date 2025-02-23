using AntiFraudService.Kafka;
using AntiFraudService.Validators;
using System.Text.Json;
using TransactionService.Core.Domain.Entities;

namespace AntiFraudService.Handlers;

public class TransactionHandler
{
    private readonly AntiFraudValidator _validator;
    private readonly KafkaProducer _producer;

    public TransactionHandler(AntiFraudValidator validator)
    {
        _validator = validator;
        _producer = new KafkaProducer("localhost:9092");
    }

    public async Task HandleAsync(string transactionJson)
    {
        var transaction = JsonSerializer.Deserialize<TransactionForValidation>(transactionJson);
        var status = _validator.Validate(transaction);

        var validationResult = new TransactionValidationResult();
        validationResult.TransactionExternalId = transaction.transactionExternalId;
        validationResult.Status = status;

        var serializedResult = JsonSerializer.Serialize(validationResult);
        await _producer.ProduceAsync("validation-results-topic", serializedResult);
    }
}