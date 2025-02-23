using TransactionService.Core.Domain.Entities;
using TransactionService.Core.Domain.Interfaces;

namespace TransactionService.Core.Application
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly ITransactionCommunicationProducer _transactionCommunicationProducer;

        public TransactionService(
            ITransactionRepository transactionRepository, 
            ITransactionCommunicationProducer transactionCommunicationProducer
        )
        {
            _transactionRepository = transactionRepository;
            _transactionCommunicationProducer = transactionCommunicationProducer;
        }

        public async Task<Transaction> GetById(Guid transactionExternalId)
        {
            var retrievedTransaction = await _transactionRepository.GetById(transactionExternalId);
            return retrievedTransaction;
        }

        public async Task<Transaction> Create(Transaction transaction)
        {
            transaction.Status = "Pending";
            var createdTransaction = await _transactionRepository.Create(transaction);
            var todaytotal = await _transactionRepository.GetTodayTotal();

            var transactionForValidation = new TransactionForValidationDTO();
            transactionForValidation.transactionExternalId = createdTransaction.TransactionExternalId;
            transactionForValidation.Value = createdTransaction.Value;
            transactionForValidation.TodayTotal = todaytotal + createdTransaction.Value;

            await _transactionCommunicationProducer.SendTransactionForVerification(transactionForValidation, "transactions-topic");
            return createdTransaction;
        }
    }
}
