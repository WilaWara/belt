using TransactionService.Core.Domain.Entities;

namespace TransactionService.Core.Domain.Interfaces
{
    public interface ITransactionRepository
    {
        Task<Transaction> GetById(Guid transactionExternalId);
        Task<decimal> GetTodayTotal();
        Task<Transaction> Create(Transaction transaction);
        Task Update(Guid transactionExternalId, string newStatus);
    }
}
