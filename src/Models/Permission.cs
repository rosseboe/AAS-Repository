/*
 * DotAAS Part 2 | HTTP/REST | Entire Interface Collection
 *
 * The entire interface collection as part of Details of the Asset Administration Shell Part 2
 *
 * OpenAPI spec version: Final-Draft
 *
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */

using Newtonsoft.Json;
using System;
using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;
using System.Text;

namespace AdminShell
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class Permission : IEquatable<Permission>
    {
        /// <summary>
        /// Gets or Sets KindOfPermission
        /// </summary>
        [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum KindOfPermissionEnum
        {
            /// <summary>
            /// Enum AllowEnum for Allow
            /// </summary>
            [EnumMember(Value = "Allow")]
            AllowEnum = 0,
            /// <summary>
            /// Enum DenyEnum for Deny
            /// </summary>
            [EnumMember(Value = "Deny")]
            DenyEnum = 1,
            /// <summary>
            /// Enum NotApplicableEnum for NotApplicable
            /// </summary>
            [EnumMember(Value = "NotApplicable")]
            NotApplicableEnum = 2,
            /// <summary>
            /// Enum UndefinedEnum for Undefined
            /// </summary>
            [EnumMember(Value = "Undefined")]
            UndefinedEnum = 3        }

        /// <summary>
        /// Gets or Sets KindOfPermission
        /// </summary>
        [Required]

        [DataMember(Name="kindOfPermission")]
        public KindOfPermissionEnum? KindOfPermission { get; set; }

        /// <summary>
        /// Gets or Sets _Permission
        /// </summary>
        [Required]

        [DataMember(Name="permission")]
        public Reference _Permission { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Permission {\n");
            sb.Append("  KindOfPermission: ").Append(KindOfPermission).Append("\n");
            sb.Append("  _Permission: ").Append(_Permission).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Permission)obj);
        }

        /// <summary>
        /// Returns true if Permission instances are equal
        /// </summary>
        /// <param name="other">Instance of Permission to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Permission other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    KindOfPermission == other.KindOfPermission ||
                    KindOfPermission != null &&
                    KindOfPermission.Equals(other.KindOfPermission)
                ) &&
                (
                    _Permission == other._Permission ||
                    _Permission != null &&
                    _Permission.Equals(other._Permission)
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
                    if (KindOfPermission != null)
                    hashCode = hashCode * 59 + KindOfPermission.GetHashCode();
                    if (_Permission != null)
                    hashCode = hashCode * 59 + _Permission.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Permission left, Permission right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Permission left, Permission right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
