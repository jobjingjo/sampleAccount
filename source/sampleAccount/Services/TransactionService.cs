using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using sampleAccount.Abstract;
using sampleAccount.Models;

namespace sampleAccount.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly IAccountRepository _accountRepository;

        public TransactionService(IAccountRepository accountRepository) {
            _accountRepository = accountRepository ?? throw new System.ArgumentNullException(nameof(accountRepository));
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
        public async Task<OperationResult> WithdrawAsync(AccountTransaction accountTransaction)
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
                    account.Balance -= accountTransaction.Amount;
                    await _accountRepository.UpdateTransactionAsync(account, accountTransaction);
                    result.Balance = account.Balance;
                }
                else {
                    result.Status = OperationStatus.NotEnoughMoney;
                }
            }
            return result;
        }
        public async Task<OperationResult> DepositAsync(AccountTransaction accountTransaction, decimal fee)
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
                result.Status = OperationStatus.Ok;
                accountTransaction.Amount -= fee;
                account.Balance += accountTransaction.Amount;
                await _accountRepository.UpdateTransactionAsync(account, accountTransaction);
                if (fee > 0) await _accountRepository.CollectFeeAsync(account, fee);
                result.Balance = account.Balance;
            }
            return result;
        }
    }

}
