﻿
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Xml.Serialization;

    public class UnitId
    {
        [XmlIgnore]
        public KeyList keys = new KeyList();

        [XmlArray("keys")]
        [XmlArrayItem("key")]
        public List<Key> Keys { get { return keys; } }

        [XmlIgnore]
        public bool IsEmpty { get { return keys == null || keys.IsEmpty; } }

        [XmlIgnore]
        public int Count { get { if (keys == null) return 0; return keys.Count; } }

        [XmlIgnore]
        public Key this[int index] { get { return keys[index]; } }

        public UnitId() { }

        public UnitId(UnitId src)
        {
            if (src.keys != null)
                foreach (var k in src.Keys)
                    keys.Add(new Key(k));
        }
    }
}
