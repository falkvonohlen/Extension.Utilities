using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace Extension.Utilities.Tests.Serialization
{
    [Serializable]
    [XmlRoot("TestObject", IsNullable = false)]
    public class SerializableObject
    {
        [XmlElement]
        public string ValueA { get; set; }

        [XmlElement]
        public int ValueB { get; set; }
    }
}
