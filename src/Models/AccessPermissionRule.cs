
namespace AdminShell
{
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Runtime.Serialization;
    using System.Xml.Serialization;

    [DataContract]
    public class AccessPermissionRule : Referable
    {
        [DataMember(Name="qualifiers")]
        [XmlArray(ElementName= "qualifiers")]
        public List<Qualifier> Qualifiers { get; set; }

        [DataMember(Name="permissionsPerObject")]
        [XmlArray(ElementName = "permissionsPerObject")]
        public List<PermissionsPerObject> PermissionsPerObject { get; set; }

        [Required]
        [DataMember(Name="targetSubjectAttributes")]
        [XmlArray(ElementName = "targetSubjectAttributes")]
        public List<SubjectAttributes> TargetSubjectAttributes { get; set; }
    }
}
