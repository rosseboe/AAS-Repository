﻿/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml;
using System.Xml.Serialization;


//namespace AdminShellNS
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        [XmlType(TypeName = "submodelElement")]
        public class SubmodelElementWrapper
        {

            // members

            [XmlElement(ElementName = "property", Type = typeof(Property))]
            [XmlElement(ElementName = "multiLanguageProperty", Type = typeof(MultiLanguageProperty))]
            [XmlElement(ElementName = "range", Type = typeof(Range))]
            [XmlElement(ElementName = "file", Type = typeof(File))]
            [XmlElement(ElementName = "blob", Type = typeof(Blob))]
            [XmlElement(ElementName = "referenceElement", Type = typeof(ReferenceElement))]
            [XmlElement(ElementName = "relationshipElement", Type = typeof(RelationshipElement))]
            [XmlElement(ElementName = "annotatedRelationshipElement", Type = typeof(AnnotatedRelationshipElement))]
            [XmlElement(ElementName = "capability", Type = typeof(Capability))]
            [XmlElement(ElementName = "submodelElementCollection", Type = typeof(SubmodelElementCollection))]
            [XmlElement(ElementName = "operation", Type = typeof(Operation))]
            [XmlElement(ElementName = "basicEvent", Type = typeof(BasicEvent))]
            [XmlElement(ElementName = "entity", Type = typeof(Entity))]
            [XmlElement(ElementName = "submodelElementList", Type = typeof(SubmodelElementList))]
            [XmlElement(ElementName = "submodelElementStruct", Type = typeof(SubmodelElementStruct))]
            [XmlElement(ElementName = "globalReferenceElement", Type = typeof(GlobalReferenceElement))]
            [XmlElement(ElementName = "modelReferenceElement", Type = typeof(ModelReferenceElement))]
            public SubmodelElement submodelElement;

            /// <summary>
            /// Major version of the meta-model
            /// </summary>
            [XmlIgnore]

            public static string MetaModelVersionCoarse = "AAS3.0";

            /// <summary>
            /// Minor version (extension) of the meta-model.
            /// Should be added to <c>MetaModelVersionCoarse</c>
            /// </summary>
            [XmlIgnore]

            public static string MetaModelVersionFine = "RC02";

            // element names
            public enum AdequateElementEnum
            {
                Unknown = 0, SubmodelElementCollection, Property, MultiLanguageProperty, Range, File, Blob,
                ReferenceElement, RelationshipElement, AnnotatedRelationshipElement, Capability, Operation,
                BasicEvent, Entity, SubmodelElementList, SubmodelElementStruct,
                ModelReferenceElement, GlobalReferenceElement
            }

            public static AdequateElementEnum[] AdequateElementsDataElement =
            {
                AdequateElementEnum.SubmodelElementCollection, AdequateElementEnum.RelationshipElement,
                AdequateElementEnum.AnnotatedRelationshipElement, AdequateElementEnum.Capability,
                AdequateElementEnum.Operation, AdequateElementEnum.BasicEvent, AdequateElementEnum.Entity
            };

            // shall be consistent to (int) AdequateElementEnum !
            public static string[] AdequateElementNames = {
                "Unknown", "SubmodelElementCollection", "Property",
                "MultiLanguageProperty", "Range", "File", "Blob", "ReferenceElement", "RelationshipElement",
                "AnnotatedRelationshipElement", "Capability", "Operation", "BasicEvent", "Entity",
                "SubmodelElementList", "SubmodelElementStruct",
                "ModelReferenceElement", "GlobalReferenceElement"
            };

            // shall be consistent to (int) AdequateElementEnum !
            public static string[] AdequateElementShortName = {
                null, "SMC", null,
                "MLP", null, null, null, "Ref", "Rel",
                "ARel", null, null, "Event", "Entity", "SML", "SMS",
                "RefM", "RefG"
            };

            // shall be consistent to (int) AdequateElementEnum !
            public static bool[] AdequateElementDeprecated = {
                false, true, false,
                false, false, false, false, true, false,
                false, false, false, false, false, false, false,
                false, false
            };

            // constructors

            public SubmodelElementWrapper() { }

            // cloning

            public SubmodelElementWrapper(SubmodelElement src, bool shallowCopy = false)
            {
                /* TODO (MIHO, 2021-08-12): consider using:
                   Activator.CreateInstance(pl.GetType(), new object[] { pl }) */

                if (src is Property)
                    this.submodelElement = new Property(src as Property);
                if (src is MultiLanguageProperty)
                    this.submodelElement = new MultiLanguageProperty(src as MultiLanguageProperty);
                if (src is Range)
                    this.submodelElement = new Range(src as Range);
                if (src is File)
                    this.submodelElement = new File(src as File);
                if (src is Blob)
                    this.submodelElement = new Blob(src as Blob);

                if (src is Capability)
                    this.submodelElement = new Capability(src as Capability);
                if (src is Operation)
                    this.submodelElement = new Operation(src as Operation);
                if (src is BasicEvent)
                    this.submodelElement = new BasicEvent(src as BasicEvent);
                if (src is Entity)
                    this.submodelElement = new Entity(src as Entity);

                // care abuot elements, which could be derived from (not so) abstract base types
                if (src is SubmodelElementList)
                    this.submodelElement = new SubmodelElementList(src as SubmodelElementList);
                else
                if (src is SubmodelElementStruct)
                    this.submodelElement = new SubmodelElementStruct(src as SubmodelElementStruct);
                else
                if (src is SubmodelElementCollection)
                    this.submodelElement = new SubmodelElementCollection(
                        src as SubmodelElementCollection, shallowCopy: shallowCopy);

                // again
                if (src is AnnotatedRelationshipElement)
                    this.submodelElement = new AnnotatedRelationshipElement(src as AnnotatedRelationshipElement);
                else
                if (src is RelationshipElement)
                    this.submodelElement = new RelationshipElement(src as RelationshipElement);

                // again
                if (src is ModelReferenceElement)
                    this.submodelElement = new ModelReferenceElement(src as ModelReferenceElement);
                else
                    if (src is GlobalReferenceElement)
                    this.submodelElement = new GlobalReferenceElement(src as GlobalReferenceElement);
                else
                    if (src is ReferenceElement)
                    this.submodelElement = new ReferenceElement(src as ReferenceElement);

            }

#if !DoNotUseAasxCompatibilityModels
            public SubmodelElementWrapper(
                AasxCompatibilityModels.AdminShellV10.SubmodelElement src, bool shallowCopy = false)
            {
                if (src is AasxCompatibilityModels.AdminShellV10.SubmodelElementCollection)
                    this.submodelElement = new SubmodelElementCollection(
                        src as AasxCompatibilityModels.AdminShellV10.SubmodelElementCollection,
                        shallowCopy: shallowCopy);
                if (src is AasxCompatibilityModels.AdminShellV10.Property)
                    this.submodelElement = new Property(src as AasxCompatibilityModels.AdminShellV10.Property);
                if (src is AasxCompatibilityModels.AdminShellV10.File)
                    this.submodelElement = new File(src as AasxCompatibilityModels.AdminShellV10.File);
                if (src is AasxCompatibilityModels.AdminShellV10.Blob)
                    this.submodelElement = new Blob(src as AasxCompatibilityModels.AdminShellV10.Blob);
                if (src is AasxCompatibilityModels.AdminShellV10.ReferenceElement)
                    this.submodelElement = new ReferenceElement(
                        src as AasxCompatibilityModels.AdminShellV10.ReferenceElement);
                if (src is AasxCompatibilityModels.AdminShellV10.RelationshipElement)
                    this.submodelElement = new RelationshipElement(
                        src as AasxCompatibilityModels.AdminShellV10.RelationshipElement);
                if (src is AasxCompatibilityModels.AdminShellV10.Operation)
                    this.submodelElement = new Operation(src as AasxCompatibilityModels.AdminShellV10.Operation);
            }

            public SubmodelElementWrapper(
                AasxCompatibilityModels.AdminShellV20.SubmodelElement src, bool shallowCopy = false)
            {
                /* TODO (MIHO, 2021-08-12): consider using:
                   Activator.CreateInstance(pl.GetType(), new object[] { pl }) */

                if (src is AasxCompatibilityModels.AdminShellV20.SubmodelElementCollection)
                    this.submodelElement = new SubmodelElementCollection(
                        src as AasxCompatibilityModels.AdminShellV20.SubmodelElementCollection,
                        shallowCopy: shallowCopy);
                if (src is AasxCompatibilityModels.AdminShellV20.Property)
                    this.submodelElement = new Property(src as AasxCompatibilityModels.AdminShellV20.Property);
                if (src is AasxCompatibilityModels.AdminShellV20.MultiLanguageProperty)
                    this.submodelElement = new MultiLanguageProperty(
                        src as AasxCompatibilityModels.AdminShellV20.MultiLanguageProperty);
                if (src is AasxCompatibilityModels.AdminShellV20.Range)
                    this.submodelElement = new Range(src as AasxCompatibilityModels.AdminShellV20.Range);
                if (src is AasxCompatibilityModels.AdminShellV20.File)
                    this.submodelElement = new File(src as AasxCompatibilityModels.AdminShellV20.File);
                if (src is AasxCompatibilityModels.AdminShellV20.Blob)
                    this.submodelElement = new Blob(src as AasxCompatibilityModels.AdminShellV20.Blob);
                if (src is AasxCompatibilityModels.AdminShellV20.RelationshipElement)
                    this.submodelElement = new RelationshipElement(
                        src as AasxCompatibilityModels.AdminShellV20.RelationshipElement);
                if (src is AasxCompatibilityModels.AdminShellV20.AnnotatedRelationshipElement)
                    this.submodelElement = new AnnotatedRelationshipElement(
                        src as AasxCompatibilityModels.AdminShellV20.AnnotatedRelationshipElement);
                if (src is AasxCompatibilityModels.AdminShellV20.Capability)
                    this.submodelElement = new Capability(src as AasxCompatibilityModels.AdminShellV20.Capability);
                if (src is AasxCompatibilityModels.AdminShellV20.Operation)
                    this.submodelElement = new Operation(src as AasxCompatibilityModels.AdminShellV20.Operation);
                if (src is AasxCompatibilityModels.AdminShellV20.BasicEvent)
                    this.submodelElement = new BasicEvent(src as AasxCompatibilityModels.AdminShellV20.BasicEvent);
                if (src is AasxCompatibilityModels.AdminShellV20.Entity)
                    this.submodelElement = new Entity(src as AasxCompatibilityModels.AdminShellV20.Entity);

                // a little more special
                if (src is AasxCompatibilityModels.AdminShellV20.ReferenceElement)
                    this.submodelElement = new ModelReferenceElement(
                        src as AasxCompatibilityModels.AdminShellV20.ReferenceElement);
            }
#endif

            public static string GetAdequateName(AdequateElementEnum ae)
            {
                return AdequateElementNames[(int)ae];
            }

            /// <summary>
            /// Deprecated. See below.
            /// </summary>
            public static AdequateElementEnum GetAdequateEnum(string adequateName)
            {
                if (adequateName == null)
                    return AdequateElementEnum.Unknown;

                foreach (var en in (AdequateElementEnum[])Enum.GetValues(typeof(AdequateElementEnum)))
                    if (Enum.GetName(typeof(AdequateElementEnum), en)?.Trim().ToLower() ==
                        adequateName.Trim().ToLower())
                        return en;

                return AdequateElementEnum.Unknown;
            }

            /// <summary>
            /// This version uses the element name array and allows using ShortName
            /// </summary>
            public static AdequateElementEnum GetAdequateEnum2(string adequateName, bool useShortName = false)
            {
                if (adequateName == null)
                    return AdequateElementEnum.Unknown;

                foreach (var en in (AdequateElementEnum[])Enum.GetValues(typeof(AdequateElementEnum)))
                    if (((int)en < AdequateElementNames.Length
                          && AdequateElementNames[(int)en].Trim().ToLower() == adequateName.Trim().ToLower())
                        || (useShortName
                          && (int)en < AdequateElementShortName.Length
                          && AdequateElementShortName[(int)en] != null
                          && AdequateElementShortName[(int)en].Trim().ToLower() == adequateName.Trim().ToLower()))
                        return en;

                return AdequateElementEnum.Unknown;
            }

            /// <summary>
            /// Returns, if the element is deprecated in the most current version of the meta model.
            /// </summary>
            public static bool GetElementIsDeprecated(AdequateElementEnum ae)
            {
                return AdequateElementDeprecated[(int)ae];
            }

            /// <summary>
            /// Returns, if the element is deprecated in the most current version of the meta model.
            /// </summary>
            public static bool GetElementIsDeprecated(SubmodelElement sme)
            {
                if (sme == null)
                    return false;
                var sd = sme.GetSelfDescription();
                if (sd == null)
                    return false;
                return AdequateElementDeprecated[(int)sd.ElementEnum];
            }

            /// <summary>
            /// Returns a rather general statement, if the SME is deprecated.
            /// </summary>
            /// <param name="sme"></param>
            public static string EvalDeprecationMessage(SubmodelElement sme)
            {
                string res = null;
                if (AdminShell.SubmodelElementWrapper.GetElementIsDeprecated(sme))
                {
                    res = "This SubmodelElement is considered deprecated by the AdminShell meta model "
                            + AdminShell.MetaModelVersionCoarse + AdminShell.MetaModelVersionFine + ". "
                            + "Please refactor to another adequate SubmodelElement. ";

                    if (sme is SubmodelElementCollection)
                        res += "Please consider to used SubmodelElementList or SubmodelElemenStructure. ";
                    if (sme is ReferenceElement)
                        res += "For references to elements of an AAS, either local or external, consider " +
                            "ModelReferenceElement. For references to external information or services, " +
                            "consider GlobalReferenceElement. ";
                }
                return res;
            }

            public static IEnumerable<AdequateElementEnum> GetAdequateEnums(
                AdequateElementEnum[] excludeValues = null, AdequateElementEnum[] includeValues = null)
            {
                if (includeValues != null)
                {
                    foreach (var en in includeValues)
                        yield return en;
                }
                else
                {
                    foreach (var en in (AdequateElementEnum[])Enum.GetValues(typeof(AdequateElementEnum)))
                    {
                        if (en == AdequateElementEnum.Unknown)
                            continue;
                        if (excludeValues != null && excludeValues.Contains(en))
                            continue;
                        yield return en;
                    }
                }
            }

            /// <summary>
            /// Introduced for JSON serialization, can create SubModelElements based on a string name
            /// </summary>
            public static SubmodelElement CreateAdequateType(AdequateElementEnum ae, SubmodelElement src = null)
            {
                if (ae == AdequateElementEnum.Property)
                    return new Property(src);
                if (ae == AdequateElementEnum.MultiLanguageProperty)
                    return new MultiLanguageProperty(src);
                if (ae == AdequateElementEnum.Range)
                    return new Range(src);
                if (ae == AdequateElementEnum.File)
                    return new File(src);
                if (ae == AdequateElementEnum.Blob)
                    return new Blob(src);
                if (ae == AdequateElementEnum.ReferenceElement)
                    return new ReferenceElement(src);
                if (ae == AdequateElementEnum.RelationshipElement)
                    return new RelationshipElement(src);
                if (ae == AdequateElementEnum.AnnotatedRelationshipElement)
                    return new AnnotatedRelationshipElement(src);
                if (ae == AdequateElementEnum.Capability)
                    return new Capability(src);
                if (ae == AdequateElementEnum.SubmodelElementCollection)
                    return new SubmodelElementCollection(src);
                if (ae == AdequateElementEnum.Operation)
                    return new Operation(src);
                if (ae == AdequateElementEnum.BasicEvent)
                    return new BasicEvent(src);
                if (ae == AdequateElementEnum.Entity)
                    return new Entity(src);
                if (ae == AdequateElementEnum.SubmodelElementList)
                    return new SubmodelElementList(src);
                if (ae == AdequateElementEnum.SubmodelElementStruct)
                    return new SubmodelElementStruct(src);
                if (ae == AdequateElementEnum.ModelReferenceElement)
                    return new ModelReferenceElement(src);
                if (ae == AdequateElementEnum.GlobalReferenceElement)
                    return new GlobalReferenceElement(src);
                return null;
            }

            /// <summary>
            /// Introduced for JSON serialization, can create SubModelElements based on a string name
            /// </summary>
            /// <param name="elementName">string name (standard PascalCased)</param>
            public static SubmodelElement CreateAdequateType(string elementName)
            {
                return CreateAdequateType(GetAdequateEnum(elementName));
            }

            /// <summary>
            /// Can create SubmodelElements based on a given type information
            /// </summary>
            /// <param name="t">Type of the SME to be created</param>
            /// <returns>SubmodelElement or null</returns>
            public static SubmodelElement CreateAdequateType(Type t)
            {
                if (t == null || !t.IsSubclassOf(typeof(AdminShell.SubmodelElement)))
                    return null;
                var sme = Activator.CreateInstance(t) as SubmodelElement;
                return sme;
            }

            public string GetElementAbbreviation()
            {
                if (submodelElement == null)
                    return ("Null");
                var dsc = submodelElement.GetSelfDescription();
                if (dsc?.ElementAbbreviation == null)
                    return ("Null");
                return dsc.ElementAbbreviation;
            }

            public static string GetElementNameByAdequateType(SubmodelElement sme)
            {
                // access
                var sd = sme.GetSelfDescription();
                if (sd == null || sd.ElementEnum == AdequateElementEnum.Unknown)
                    return null;
                var en = sd.ElementEnum;

                // get the names
                string res = null;
                if ((int)en < AdequateElementNames.Length)
                    res = AdequateElementNames[(int)en].Trim();
                if ((int)en < AdequateElementShortName.Length && AdequateElementShortName[(int)en] != null)
                    res = AdequateElementShortName[(int)en].Trim();
                return res;
            }

            public static ListOfSubmodelElement ListOfWrappersToListOfElems(List<SubmodelElementWrapper> wrappers)
            {
                var res = new ListOfSubmodelElement();
                if (wrappers == null)
                    return res;
                foreach (var w in wrappers)
                    if (w.submodelElement != null)
                        res.Add(w.submodelElement);
                return res;
            }

            public static SubmodelElementWrapper CreateFor(SubmodelElement sme)
            {
                var res = new SubmodelElementWrapper() { submodelElement = sme };
                return res;
            }

            public static Referable FindReferableByReference(
                List<SubmodelElementWrapper> wrappers, ModelReference rf, int keyIndex)
            {
                return FindReferableByReference(wrappers, rf?.Keys, keyIndex);
            }

            public static Referable FindReferableByReference(
                List<SubmodelElementWrapper> wrappers, KeyList rf, int keyIndex)
            {
                // first index needs to exist ..
                if (wrappers == null || rf == null || keyIndex >= rf.Count)
                    return null;

                // as SubmodelElements are not Identifiables, the actual key shall be IdShort
                if (!rf[keyIndex].IsInSubmodelElements())
                    return null;

                // over all wrappers
                foreach (var smw in wrappers)
                    if (smw.submodelElement != null &&
                        smw.submodelElement.idShort.Trim().ToLower() == rf[keyIndex].value.Trim().ToLower())
                    {
                        // match on this level. Did we find a leaf element?
                        if ((keyIndex + 1) >= rf.Count)
                            return smw.submodelElement;

                        // dive into SMC?
                        if (smw.submodelElement is SubmodelElementCollection smc)
                        {
                            var found = FindReferableByReference(smc.value, rf, keyIndex + 1);
                            if (found != null)
                                return found;
                        }

                        // dive into Entity statements?
                        if (smw.submodelElement is Entity ent)
                        {
                            var found = FindReferableByReference(ent.statements, rf, keyIndex + 1);
                            if (found != null)
                                return found;
                        }

                        // else:
                        return null;
                    }

                // no?
                return null;
            }

            // typecasting wrapper into specific type
            public T GetAs<T>() where T : SubmodelElement
            {
                var x = (this.submodelElement) as T;
                return x;
            }

        }

    }
}
