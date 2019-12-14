using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sampleAccount.Abstract;
using sampleAccount.Models;
using sampleAccount.Services;
using System.Threading.Tasks;

namespace sampleAccount.Tests.Services
{
    [TestClass]
    public class TransactionServiceTests
    {
        private TransactionService _target;
        private Mock<IAccountRepository> _accountRepositoryMock;

        [TestInitialize]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>(MockBehavior.Strict);
            _target = new TransactionService(
                _accountRepositoryMock.Object);
        }

        [TestMethod]
        public void Balance_WhenCalled_ShouldReturnValue()
        {
            //Arrange
            string accountName = "mockAccount1";
            _accountRepositoryMock.Setup(x => x.FindAccount(accountName))
                    .Returns(new Account());

            //Act
            var result = _target.Balance(accountName);

            //Assert
            Assert.AreEqual(0, result.Balance);
        }

        [TestMethod]
        public void Balance_WhenNotFound_ShouldReturnStatusNotFound()
        {
            //Arrange
            string accountName = "mockAccount1";
            _accountRepositoryMock.Setup(x => x.FindAccount(accountName))
                    .Returns(() => null);

            //Act
            var result = _target.Balance(accountName);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public async Task Deposit_WhenNotFound_ShouldReturnStatusNotFound()
        {
            //Arrange
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Deposit
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(() => null);

            //Act
            var result = await _target.DepositAsync(accountTransaction, 0);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public async Task Deposit_WhenCalled_ShouldUpdateAndCollectFee()
        {
            //Arrange
            Account account = new Account();
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Deposit
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(account);

            _accountRepositoryMock.Setup(x => x.UpdateTransactionAsync(account, accountTransaction))
                .Returns(Task.CompletedTask);

            _accountRepositoryMock.Setup(x => x.CollectFeeAsync(account, It.IsAny<decimal>()))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _target.DepositAsync(accountTransaction, 1);

            //Assert
            Assert.AreEqual(999, result.Balance);
        }

        [TestMethod]
        public async Task Withdraw_WhenNotFound_ShouldReturnStatusNotFound()
        {
            //Arrange
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Withdraw
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(() => null);

            //Act
            var result = await _target.WithdrawAsync(accountTransaction);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public async Task Withdraw_WhenCalled_ShouldUpdateAndCollectFee()
        {
            //Arrange
            Account account = new Account()
            {
                Balance = 1000
            };
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Withdraw
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(account);

            _accountRepositoryMock.Setup(x => x.UpdateTransactionAsync(account, accountTransaction))
                .Returns(Task.CompletedTask);

            //Act
            var result = await _target.WithdrawAsync(accountTransaction);

            //Assert
            Assert.AreEqual(0, result.Balance);
        }

        [TestMethod]
        public async Task Withdraw_WhenNotEnoughMoney_ShouldReturnNotEnoughMoney()
        {
            //Arrange
            Account account = new Account()
            {
                Balance = 100
            };
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Withdraw
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(account);

            //Act
            var result = await _target.WithdrawAsync(accountTransaction);

            //Assert
            Assert.AreEqual(OperationStatus.NotEnoughMoney, result.Status);
        }

    }
}
