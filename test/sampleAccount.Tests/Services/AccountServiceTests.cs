using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sampleAccount.Abstract;
using sampleAccount.Models;
using sampleAccount.Services;
using sampleAccount.TestHelpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace sampleAccount.Tests.Services
{
    [TestClass]
    public class AccountServiceTests
    {

        private IServiceProvider _provider;
        private AccountService _target;

        [TestInitialize]
        public void Setup()
        {
            _provider = new ServiceCollection()
                .AddScoped<AccountService>()
                .AddMock<IAccountRepository>()
                .BuildServiceProvider();
            _target = _provider.GetRequiredService<AccountService>();
        }

        [DataRow(1, DisplayName = "call with null of IAccountRepository")]
        [TestMethod]
        public void ConstructorTests(int caseNumber)
        {
            Assert.ThrowsException<ArgumentNullException>(() =>
            {
                _target = new AccountService(
                    caseNumber == 1 ? null : Mock.Of<IAccountRepository>()
                   );
            });
        }

        [TestMethod]
        public async Task CreateAccountAsync_WhenCalled_ShouldCallRepository()
        {
            //Arrange
            var account = new Account();
            _provider.GetMock<IAccountRepository>()
                .Setup(x => x.CreateAccountAsync(account))
                .Returns(Task.FromResult(account));

            //Act
            var result = await _target.CreateAccountAsync(account);

            //Assert
            _provider.GetMock<IAccountRepository>()
                .Verify(x => x.CreateAccountAsync(account), Times.Once);
        }

        [TestMethod]
        public void GetAccountByNumber_WhenCalled_ShouldCallRepository()
        {
            //Arrange
            var accountName = "mockAccount";
            _provider.GetMock<IAccountRepository>()
                .Setup(x => x.FindAccount(accountName))
                .Returns(new Account());

            //Act
            var result = _target.GetAccountByNumber(accountName);

            //Assert
            _provider.GetMock<IAccountRepository>()
                .Verify(x => x.FindAccount(accountName), Times.Once);
        }


        [TestMethod]
        public void GetAccountByUserName_WhenCalled_ShouldCallRepository()
        {
            //Arrange
            var userName = "mockUser";
            _provider.GetMock<IAccountRepository>()
                .Setup(x => x.FindAccountByOwner(userName))
                .Returns(new Account());

            //Act
            var result = _target.GetAccountByUserName(userName);

            //Assert
            _provider.GetMock<IAccountRepository>()
                .Verify(x => x.FindAccountByOwner(userName), Times.Once);
        }

        [TestMethod]
        public async Task CountTransactionByAccountNameAsync_WhenCalled_ShouldCallRepository()
        {
            //Arrange
            var accountName = "mockAccount";
            _provider.GetMock<IAccountRepository>()
                .Setup(x => x.CountTransactionByAccountAsync(accountName))
                .Returns(Task.FromResult(100));

            //Act
            var result = await _target.CountTransactionByAccountNameAsync(accountName);

            //Assert
            _provider.GetMock<IAccountRepository>()
                .Verify(x => x.CountTransactionByAccountAsync(accountName), Times.Once);
        }

        [TestMethod]
        public async Task GetTransactionByAccountNameAsync_WhenCalled_ShouldCallRepository()
        {
            //Arrange
            var pagination = new Pagination(1, 10);
            var accountName = "mockAccount";
            _provider.GetMock<IAccountRepository>()
                .Setup(x => x.FindTransactionByAccountAsync(accountName, pagination))
                .Returns(Task.FromResult((IList<AccountTransaction>)new List<AccountTransaction>()));

            //Act
            var result = await _target.GetTransactionByAccountNameAsync(accountName, pagination);

            //Assert
            _provider.GetMock<IAccountRepository>()
                .Verify(x => x.FindTransactionByAccountAsync(accountName, pagination), Times.Once);
        }
    }
}
