using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface IAccountService
    {
        Task<string> GetIBAN();
        Task<Account> CreateAccountAsync(Account account);
        Account GetAccountByNumber(string accountNumber);
        Account GetAccountByUserName(string name);
        Task<int> CountTransactionByAccountNameAsync(string accountName);
        Task<IList<AccountTransaction>> GetTransactionByAccountNameAsync(string accountName, Pagination pagination);
    }
}
