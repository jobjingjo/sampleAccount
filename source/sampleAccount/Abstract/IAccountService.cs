using System;
using System.Collections.Generic;
using System.Text;
using sampleAccount.Models;

namespace sampleAccount.Abstract
{
    public interface IAccountService
    {
        Account CreateAccount(Account account);
        Account GetAccountByNumber(string accountNumber);
    }
}
