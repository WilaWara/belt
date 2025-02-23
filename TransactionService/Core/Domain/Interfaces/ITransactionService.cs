using TransactionService.Core.Domain.Entities;

namespace TransactionService.Core.Domain.Interfaces
{
    public interface ITransactionService
    {
        Task<Transaction> GetById(Guid transactionExternalId);
        Task<Transaction> Create(Transaction transaction);
    }
}
