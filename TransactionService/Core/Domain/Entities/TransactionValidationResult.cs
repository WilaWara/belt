namespace TransactionService.Core.Domain.Entities
{
    public class TransactionValidationResult
    {
        public Guid TransactionExternalId { get; set; }
        public string Status { get; set; }
    }
}
