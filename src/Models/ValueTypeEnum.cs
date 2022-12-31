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
using System.Runtime.Serialization;

namespace AdminShell
{
    /// <summary>
    /// Gets or Sets ValueTypeEnum
    /// </summary>
    [JsonConverter(typeof(Newtonsoft.Json.Converters.StringEnumConverter))]
        public enum ValueTypeEnum
        {
            /// <summary>
            /// Enum AnyUriEnum for anyUri
            /// </summary>
            [EnumMember(Value = "anyUri")]
            AnyUriEnum = 0,
            /// <summary>
            /// Enum Base64BinaryEnum for base64Binary
            /// </summary>
            [EnumMember(Value = "base64Binary")]
            Base64BinaryEnum = 1,
            /// <summary>
            /// Enum BooleanEnum for boolean
            /// </summary>
            [EnumMember(Value = "boolean")]
            BooleanEnum = 2,
            /// <summary>
            /// Enum DateEnum for date
            /// </summary>
            [EnumMember(Value = "date")]
            DateEnum = 3,
            /// <summary>
            /// Enum DateTimeEnum for dateTime
            /// </summary>
            [EnumMember(Value = "dateTime")]
            DateTimeEnum = 4,
            /// <summary>
            /// Enum DateTimeStampEnum for dateTimeStamp
            /// </summary>
            [EnumMember(Value = "dateTimeStamp")]
            DateTimeStampEnum = 5,
            /// <summary>
            /// Enum DecimalEnum for decimal
            /// </summary>
            [EnumMember(Value = "decimal")]
            DecimalEnum = 6,
            /// <summary>
            /// Enum IntegerEnum for integer
            /// </summary>
            [EnumMember(Value = "integer")]
            IntegerEnum = 7,
            /// <summary>
            /// Enum LongEnum for long
            /// </summary>
            [EnumMember(Value = "long")]
            LongEnum = 8,
            /// <summary>
            /// Enum IntEnum for int
            /// </summary>
            [EnumMember(Value = "int")]
            IntEnum = 9,
            /// <summary>
            /// Enum ShortEnum for short
            /// </summary>
            [EnumMember(Value = "short")]
            ShortEnum = 10,
            /// <summary>
            /// Enum ByteEnum for byte
            /// </summary>
            [EnumMember(Value = "byte")]
            ByteEnum = 11,
            /// <summary>
            /// Enum NonNegativeIntegerEnum for nonNegativeInteger
            /// </summary>
            [EnumMember(Value = "nonNegativeInteger")]
            NonNegativeIntegerEnum = 12,
            /// <summary>
            /// Enum PositiveIntegerEnum for positiveInteger
            /// </summary>
            [EnumMember(Value = "positiveInteger")]
            PositiveIntegerEnum = 13,
            /// <summary>
            /// Enum UnsignedLongEnum for unsignedLong
            /// </summary>
            [EnumMember(Value = "unsignedLong")]
            UnsignedLongEnum = 14,
            /// <summary>
            /// Enum UnsignedIntEnum for unsignedInt
            /// </summary>
            [EnumMember(Value = "unsignedInt")]
            UnsignedIntEnum = 15,
            /// <summary>
            /// Enum UnsignedShortEnum for unsignedShort
            /// </summary>
            [EnumMember(Value = "unsignedShort")]
            UnsignedShortEnum = 16,
            /// <summary>
            /// Enum UnsignedByteEnum for unsignedByte
            /// </summary>
            [EnumMember(Value = "unsignedByte")]
            UnsignedByteEnum = 17,
            /// <summary>
            /// Enum NonPositiveIntegerEnum for nonPositiveInteger
            /// </summary>
            [EnumMember(Value = "nonPositiveInteger")]
            NonPositiveIntegerEnum = 18,
            /// <summary>
            /// Enum NegativeIntegerEnum for negativeInteger
            /// </summary>
            [EnumMember(Value = "negativeInteger")]
            NegativeIntegerEnum = 19,
            /// <summary>
            /// Enum DoubleEnum for double
            /// </summary>
            [EnumMember(Value = "double")]
            DoubleEnum = 20,
            /// <summary>
            /// Enum DurationEnum for duration
            /// </summary>
            [EnumMember(Value = "duration")]
            DurationEnum = 21,
            /// <summary>
            /// Enum DayTimeDurationEnum for dayTimeDuration
            /// </summary>
            [EnumMember(Value = "dayTimeDuration")]
            DayTimeDurationEnum = 22,
            /// <summary>
            /// Enum YearMonthDurationEnum for yearMonthDuration
            /// </summary>
            [EnumMember(Value = "yearMonthDuration")]
            YearMonthDurationEnum = 23,
            /// <summary>
            /// Enum FloatEnum for float
            /// </summary>
            [EnumMember(Value = "float")]
            FloatEnum = 24,
            /// <summary>
            /// Enum GDayEnum for gDay
            /// </summary>
            [EnumMember(Value = "gDay")]
            GDayEnum = 25,
            /// <summary>
            /// Enum GMonthEnum for gMonth
            /// </summary>
            [EnumMember(Value = "gMonth")]
            GMonthEnum = 26,
            /// <summary>
            /// Enum GMonthDayEnum for gMonthDay
            /// </summary>
            [EnumMember(Value = "gMonthDay")]
            GMonthDayEnum = 27,
            /// <summary>
            /// Enum GYearEnum for gYear
            /// </summary>
            [EnumMember(Value = "gYear")]
            GYearEnum = 28,
            /// <summary>
            /// Enum GYearMonthEnum for gYearMonth
            /// </summary>
            [EnumMember(Value = "gYearMonth")]
            GYearMonthEnum = 29,
            /// <summary>
            /// Enum HexBinaryEnum for hexBinary
            /// </summary>
            [EnumMember(Value = "hexBinary")]
            HexBinaryEnum = 30,
            /// <summary>
            /// Enum NOTATIONEnum for NOTATION
            /// </summary>
            [EnumMember(Value = "NOTATION")]
            NOTATIONEnum = 31,
            /// <summary>
            /// Enum QNameEnum for QName
            /// </summary>
            [EnumMember(Value = "QName")]
            QNameEnum = 32,
            /// <summary>
            /// Enum StringEnum for string
            /// </summary>
            [EnumMember(Value = "string")]
            StringEnum = 33,
            /// <summary>
            /// Enum NormalizedStringEnum for normalizedString
            /// </summary>
            [EnumMember(Value = "normalizedString")]
            NormalizedStringEnum = 34,
            /// <summary>
            /// Enum TokenEnum for token
            /// </summary>
            [EnumMember(Value = "token")]
            TokenEnum = 35,
            /// <summary>
            /// Enum LanguageEnum for language
            /// </summary>
            [EnumMember(Value = "language")]
            LanguageEnum = 36,
            /// <summary>
            /// Enum NameEnum for Name
            /// </summary>
            [EnumMember(Value = "Name")]
            NameEnum = 37,
            /// <summary>
            /// Enum NCNameEnum for NCName
            /// </summary>
            [EnumMember(Value = "NCName")]
            NCNameEnum = 38,
            /// <summary>
            /// Enum ENTITYEnum for ENTITY
            /// </summary>
            [EnumMember(Value = "ENTITY")]
            ENTITYEnum = 39,
            /// <summary>
            /// Enum IDEnum for ID
            /// </summary>
            [EnumMember(Value = "ID")]
            IDEnum = 40,
            /// <summary>
            /// Enum IDREFEnum for IDREF
            /// </summary>
            [EnumMember(Value = "IDREF")]
            IDREFEnum = 41,
            /// <summary>
            /// Enum NMTOKENEnum for NMTOKEN
            /// </summary>
            [EnumMember(Value = "NMTOKEN")]
            NMTOKENEnum = 42,
            /// <summary>
            /// Enum TimeEnum for time
            /// </summary>
            [EnumMember(Value = "time")]
            TimeEnum = 43        }
}
