﻿using AasxRestServerLibrary;
using AdminShell_V30;
using AdminShellNS;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Threading;
using System.Timers;
using static AasxDemonstration.EnergyModel;

/*
Copyright (c) 2019-2020 PHOENIX CONTACT GmbH & Co. KG <opensource@phoenixcontact.com>, author: Andreas Orzelski
Copyright (c) 2018-2020 Festo SE & Co. KG <https://www.festo.com/net/de_de/Forms/web/contact_international>, author: Michael Hoffmeister
*/

namespace AasxServerBlazor
{
    public static partial class Program
    {
        public static List<AdminShellPackageEnv> env = new List<AdminShellPackageEnv>();
        public static List<string> envFileName = new List<string>();


        public static string hostPort = "";
        public static string blazorHostPort = "";

        public static WebProxy proxy = null;
        public static HttpClientHandler clientHandler = null;

        public static bool noSecurity = false;
        public static bool edit = false;
        public static string externalRest = "";

        public static HashSet<object> submodelsToPublish = new HashSet<object>();
        public static HashSet<object> submodelsToSubscribe = new HashSet<object>();

        public static Dictionary<object, string> generatedQrCodes = new Dictionary<object, string>();

        public static string redirectServer = "";
        public static string authType = "";

        public static bool isLoading = true;

        public static object changeAasxFile = new object();

        public class NewDataAvailableArgs : EventArgs
        {
            public int signalNewDataMode;

            public NewDataAvailableArgs(int mode = 2)
            {
                signalNewDataMode = mode;
            }
        }

        // 0 == same tree, only values changed
        // 1 == same tree, structure may change
        // 2 == build new tree, keep open nodes
        // 3 == build new tree, all nodes closed
        // public static int signalNewDataMode = 2;
        public static void signalNewData(int mode)
        {
            // signalNewDataMode = mode;
            // NewDataAvailable?.Invoke(null, EventArgs.Empty);
            NewDataAvailable?.Invoke(null, new NewDataAvailableArgs(mode));
        }

        private static int Run()
        {
            // Read root cert from root subdirectory
            Console.WriteLine("Security 1 Startup - Server");
            Console.WriteLine("Security 1.1 Load X509 Root Certificates into X509 Store Root");

            X509Store root = new X509Store("Root", StoreLocation.CurrentUser);
            root.Open(OpenFlags.ReadWrite);

            DirectoryInfo ParentDirectory = new DirectoryInfo(".");

            if (Directory.Exists("./root"))
            {
                foreach (FileInfo f in ParentDirectory.GetFiles("./root/*.cer"))
                {
                    X509Certificate2 cert = new X509Certificate2("./root/" + f.Name);

                    root.Add(cert);
                    Console.WriteLine("Security 1.1 Add " + f.Name);
                }
            }

            Directory.CreateDirectory("./temp");

            string fn = null;
            int envi = 0;

            string[] fileNames = Directory.GetFiles(Directory.GetCurrentDirectory(), "*.aasx");
            Console.WriteLine("Found " + fileNames.Length.ToString() + " AAS in directory " + Directory.GetCurrentDirectory());
            Array.Sort(fileNames);

            while (envi < fileNames.Length)
            {
                fn = fileNames[envi];

                Console.WriteLine("Loading {0}...", fn);
                envFileName.Add(fn);
                env.Add(new AdminShellPackageEnv(fn, true));
                if (env[envi] == null)
                {
                    Console.Error.WriteLine($"Cannot open {fn}. Aborting..");
                    return 1;
                }

                envi++;
            }
            

            RunScript(true);

            isLoading = false;

            SetScriptTimer(1000); // also updates balzor view

            if (env != null)
            {
                foreach (var e in env)
                {
                    if (e?.AasEnv?.Submodels != null)
                    {
                        foreach (var sm in e.AasEnv.Submodels)
                        {
                            if (sm != null)
                            {
                                sm.SetAllParents();
                            }
                        }
                    }
                }
            }

            Console.WriteLine("Servers successfully started. Press Ctrl-C to exit...");
            ManualResetEvent quitEvent = new ManualResetEvent(false);
            try
            {
                Console.CancelKeyPress += (sender, eArgs) =>
                {
                    quitEvent.Set();
                    eArgs.Cancel = true;
                };
            }
            catch
            {
            }

            // wait for timeout or Ctrl-C
            quitEvent.WaitOne(Timeout.Infinite);

           return 0;
        }
              
        public class aasListParameters
        {
            public int index;
            public string idShort;
            public string identification;
            public string fileName;
            public string assetId;
            public string humanEndPoint;
            public string restEndPoint;
        }

        public class aasDirectoryParameters
        {
            public string source;
            public List<aasListParameters> aasList;
            public aasDirectoryParameters()
            {
                aasList = new List<aasListParameters> { };
            }
        }

        public class TransmitData
        {
            public string source;
            public string destination;
            public string type;
            public string encrypt;
            public string extensions;
            public List<string> publish;
            public TransmitData()
            {
                publish = new List<string> { };
            }
        }

        public class TransmitFrame
        {
            public string source;
            public List<TransmitData> data;
            public TransmitFrame()
            {
                data = new List<TransmitData> { };
            }
        }

        public static event EventHandler NewDataAvailable;

        public enum TreeUpdateMode
        {
            ValuesOnly = 0,
            Rebuild,
            RebuildAndCollapse
        }

        public static void SignalNewData(TreeUpdateMode mode)
        {
            NewDataAvailable?.Invoke(mode, EventArgs.Empty);
        }

        private static System.Timers.Timer scriptTimer;

        private static void SetScriptTimer(double value)
        {
            // Create a timer with a two second interval.
            scriptTimer = new System.Timers.Timer(value);

            // Hook up the Elapsed event for the timer.
            scriptTimer.Elapsed += OnScriptTimedEvent;
            scriptTimer.AutoReset = true;
            scriptTimer.Enabled = true;
        }

        private static void OnScriptTimedEvent(Object source, ElapsedEventArgs e)
        {
            RunScript(false);
        }

        static void RunScript(bool init)
        {
            if (env == null)
                return;

            lock (changeAasxFile)
            {
                int i = 0;
                while ((env.Count < i) && (env[i] != null))
                {
                    foreach (var sm in env[i].AasEnv.Submodels)
                    {
                        if (sm != null && sm.idShort != null)
                        {
                            int count = sm.qualifiers != null ? sm.qualifiers.Count : 0;
                            if (count != 0)
                            {
                                var q = sm.qualifiers[0] as AdminShell.Qualifier;
                                if (q.type == "SCRIPT")
                                {
                                    // Triple
                                    // Reference to property with Number
                                    // Reference to submodel with numbers/strings
                                    // Reference to property to store found text
                                    count = sm.submodelElements.Count;
                                    int smi = 0;
                                    while (smi < count)
                                    {
                                        var sme1 = sm.submodelElements[smi++].submodelElement;
                                        if (sme1.qualifiers.Count == 0)
                                        {
                                            continue;
                                        }
                                        var qq = sme1.qualifiers[0] as AdminShell.Qualifier;

                                        if (qq.type == "Add")
                                        {
                                            int v = Convert.ToInt32((sme1 as AdminShell.Property).value);
                                            v += Convert.ToInt32(qq.value);
                                            (sme1 as AdminShell.Property).value = v.ToString();
                                            continue;
                                        }

                                        if (qq.type == "GetJSON")
                                        {
                                            if (init)
                                                return;

                                            if (isLoading)
                                                return;

                                            if (!(sme1 is AdminShell.ReferenceElement))
                                            {
                                                continue;
                                            }

                                            string url = qq.value;
                                            string username = "";
                                            string password = "";

                                            if (sme1.qualifiers.Count == 3)
                                            {
                                                qq = sme1.qualifiers[1] as AdminShell.Qualifier;
                                                if (qq.type != "Username")
                                                    continue;
                                                username = qq.value;
                                                qq = sme1.qualifiers[2] as AdminShell.Qualifier;
                                                if (qq.type != "Password")
                                                    continue;
                                                password = qq.value;
                                            }

                                            var handler = new HttpClientHandler();
                                            handler.DefaultProxyCredentials = CredentialCache.DefaultCredentials;
                                            var client = new HttpClient(handler);

                                            if (username != "" && password != "")
                                            {
                                                var authToken = System.Text.Encoding.ASCII.GetBytes(username + ":" + password);
                                                client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic",
                                                        Convert.ToBase64String(authToken));
                                            }

                                            Console.WriteLine("GetJSON: " + url);
                                            string response = client.GetStringAsync(url).Result;
                                            Console.WriteLine(response);

                                            if (response != "")
                                            {
                                                var r12 = sme1 as AdminShell.ReferenceElement;
                                                var ref12 = env[i].AasEnv.FindReferableByReference(r12.GetModelReference());
                                                if (ref12 is AdminShell.SubmodelElementCollection)
                                                {
                                                    var c1 = ref12 as AdminShell.SubmodelElementCollection;
                                                    // if (c1.value.Count == 0)
                                                    {
                                                        // dynamic model = JObject.Parse(response);
                                                        JObject parsed = JObject.Parse(response);
                                                        parseJson(c1, parsed);
                                                    }
                                                }
                                            }
                                            continue;
                                        }

                                        if (qq.type != "SearchNumber" || smi >= count)
                                        {
                                            continue;
                                        }
                                        var sme2 = sm.submodelElements[smi++].submodelElement;
                                        if (sme2.qualifiers.Count == 0)
                                        {
                                            continue;
                                        }
                                        qq = sme2.qualifiers[0] as AdminShell.Qualifier;
                                        if (qq.type != "SearchList" || smi >= count)
                                        {
                                            continue;
                                        }
                                        var sme3 = sm.submodelElements[smi++].submodelElement;
                                        if (sme3.qualifiers.Count == 0)
                                        {
                                            continue;
                                        }
                                        qq = sme3.qualifiers[0] as AdminShell.Qualifier;
                                        if (qq.type != "SearchResult")
                                        {
                                            break;
                                        }
                                        if (sme1 is AdminShell.ReferenceElement &&
                                            sme2 is AdminShell.ReferenceElement &&
                                            sme3 is AdminShell.ReferenceElement)
                                        {
                                            var r1 = sme1 as AdminShell.ReferenceElement;
                                            var r2 = sme2 as AdminShell.ReferenceElement;
                                            var r3 = sme3 as AdminShell.ReferenceElement;
                                            var ref1 = env[i].AasEnv.FindReferableByReference(r1.GetModelReference());
                                            var ref2 = env[i].AasEnv.FindReferableByReference(r2.GetModelReference());
                                            var ref3 = env[i].AasEnv.FindReferableByReference(r3.GetModelReference());
                                            if (ref1 is AdminShell.Property && ref2 is AdminShell.Submodel && ref3 is AdminShell.Property)
                                            {
                                                var p1 = ref1 as AdminShell.Property;
                                                // Simulate changes
                                                var sm2 = ref2 as AdminShell.Submodel;
                                                var p3 = ref3 as AdminShell.Property;
                                                int count2 = sm2.submodelElements.Count;
                                                for (int j = 0; j < count2; j++)
                                                {
                                                    var sme = sm2.submodelElements[j].submodelElement;
                                                    if (sme.idShort == p1.value)
                                                    {
                                                        p3.value = (sme as AdminShell.Property).value;
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                    i++;
                }
            }

            return;
        }

        public static void parseJson(AdminShell.SubmodelElementCollection c, JObject o)
        {
            TreeUpdateMode newMode = TreeUpdateMode.ValuesOnly;
            DateTime timeStamp = DateTime.UtcNow;

            foreach (JProperty jp1 in (JToken)o)
            {
                AdminShell.SubmodelElementCollection c2;
                switch (jp1.Value.Type)
                {
                    case JTokenType.Array:
                        c2 = c.value.FindFirstIdShortAs<AdminShell.SubmodelElementCollection>(jp1.Name);
                        if (c2 == null)
                        {
                            c2 = AdminShell.SubmodelElementCollection.CreateNew(jp1.Name);
                            c.Add(c2);
                            c2.TimeStampCreate = timeStamp;
                            c2.setTimeStamp(timeStamp);
                            newMode = TreeUpdateMode.Rebuild;
                        }
                        int count = 1;
                        foreach (JObject el in jp1.Value)
                        {
                            string n = jp1.Name + "_array_" + count++;
                            AdminShell.SubmodelElementCollection c3 =
                                c2.value.FindFirstIdShortAs<AdminShell.SubmodelElementCollection>(n);
                            if (c3 == null)
                            {
                                c3 = AdminShell.SubmodelElementCollection.CreateNew(n);
                                c2.Add(c3);
                                c3.TimeStampCreate = timeStamp;
                                c3.setTimeStamp(timeStamp);
                                newMode = TreeUpdateMode.Rebuild;
                            }
                            parseJson(c3, el);
                        }
                        break;
                    case JTokenType.Object:
                        c2 = c.value.FindFirstIdShortAs<AdminShell.SubmodelElementCollection>(jp1.Name);
                        if (c2 == null)
                        {
                            c2 = AdminShell.SubmodelElementCollection.CreateNew(jp1.Name);
                            c.Add(c2);
                            c2.TimeStampCreate = timeStamp;
                            c2.setTimeStamp(timeStamp);
                            newMode = TreeUpdateMode.Rebuild;
                        }
                        foreach (JObject el in jp1.Value)
                        {
                            parseJson(c2, el);
                        }
                        break;
                    default:
                        AdminShell.Property p = c.value.FindFirstIdShortAs<AdminShell.Property>(jp1.Name);
                        if (p == null)
                        {
                            p = AdminShell.Property.CreateNew(jp1.Name);
                            c.Add(p);
                            p.TimeStampCreate = timeStamp;
                            p.setTimeStamp(timeStamp);
                            newMode = TreeUpdateMode.Rebuild;
                        }
                        // see https://github.com/JamesNK/Newtonsoft.Json/issues/874
                        p.value = (jp1.Value as JValue).ToString(CultureInfo.InvariantCulture);
                        p.setTimeStamp(timeStamp);
                        break;
                }
            }

            SignalNewData(newMode);
        }
    }
}

