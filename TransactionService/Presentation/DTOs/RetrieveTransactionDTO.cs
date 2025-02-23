namespace TransactionService.Presentation.DTOs
{
    public class RetrieveTransactionDTO
    {
        public Guid TransactionExternalId { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
