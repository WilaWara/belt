namespace TransactionService.Core.Domain.Entities
{
    public class Auditable
    {
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
