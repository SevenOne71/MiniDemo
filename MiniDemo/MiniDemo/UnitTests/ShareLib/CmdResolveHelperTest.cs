using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShareLib.CommandParser;
using ShareLib.CommandParser.Attributes;
using ShareLib.CommandParser.Converter;
using ShareLib.CommandParser.Interface;

namespace UnitTests.XmlParser
{
    public class TestCommand : IResolvableCmd
    {
        [Constraint(typeof (StringConverter), MaxLength = 1)]
        public string Data1 { set; get; }

        [Constraint(typeof (StringConverter), MaxLength = 6)]
        public string Data2 { set; get; }

        [Constraint(typeof (StringConverter), MaxLength = 2)]
        public string Data3 { set; get; }
    }

    [TestClass()]
    public class CmdResolveHelperTest
    {
        [TestMethod()]
        public void ParseStructByTypeTest()
        {
            var testCommand = new TestCommand();
            var command = "C,001001,OK";
            CmdResolveHelper.ParseStructByType(testCommand, typeof (TestCommand), command, ',');

            var expect = CmdResolveHelper.ToCommandString(testCommand, ',', false);
            Assert.AreEqual(command, expect);
        }
    }
}
