using System;
using System.Collections.Generic;
using System.Text;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface ITransactionService
    {
        OperationResult Balance(string accountName);
        OperationResult Withdraw(AccountTransaction accountTransaction);

        OperationResult Deposit(AccountTransaction accountTransaction);
    }
}
