namespace TransactionService.Core.Domain.Entities
{
    public class TransactionForValidationDTO
    {
        public Guid transactionExternalId { get; set; } = new Guid();
        public decimal Value { get; set; }
        public decimal TodayTotal { get; set; }
    }
}
