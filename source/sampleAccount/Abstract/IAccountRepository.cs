using System;
using System.Collections.Generic;
using System.Text;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface IAccountRepository
    {
        Account FindAccount(string accountName);
        decimal UpdateTransaction(Account account, AccountTransaction accountTransaction);
        void CollectFee(Account account, decimal fee);
    }
}
