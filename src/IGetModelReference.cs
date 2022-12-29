﻿/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/


//namespace AdminShellNS
namespace AdminShell_V30
{

    #region AdminShell_V3_0

    public partial class AdminShellV30
    {
        /// <summary>
        /// This interface marks an entity, which can provide a ModelReference of itself.
        /// Typically, these are Referables or Identifiables.
        /// </summary>
        public interface IGetModelReference
        {
            ModelReference GetModelReference(bool includeParents = true);
        }

    }

    #endregion
}

