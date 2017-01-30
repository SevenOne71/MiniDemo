using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Policy;
using System.Xml.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ShareLib.XmlParser;

namespace UnitTests.XmlParser
{
    #region
    /// <summary>
    /// The name of class should be the same as root name of xml file
    /// </summary>
    [Serializable]
    public class Configer 
    {
        [XmlAttribute]
        public string Mark { get; set; }

 
        [XmlElement(ElementName = "PairList")]
        public PairList PairList { get; set; }


        [XmlElement(ElementName = "MultiAttribute")]
        public MultiAttribute MultiAttribute { get; set; }

        [XmlElement(ElementName = "SubValues")]
        public SubValues SubValues { get; set; }
    }

    public class PairList
    {
        [XmlAttribute]
        public string Mark { get; set; }

        //[XmlArray(ElementName = "PairList")]
        //[XmlArrayItem(ElementName = "Setting")]
        [XmlElement(ElementName = "Setting")]
        public List<SerializablePair<int, int>> PairList11 { get; set; }
    }


    public class MultiAttribute
    {
        [XmlAttribute]
        public string Val1 { get; set; } //Name should be the same as xml's node

        [XmlAttribute]
        public string Val2 { get; set; }
    }

    public class SubValues
    {
        [XmlElement(ElementName = "Key1")]
        public int Key1 { get; set; }

        [XmlElement(ElementName = "Key2")]
        public double Key2 { get; set; }

        [XmlElement(ElementName = "Key3")]
        public string Key3 { get; set; }
    }


    #endregion


    /// <summary>
    ///This is a test class for XmlParseHelperTest and is intended
    ///to contain all XmlParseHelperTest Unit Tests
    ///</summary>
    [TestClass()]
    public class XmlParseHelperTest
    {
        [TestMethod()]
        public void DeserializeTest()
        {
            string path = System.IO.Directory.GetCurrentDirectory(); 
            Configer config;
            path = Path.Combine(path, @"..\..\XmlParser\Configer.xml");
            config = XmlParseHelper.Deserialize<Configer>(path);
        }

    }
}
