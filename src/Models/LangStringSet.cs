﻿
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class LangStringSet
    {
        public LangStringSet() { }

        public LangStringSet(LangStringSet value)
        {
            LangString = value.LangString;
        }

        [DataMember(Name = "langString")]
        [XmlArray(ElementName = "langString")]
        public List<LangString> LangString { get; set; } = new();
    }
}
