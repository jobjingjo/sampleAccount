using Microsoft.VisualStudio.TestTools.UnitTesting;
using sampleAccount.DAL;

namespace sampleAccount.DAL.Tests
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

        [TestMethod]
        public void TestMethod2()
        {
            _target.Func2();
        }
    }
}
