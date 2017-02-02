using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace UnitTests.XmlParser
{
    public interface IFake
    {
        bool DoSomething(string val);
        int ConventInt(int val);
    }

    [TestClass]
    public class MoqTest
    {
        [TestMethod]
        public void DoSomething_Test()
        {
            //refer to: http://www.cnblogs.com/techborther/archive/2012/01/10/2317998.html
            var moq = new Mock<IFake>();
            moq.Setup(s => s.DoSomething("ping")).Returns(true);
            Assert.AreEqual(true, moq.Object.DoSomething("ping"));
        }
    }
}
