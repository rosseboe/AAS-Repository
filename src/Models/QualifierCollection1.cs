﻿/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

using System.Collections.Generic;

//namespace AdminShell
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        /// <summary>
        ///  This class holds some convenience functions for Qualifiers
        /// </summary>
        public class QualifierCollection : List<Qualifier>
        {
            public QualifierCollection()
            {

            }

#if !DoNotUseAasxCompatibilityModels
            public QualifierCollection(
                List<AasxCompatibilityModels.AdminShellV10.Qualifier> src, bool shallowCopy = false)

            {
                if (src != null && src.Count != 0)
                {
                    foreach (var q in src)
                    {
                        this.Add(new Qualifier(q));
                    }
                }
            }
#endif

            /// <summary>
            /// Add qualifier. If null, do nothing
            /// </summary>
            public new void Add(Qualifier q)
            {
                if (q == null)
                    return;
                base.Add(q);
            }

            public Qualifier FindType(string type)
            {
                if (type == null)
                    return null;
                foreach (var q in this)
                    if (q != null && q.type != null && q.type.Trim() == type.Trim())
                        return q;
                return null;
            }

            public Qualifier FindSemanticId(SemanticId semId)
            {
                if (semId == null)
                    return null;
                foreach (var q in this)
                    if (q != null && q.semanticId != null && q.semanticId.Matches(semId))
                        return q;
                return null;
            }

            // ReSharper disable MethodOverloadWithOptionalParameter .. this seems to work, anyhow
            // ReSharper disable RedundantArgumentDefaultValue
            public string ToString(int format = 0, string delimiter = ";", string referencesDelimiter = ",")
            {
                var res = "";
                foreach (var q in this)
                {
                    if (res != "")
                        res += delimiter;
                    res += q.ToString(format, referencesDelimiter);
                }
                return res;
            }

            public override string ToString()
            {
                return this.ToString(0);
            }
            // ReSharper enable MethodOverloadWithOptionalParameter
            // ReSharper enable RedundantArgumentDefaultValue

            // for convenience methods of Submodel, SubmodelElement

            public static void AddQualifier(
                ref QualifierCollection qualifiers,
                string qualifierType = null, string qualifierValue = null, KeyList semanticKeys = null,
                GlobalReference qualifierValueId = null)
            {
                if (qualifiers == null)
                    qualifiers = new QualifierCollection();
                var q = new Qualifier()
                {
                    type = qualifierType,
                    value = qualifierValue,
                    valueId = qualifierValueId,
                };
                if (semanticKeys != null)
                    q.semanticId = SemanticId.CreateFromKeys(semanticKeys);
                qualifiers.Add(q);
            }

            public static Qualifier HasQualifierOfType(
                QualifierCollection qualifiers,
                string qualifierType)
            {
                if (qualifiers == null || qualifierType == null)
                    return null;
                foreach (var q in qualifiers)
                    if (q.type.Trim().ToLower() == qualifierType.Trim().ToLower())
                        return q;
                return null;
            }

            public IEnumerable<Qualifier> FindAllQualifierType(string qualifierType)
            {
                if (qualifierType == null)
                    yield break;
                foreach (var q in this)
                    if (q.type.Trim().ToLower() == qualifierType.Trim().ToLower())
                        yield return q;
            }
        }

    }

    #endregion
}

