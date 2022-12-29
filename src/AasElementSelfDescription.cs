﻿/*
Copyright (c) 2018-2021 Festo AG & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>
Author: Michael Hoffmeister

This source code is licensed under the Apache License 2.0 (see LICENSE.txt).

This source code may use other Open Source software components (see LICENSE.txt).
*/

namespace AasxCompatibilityModels
{
    #region AdminShell_V2_0

    public partial class AdminShellV20
    {
        public class AasElementSelfDescription
        {
            public string ElementName = "";
            public string ElementAbbreviation = "";

            public AasElementSelfDescription() { }

            public AasElementSelfDescription(string ElementName, string ElementAbbreviation)
            {
                this.ElementName = ElementName;
                this.ElementAbbreviation = ElementAbbreviation;
            }
        }



    }

    #endregion
}

