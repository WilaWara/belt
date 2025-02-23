using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TransactionService.Core.Domain.Entities
{
    [Table("Transactions")]
    public class Transaction : Auditable
    {
        [Key]
        public Guid TransactionExternalId { get; set; } = new Guid();
        public Guid SourceAccountId { get; set; }
        public Guid TargetAccountId { get; set; }
        public int TranferTypeId { get; set; }
        public string? Status { get; set; }
        public decimal Value { get; set; }
    }
}
