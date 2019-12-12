using sampleAccount.Abstract;
using sampleAccount.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace sampleAccount.DAL
{
    public class AccountRepository : IAccountRepository
    {
        public AccountRepository() {

        }
        public void CollectFee(Account account, decimal fee)
        {
            throw new NotImplementedException();
        }

        public Account FindAccount(string accountName)
        {
            throw new NotImplementedException();
        }

        public void UpdateTransaction(Account account, AccountTransaction accountTransaction)
        {
            throw new NotImplementedException();
        }
    }
}
