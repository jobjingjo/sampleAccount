using Microsoft.VisualStudio.TestTools.UnitTesting;
using sampleAccount;

namespace sampleAccount.Tests
{
    [TestClass]
    public class UnitTest1
    {
        private Class1 _target;

        [TestInitialize]
        public void Setup()
        {
            _target = new Class1();
        }

        [TestMethod]
        public void TestMethod1()
        {
            _target.Func1();
        }
    }
}
