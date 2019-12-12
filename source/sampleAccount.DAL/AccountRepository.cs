using AutoMapper;
using sampleAccount.Abstract;
using sampleAccount.DAL.Data;
using sampleAccount.Helpers;
using sampleAccount.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace sampleAccount.DAL
{
    public class AccountRepository : IAccountRepository
    {
        private readonly DataDbContext _dataDbContext;
        private readonly IMapper _mapper;

        public AccountRepository(DataDbContext dataDbContext, IMapper mapper) {
            _dataDbContext = dataDbContext ?? throw new ArgumentNullException(nameof(dataDbContext));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }
        public async System.Threading.Tasks.Task CollectFeeAsync(Account account, decimal fee)
        {
            using (var dbContextTransaction = _dataDbContext.Database.BeginTransaction())
            {
                var entity = _dataDbContext.Accounts.Single(x => string.Equals(x.IBAN, account.AccountName));    

                entity.Balance = account.Balance;
                entity.UpdatedAt = SystemDateTime.UtcNow();

                var transaction = new TransactionEntity()
                {
                    FromId = entity.Id,
                    Amount = fee,
                    CreateAt = entity.UpdatedAt,
                    Type = TransactionType.Withdraw,
                    Status = OperationStatus.Ok,
                    AccountTo = "SYSTEM"
                };

                _dataDbContext.Transactions.Add(transaction);
                await _dataDbContext.SaveChangesAsync();
                dbContextTransaction.Commit();
            }
        }

        public async System.Threading.Tasks.Task<Account> CreateAccountAsync(Account account)
        {
            using (var dbContextTransaction = _dataDbContext.Database.BeginTransaction())
            {
                if (_dataDbContext.Accounts.Any(x => string.Equals(x.IBAN, account.AccountName)))
                    return null;

                var accountEntity = _mapper.Map<AccountEntity>(account);

                _dataDbContext.Accounts.Add(accountEntity);
                await _dataDbContext.SaveChangesAsync();
                dbContextTransaction.Commit();
                return account;              
            }
        }

        public Account FindAccount(string accountName)
        {
            var entity = _dataDbContext.Accounts.SingleOrDefault(x => string.Equals(x.IBAN, accountName));
            var account = _mapper.Map<Account>(entity);
            return account;
        }

        public Account FindAccountByOwner(string name)
        {
            var entity = _dataDbContext.Accounts.SingleOrDefault(x => string.Equals(x.OwenerId, name));
            var account = _mapper.Map<Account>(entity);
            return account;
        }

        public async System.Threading.Tasks.Task UpdateTransactionAsync(Account account, AccountTransaction accountTransaction)
        {
            using (var dbContextTransaction = _dataDbContext.Database.BeginTransaction())
            {
                var entity = _dataDbContext.Accounts.Single(x => string.Equals(x.IBAN, account.AccountName));
                var entityTo = _dataDbContext.Accounts.SingleOrDefault(x => string.Equals(x.IBAN, accountTransaction.AccountName));

                    entity.Balance = account.Balance;
                    entity.UpdatedAt = SystemDateTime.UtcNow();

                    var transaction = new TransactionEntity() {
                        FromId = entity.Id,
                        Amount = accountTransaction.Amount,
                        CreateAt = entity.UpdatedAt,
                        Type = accountTransaction.Type,
                        Status = OperationStatus.Ok,
                        AccountTo = accountTransaction.AccountName
                    };

                if (entityTo == null) {
                    transaction.Status = OperationStatus.AccountNotFound;
                }
                _dataDbContext.Transactions.Add(transaction);
                await _dataDbContext.SaveChangesAsync();
                dbContextTransaction.Commit();
            }
        }
    }
}
