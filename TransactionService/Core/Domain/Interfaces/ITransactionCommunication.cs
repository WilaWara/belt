using TransactionService.Core.Domain.Entities;

namespace TransactionService.Core.Domain.Interfaces
{
    public interface ITransactionCommunicationProducer
    {
        Task SendTransactionForVerification(TransactionForValidationDTO transaction, string topic);
    }
}
