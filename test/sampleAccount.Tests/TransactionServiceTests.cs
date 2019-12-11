using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sampleAccount;
using sampleAccount.Abstract;
using sampleAccount.Models;
using sampleAccount.Services;
namespace sampleAccount.Tests
{
    [TestClass]
    public class TransactionServiceTests
    {        
        private TransactionService _target;
        private Mock<IAccountRepository> _accountRepositoryMock;
        private Mock<ISettingConfiguration> _settingConfigurationMock;

        [TestInitialize]
        public void Setup()
        {
            _accountRepositoryMock = new Mock<IAccountRepository>(MockBehavior.Strict);
            _settingConfigurationMock = new Mock<ISettingConfiguration>(MockBehavior.Strict);
            _target = new TransactionService(
                _accountRepositoryMock.Object,
                _settingConfigurationMock.Object);
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
                    .Returns(()=>null);

            //Act
            var result = _target.Balance(accountName);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public void Deposit_WhenNotFound_ShouldReturnStatusNotFound()
        {
            //Arrange
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Deposit
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(()=>null);

            //Act
            var result = _target.Deposit(accountTransaction);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public void Deposit_WhenCalled_ShouldUpdateAndCollectFee()
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
            _settingConfigurationMock.Setup(x => x.DepositFeeInPercent)
                .Returns(0.1m);
            _accountRepositoryMock.Setup(x => x.UpdateTransaction(account, accountTransaction));

            _accountRepositoryMock.Setup(x => x.CollectFee(account, It.IsAny<decimal>()));

            //Act
            var result = _target.Deposit(accountTransaction);

            //Assert
            Assert.AreEqual(999, result.Balance);
        }

        [TestMethod]
        public void Withdraw_WhenNotFound_ShouldReturnStatusNotFound()
        {
            //Arrange
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Withdraw
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(()=>null);

            //Act
            var result = _target.Withdraw(accountTransaction);

            //Assert
            Assert.AreEqual(OperationStatus.AccountNotFound, result.Status);
        }

        [TestMethod]
        public void Withdraw_WhenCalled_ShouldUpdateAndCollectFee()
        {
            //Arrange
            Account account = new Account() {
                Balance =1000
            };
            AccountTransaction accountTransaction = new AccountTransaction()
            {
                AccountName = "Mock",
                Amount = 1000,
                Type = TransactionType.Withdraw
            };
            _accountRepositoryMock.Setup(x => x.FindAccount(accountTransaction.AccountName))
                .Returns(account);

            _accountRepositoryMock.Setup(x => x.UpdateTransaction(account, accountTransaction));

            //Act
            var result = _target.Withdraw(accountTransaction);

            //Assert
            Assert.AreEqual(0, result.Balance);
        }

        [TestMethod]
        public void Withdraw_WhenNotEnoughMoney_ShouldReturnNotEnoughMoney()
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
            var result = _target.Withdraw(accountTransaction);

            //Assert
            Assert.AreEqual(OperationStatus.NotEnoughMoney, result.Status);
        }

    }
}
