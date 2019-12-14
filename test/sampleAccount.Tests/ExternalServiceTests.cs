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
    public class ExternalServiceTests
    {
        private ExternalService _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new ExternalService();
        }

        [TestMethod]
        public async Task GetIBAN()
        {
            var result = await _target.GetIBAN();
            Assert.AreNotEqual(string.Empty, result);
        }
    }
}
