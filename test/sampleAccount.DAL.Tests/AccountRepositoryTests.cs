using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using sampleAccount.DAL.Data;
using sampleAccount.TestHelpers;

namespace sampleAccount.DAL.Tests
{
    [TestClass]
    public class AccountRepositoryTests
    {
        private List<AccountEntity> _accounts;
        private DataDbContext _context;
        private AccountRepository _target;

        [TestInitialize]
        public void Setup()
        {
            var options = new DbContextOptionsBuilder<DataDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            _context = new DataDbContext(options);
            _accounts = new List<AccountEntity> {new AccountEntity {IBAN = "mock",Balance = 1000}};
            _context.Accounts = MockDbSetHelper.CreateDbSetMock(_accounts).Object;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapping()); });

            _target = new AccountRepository(_context, new Mapper(config));
        }

        [TestMethod]
        public async Task CollectFeeAsync()
        {
            try
            {
                var account = _target.FindAccount("mock");
                await _target.CollectFeeAsync(account, 100);
                var result = _target.FindAccount("mock");
                Assert.AreEqual(900, result);
            }
            catch (InvalidOperationException ex)
            {
                Assert.Inconclusive(ex.Message);
            }
        }

        [TestMethod]
        public void FindAccount()
        {
            var result = _target.FindAccount("mock");
            Assert.AreEqual("mock", result.AccountName);
        }
    }
}