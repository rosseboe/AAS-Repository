/*
 * DotAAS Part 2 | HTTP/REST | Asset Administration Shell Repository
 *
 * An exemplary interface combination for the use case of an Asset Administration Shell Repository
 *
 * OpenAPI spec version: Final-Draft
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using Newtonsoft.Json;
using System;
using System.Runtime.Serialization;
using System.Text;

namespace IO.Swagger.Models
{
    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public partial class ValueObject : IEquatable<ValueObject>
    {
        /// <summary>
        /// Gets or Sets Value
        /// </summary>

        [DataMember(Name = "value")]
        public string Value { get; set; }

        /// <summary>
        /// Gets or Sets ValueId
        /// </summary>

        [DataMember(Name = "valueId")]
        //TODO:
        //public HttpsapiSwaggerhubComdomainsPlattformI40SharedDomainModelsFinalDraftcomponentsschemasReference ValueId { get; set; }
        public Reference ValueId { get; set; }

        /// <summary>
        /// Gets or Sets ValueType
        /// </summary>

        [DataMember(Name = "valueType")]
        //TODO:
        //public HttpsapiSwaggerhubComdomainsPlattformI40SharedDomainModelsFinalDraftcomponentsschemasValueTypeEnum ValueType { get; set; }
        public ValueTypeEnum ValueType { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class ValueObject {\n");
            sb.Append("  Value: ").Append(Value).Append("\n");
            sb.Append("  ValueId: ").Append(ValueId).Append("\n");
            sb.Append("  ValueType: ").Append(ValueType).Append("\n");
            sb.Append("}\n");
            return sb.ToString();
        }

        /// <summary>
        /// Returns the JSON string presentation of the object
        /// </summary>
        /// <returns>JSON string presentation of the object</returns>
        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }

        /// <summary>
        /// Returns true if objects are equal
        /// </summary>
        /// <param name="obj">Object to be compared</param>
        /// <returns>Boolean</returns>
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == GetType() && Equals((ValueObject)obj);
        }

        /// <summary>
        /// Returns true if ValueObject instances are equal
        /// </summary>
        /// <param name="other">Instance of ValueObject to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(ValueObject other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    Value == other.Value ||
                    Value != null &&
                    Value.Equals(other.Value)
                ) &&
                (
                    ValueId == other.ValueId ||
                    ValueId != null &&
                    ValueId.Equals(other.ValueId)
                ) &&
                (
                    ValueType == other.ValueType ||
                    ValueType != null &&
                    ValueType.Equals(other.ValueType)
                );
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                // Suitable nullity checks etc, of course :)
                if (Value != null)
                    hashCode = hashCode * 59 + Value.GetHashCode();
                if (ValueId != null)
                    hashCode = hashCode * 59 + ValueId.GetHashCode();
                if (ValueType != null)
                    hashCode = hashCode * 59 + ValueType.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
#pragma warning disable 1591

        public static bool operator ==(ValueObject left, ValueObject right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(ValueObject left, ValueObject right)
        {
            return !Equals(left, right);
        }

#pragma warning restore 1591
        #endregion Operators
    }
}
