using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace ShareLib.XmlParser
{
    [Serializable]
    public class SerializablePair<TKey, TValue>
    {
        [XmlAttribute("Key")]
        public TKey Key { get; set; }

        [XmlAttribute("Value")]
        public TValue Value { get; set; }

        public SerializablePair()
        {
        }

        public SerializablePair(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public SerializablePair(KeyValuePair<TKey, TValue> pair)
        {
            Key = pair.Key;
            Value = pair.Value;
        }
    }

}
