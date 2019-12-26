using System;
using System.Collections.Generic;
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
            _accounts = new List<AccountEntity>();
            _accounts.Add(new AccountEntity
            {
                IBAN = "mock"
            });
            _context.Accounts = MockDbSetHelper.CreateDbSetMock(_accounts).Object;
            var config = new MapperConfiguration(cfg => { cfg.AddProfile(new AutoMapping()); });

            _target = new AccountRepository(_context, new Mapper(config));
        }

        [TestMethod]
        public void CollectFeeAsync()
        {
            //_target.CollectFeeAsync();
        }

        [TestMethod]
        public void FindAccount()
        {
            var result = _target.FindAccount("mock");
            Assert.AreEqual("mock", result.AccountName);
        }
    }
}