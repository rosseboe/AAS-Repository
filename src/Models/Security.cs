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
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace AdminShell
{
    /// <summary>
    ///
    /// </summary>
    [DataContract]
    public partial class Security : IEquatable<Security>
    {
        /// <summary>
        /// Gets or Sets AccessControlPolicyPoints
        /// </summary>
        [Required]

        [DataMember(Name="accessControlPolicyPoints")]
        public AccessControlPolicyPoints AccessControlPolicyPoints { get; set; }

        /// <summary>
        /// Gets or Sets Certificate
        /// </summary>

        [DataMember(Name="certificate")]
        public List<Certificate> Certificate { get; set; }

        /// <summary>
        /// Gets or Sets RequiredCertificateExtension
        /// </summary>

        [DataMember(Name="requiredCertificateExtension")]
        public List<Reference> RequiredCertificateExtension { get; set; }

        /// <summary>
        /// Returns the string presentation of the object
        /// </summary>
        /// <returns>String presentation of the object</returns>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.Append("class Security {\n");
            sb.Append("  AccessControlPolicyPoints: ").Append(AccessControlPolicyPoints).Append("\n");
            sb.Append("  Certificate: ").Append(Certificate).Append("\n");
            sb.Append("  RequiredCertificateExtension: ").Append(RequiredCertificateExtension).Append("\n");
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
            return obj.GetType() == GetType() && Equals((Security)obj);
        }

        /// <summary>
        /// Returns true if Security instances are equal
        /// </summary>
        /// <param name="other">Instance of Security to be compared</param>
        /// <returns>Boolean</returns>
        public bool Equals(Security other)
        {
            if (ReferenceEquals(null, other)) return false;
            if (ReferenceEquals(this, other)) return true;

            return
                (
                    AccessControlPolicyPoints == other.AccessControlPolicyPoints ||
                    AccessControlPolicyPoints != null &&
                    AccessControlPolicyPoints.Equals(other.AccessControlPolicyPoints)
                ) &&
                (
                    Certificate == other.Certificate ||
                    Certificate != null &&
                    Certificate.SequenceEqual(other.Certificate)
                ) &&
                (
                    RequiredCertificateExtension == other.RequiredCertificateExtension ||
                    RequiredCertificateExtension != null &&
                    RequiredCertificateExtension.SequenceEqual(other.RequiredCertificateExtension)
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
                    if (AccessControlPolicyPoints != null)
                    hashCode = hashCode * 59 + AccessControlPolicyPoints.GetHashCode();
                    if (Certificate != null)
                    hashCode = hashCode * 59 + Certificate.GetHashCode();
                    if (RequiredCertificateExtension != null)
                    hashCode = hashCode * 59 + RequiredCertificateExtension.GetHashCode();
                return hashCode;
            }
        }

        #region Operators
        #pragma warning disable 1591

        public static bool operator ==(Security left, Security right)
        {
            return Equals(left, right);
        }

        public static bool operator !=(Security left, Security right)
        {
            return !Equals(left, right);
        }

        #pragma warning restore 1591
        #endregion Operators
    }
}
