using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface ITransactionService
    {
        OperationResult Balance(string accountName);
        Task<OperationResult> WithdrawAsync(AccountTransaction accountTransaction);

        Task<OperationResult> DepositAsync(AccountTransaction accountTransaction, decimal fee);
    }
}
