using Microsoft.VisualStudio.TestTools.UnitTesting;
using sampleAccount.Services;
using System.Threading.Tasks;

namespace sampleAccount.Tests.Services
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
