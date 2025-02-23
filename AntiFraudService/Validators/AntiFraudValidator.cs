using TransactionService.Core.Domain.Entities;

namespace AntiFraudService.Validators;

public class AntiFraudValidator
{
    public string Validate(TransactionForValidation transaction)
    {
        if (transaction.Value > 2000 || transaction.TodayTotal > 2000)
        {
            return "rejected";
        }

        return "approved";
    }
}