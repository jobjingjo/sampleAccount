using System;
using System.Collections.Generic;
using System.Text;
using sampleAccount.Abstract;
using sampleAccount.Models;

namespace sampleAccount.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;
        private readonly ISettingConfiguration _settingConfiguration;

        public TransactionService(IAccountRepository accountRepository,
            ISettingConfiguration settingConfiguration) {
            _accountRepository = accountRepository ?? throw new System.ArgumentNullException(nameof(accountRepository));
            _settingConfiguration = settingConfiguration ?? throw new System.ArgumentNullException(nameof(settingConfiguration));
        }

        public OperationResult Balance(string accountName)
        {
            OperationResult result = new OperationResult();
            Account account = _accountRepository.FindAccount(accountName);
            if (account == null)
            {
                result.Status = OperationStatus.AccountNotFound;
            }
            else {
                result.Balance = account.Balance;
                result.Status = OperationStatus.Ok;
            }

            return result;
        }
        public OperationResult Withdraw(AccountTransaction accountTransaction)
        {
            OperationResult result = new OperationResult();
            var accountName = accountTransaction.AccountName;
            Account account = _accountRepository.FindAccount(accountName);
            if (account == null)
            {
                result.Status = OperationStatus.AccountNotFound;
            }
            else
            {
                result.Balance = account.Balance;
                if (result.Balance >= accountTransaction.Amount)
                {
                    result.Status = OperationStatus.Ok;
                    result.Balance = _accountRepository.UpdateTransaction(account, accountTransaction);
                }
                else {
                    result.Status = OperationStatus.BalanceNotValid;
                }
            }
            return result;
        }
        public OperationResult Deposit(AccountTransaction accountTransaction)
        {
            OperationResult result = new OperationResult();
            var accountName = accountTransaction.AccountName;
            Account account = _accountRepository.FindAccount(accountName);
            if (account == null)
            {
                result.Status = OperationStatus.AccountNotFound;
            }
            else
            {
                result.Balance = account.Balance;
                var fee = _settingConfiguration.DepositFee;
                if (accountTransaction.Amount >= fee)
                {
                    result.Status = OperationStatus.Ok;
                    accountTransaction.Amount -= _settingConfiguration.DepositFee;
                    result.Balance = _accountRepository.UpdateTransaction(account, accountTransaction);
                    _accountRepository.CollectFee(account, fee);
                }
                else
                {
                    result.Status = OperationStatus.BalanceNotValid;
                }
            }
            return result;
        }
    }

}
