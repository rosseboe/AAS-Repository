
namespace AdminShell
{
    using System.Collections.Generic;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class SubjectAttributes
    {
        [DataMember(Name="subjectAttributes")]
        [XmlArray(ElementName = "subjectAttributes")]
        public List<Reference> _SubjectAttributes { get; set; }
    }
}
