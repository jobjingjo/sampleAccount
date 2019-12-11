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
            _accountRepositoryMock = new Mock<IAccountRepository>();
            _settingConfigurationMock = new Mock<ISettingConfiguration>();
            _target = new TransactionService(
                _accountRepositoryMock.Object,
                _settingConfigurationMock.Object);
        }

        [TestMethod]
        public void Balance_WhenCheck_ShouldReturnValue()
        {
            _accountRepositoryMock.Setup(x => x.FindAccount(It.IsAny<string>()))
                .Returns(new Account());
            var result = _target.Balance("mockAccount1");
            Assert.AreEqual(0, result.Balance);
        }


    }
}
