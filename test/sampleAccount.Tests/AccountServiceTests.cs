using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using sampleAccount.Abstract;
using sampleAccount.Services;

namespace sampleAccount.Tests
{
    [TestClass]
    public class AccountServiceTests
    {
        private AccountService _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new AccountService(Mock.Of<IAccountRepository>());
        }

        [TestMethod]
        public async Task GetIBAN()
        {
            var result = await _target.GetIBAN();
            Assert.AreNotEqual(string.Empty, result);
        }
    }
}
