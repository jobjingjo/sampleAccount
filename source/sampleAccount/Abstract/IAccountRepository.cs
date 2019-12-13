using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface IAccountRepository
    {
        Account FindAccount(string accountName);
        Task UpdateTransactionAsync(Account account, AccountTransaction accountTransaction);
        Task CollectFeeAsync(Account account, decimal fee);
        Task<Account> CreateAccountAsync(Account account);
        Account FindAccountByOwner(string name);
        Task<IList<AccountTransaction>> FindTransactionByAccountAsync(string accountName, Pagination pagination);
        Task<int> CountTransactionByAccountAsync(string accountName);
    }
}
