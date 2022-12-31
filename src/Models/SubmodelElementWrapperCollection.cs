﻿/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/



//namespace AdminShell
namespace AdminShell_V30
{
    public partial class AdminShellV30
    {
        public class SubmodelElementWrapperCollection : BaseSubmodelElementWrapperCollection<SubmodelElement>
        {
            public SubmodelElementWrapperCollection() : base() { }

            public SubmodelElementWrapperCollection(SubmodelElementWrapper smw) : base(smw) { }

            public SubmodelElementWrapperCollection(SubmodelElement sme) : base(sme) { }

            public SubmodelElementWrapperCollection(SubmodelElementWrapperCollection other) : base(other) { }

        }

    }
}
