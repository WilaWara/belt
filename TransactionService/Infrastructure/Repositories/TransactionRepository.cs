using Microsoft.EntityFrameworkCore;
using TransactionService.Core.Domain.Entities;
using TransactionService.Core.Domain.Interfaces;

namespace TransactionService.Infrastructure.Repositories
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly ApplicationDBContext _dbContext;

        public TransactionRepository(ApplicationDBContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Transaction> GetById(Guid transactionExternalId)
        {
            var existingTransaction = _dbContext
                .Transactions
                .FirstOrDefault(t => t.TransactionExternalId == transactionExternalId);
            return existingTransaction;
        }

        public async Task<decimal> GetTodayTotal()
        {
            var todayTotal = _dbContext.Transactions.Where(t => t.Status == "approved").Sum(t => t.Value);
            return todayTotal;
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            _dbContext.Transactions.Add(transaction);
            _dbContext.SaveChanges();
            return transaction;
        }

        public async Task Update(Guid transactionExternalId, string newStatus)
        {
            var existingTransaction = await GetById(transactionExternalId);

            if (existingTransaction == null)
            {
                throw new InvalidOperationException("No se encontró la transacción al tratar de actualizar su status");
            }

            existingTransaction.Status = newStatus;

            _dbContext.Transactions.Update(existingTransaction);
            _dbContext.SaveChanges();
        }
    }
}
