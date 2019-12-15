using sampleAccount.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sampleAccount.Abstract
{
    public interface IAccountService
    {
        Task<Account> CreateAccountAsync(Account account);
        Account GetAccountByNumber(string accountNumber);
        Account GetAccountByUserName(string name);
        Task<int> CountTransactionByAccountNameAsync(string accountName);
        Task<IList<AccountTransaction>> GetTransactionByAccountNameAsync(string accountName, Pagination pagination);
    }
}
