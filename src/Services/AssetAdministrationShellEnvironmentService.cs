﻿using AdminShell;
using AdminShell;
using AdminShell;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using static AdminShell_V30.AdminShellV30;
using static IO.Swagger.V1RC03.Controllers.AssetAdministrationShellEnvironmentAPIController;
using Reference = AdminShell_V30.AdminShellV30.Reference;
using Submodel = AdminShell_V30.AdminShellV30.Submodel;
using SubmodelElement = AdminShell_V30.AdminShellV30.SubmodelElement;

namespace IO.Swagger.V1RC03.Services
{
    public class AssetAdministrationShellEnvironmentService : IAssetAdministrationShellEnvironmentService
    {
        private readonly ILogger<AssetAdministrationShellEnvironmentService> _logger;
        private AdminShellPackageEnv[] _packages;
        private AasSecurityContext _securityContext;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="logger"></param>
        public AssetAdministrationShellEnvironmentService(ILogger<AssetAdministrationShellEnvironmentService> logger)
        {
            _logger = logger;
            _packages = Program.env.ToArray();
        }

        public void SecurityCheck(string objPath = "", string aasOrSubmodel = null, object objectAasOrSubmodel = null)
        {
            string currentRole = _securityContext.accessRights;
            string neededRights = _securityContext.neededRights;
            
            if (neededRights == "READ")
            {
                return;
            }

            if ((neededRights == "UPDATE" || neededRights == "DELETE") && currentRole == "UPDATE")
            {
                return;
            }
            
            throw new Exception("Unauthorized!");
        }

        #region AssetAdministrationShell

        public void UpdateFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, string fileName, string contentType, Stream fileContent)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateFileByPathSubmodelRepo(submodelIdentifier, idShortPath, fileName, contentType, fileContent);
                }
            }
        }

        public void UpdateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.idShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateSubmodelElementByPathSubmodelRepo(body, submodelIdentifier, idShortPath);
                }
            }
        }

        public void UpdateSubmodel(Submodel body, string aasIdentifier, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.idShort))
            {
                throw new Exception("Submodel");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    UpdateSubmodelById(body, submodelIdentifier);
                }
            }
        }

        private bool IsSubmodelPresentInAAS(AssetAdministrationShell aas, string submodelIdentifier)
        {
            if (aas.Submodels.Any(s => s.ToString() == submodelIdentifier))
            {
                return true;
            }
            else
            {
                throw new Exception($"SubmodelReference with id {submodelIdentifier} not found in AAS with id {aas.Identification}");
            }
        }

        public void UpdateAssetInformation(Models.AssetInformation body, string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                aas.AssetInformation = body;
                Program.signalNewData(1);
            }
        }

        public void UpdateAssetAdministrationShellById(AssetAdministrationShell body, string aasIdentifier)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
                throw new Exception("AssetAdministrationShell");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out int packageIndex);
            if (aas != null && packageIndex != -1)
            {
                _packages[packageIndex].AasEnv.AdministrationShells.Remove(aas);
                _packages[packageIndex].AasEnv.AdministrationShells.Add(body);
                Program.signalNewData(1);
            }
        }

        public Reference CreateSubmodelReference(Reference body, string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);

            if (aas != null)
            {
                var found = aas.Submodels.Any(s => s.Matches(body));
                if (found)
                {
                    throw new Exception($"Requested submodel reference already exists in AAS with id {aasIdentifier}");
                }
                else
                {
                    aas.Submodels ??= new List<Reference>();
                    aas.Submodels.Add(body);
                    return body;
                }
            }

            return null;
        }

        public SubmodelElement CreateSubmodelElementByPath(SubmodelElement body, string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return CreateSubmodelElementByPathSubmodelRepo(body, submodelIdentifier, idShortPath);
                }
            }

            return null;
        }

        public SubmodelElement CreateSubmodelElement(SubmodelElement body, string aasIdentifier, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return CreateSubmodelElementSubmodelRepo(body, submodelIdentifier);
                }
            }

            return null;
        }

        public AssetAdministrationShell CreateAssetAdministrationShell(AssetAdministrationShell body)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
                throw new Exception("AssetAdministrationShell");
            }

            //Check if AAS exists
            var found = IsAssetAdministrationShellPresent(body.Identification, out _, out _);
            if (found)
            {
                throw new Exception($"AssetAdministrationShell with id {body.Identification} already exists.");
            }

            if (EmptyPackageAvailable(out int emptyPackageIndex))
            {

                _packages[emptyPackageIndex].AasEnv.AdministrationShells.Add(body);
                Program.signalNewData(2);
                return _packages[emptyPackageIndex].AasEnv.AdministrationShells[0]; //Considering it is being added to empty package.
            }
            else
            {
                throw new Exception("No empty environment package available in the server.");
            }
        }



        public OperationResult GetOperationAsyncResult(string aasIdentifier, string submodelIdentifier, string idShortPath, string handleId)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return GetOperationAsyncResultSubmodelRepo(submodelIdentifier, idShortPath, handleId);
                }
            }

            return null;
        }

        public string GetFileByPath(string aasIdentifier, string submodelIdentifier, string idShortPath, out byte[] content, out long fileSize)
        {
            content = null;
            fileSize = 0;
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    return GetFileByPathSubmodelRepo(submodelIdentifier, idShortPath, out content, out fileSize);
                }
            }

            return null;
        }

        public void DeleteSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    DeleteSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath);
                }
            }
        }

        public SubmodelElement GetSubmodelElementByPath(string aasIdentifier, string submodelIdentifier, string idShortPath)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                if (IsSubmodelPresentInAAS(aas, submodelIdentifier))
                {
                    var output = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);
                    return output;
                }
            }

            return null;
        }

        public Submodel GetSubmodel(string aasIdentifier, string submodelIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                var submodelRefs = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if (submodelRefs.Any())
                {
                    return GetSubmodelById(submodelIdentifier, out _);
                }
                else
                {
                    throw new Exception($"SubmodelReference with id {submodelIdentifier} not found in AAS with id {aasIdentifier}");
                }
            }

            return null;
        }

        public void DeleteSubmodelReferenceById(string aasIdentifier, string submodelIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                var submodelRefs = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if (submodelRefs.Any())
                {
                    aas.Submodels.Remove(submodelRefs.First());
                    Program.signalNewData(1);
                }
                else
                {
                    throw new Exception($"SubmodelReference with id {submodelIdentifier} not found in AAS with id {aasIdentifier}");
                }
            }
        }

        public void DeleteAssetAdministrationShellById(string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out int packageIndex);
            if ((aas != null) && (packageIndex != -1))
            {
                _packages[packageIndex].AasEnv.AdministrationShells.Remove(aas);
                if (_packages[packageIndex].AasEnv.AdministrationShells.Count == 0)
                {
                    _packages[packageIndex] = null;             //TODO: jtikekar what about submodels?
                }
                Program.signalNewData(2);
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public AssetInformation GetAssetInformationFromAas(string aasIdentifier)
        {
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                return aas.AssetInformation;
            }

            return null;
        }

        public List<Reference> GetAllSubmodelReferences(string decodedAasId)
        {
            var aas = GetAssetAdministrationShellById(decodedAasId, out _);

            if (aas != null)
            {
                return aas.Submodels;
            }

            return null;
        }

        /// <summary>
        /// Retrieves all AASs from the server
        /// </summary>
        /// <returns></returns>
        public List<AssetAdministrationShell> GetAllAssetAdministrationShells(List<string> assetIds = null, string idShort = null)
        {
            var output = new List<AssetAdministrationShell>();

            //Get All AASs
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        output.AddRange(env.AdministrationShells);
                    }
                }
            }

            if (output.Any())
            {
                //Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    output = output.Where(a => a.IdShort.Equals(idShort)).ToList();
                    if ((output == null) || output?.Count == 0)
                    {
                        throw new Exception($"AssetAdministrationShells with IdShort {idShort} Not Found.");
                    }
                }

                //Filter based on AssetId
                if (assetIds != null && assetIds.Count != 0)
                {
                    var aasList = new List<AssetAdministrationShell>();
                    foreach (var assetId in assetIds)
                    {
                        aasList.AddRange(output.Where(a => a.AssetInformation.SpecificAssetIds.Contains(assetId)).ToList());
                    }

                    if (aasList.Any())
                    {
                        return aasList;
                    }
                    else
                    {
                        throw new Exception($"AssetAdministrationShells with requested SpecificAssetIds Not Found.");
                    }
                }
            }

            return output;
        }

        public object GetAllSubmodelElements(string aasIdentifier, string submodelIdentifier)
        {
            object output = null;
            //Find AAS
            var aas = GetAssetAdministrationShellById(aasIdentifier, out _);
            if (aas != null)
            {
                //Check if AAS consist the requested submodel
                IEnumerable<Reference> references = aas.Submodels.Where(s => s.Matches(submodelIdentifier));
                if ((references == null) || (references?.Count() == 0))
                {
                    throw new Exception($"Requested submodel: {submodelIdentifier} not found in AAS: {aasIdentifier}");
                }

                output = GetAllSubmodelElementsFromSubmodel(submodelIdentifier);
            }

            return output;
        }

        public AssetAdministrationShell GetAssetAdministrationShellById(string aasIdentifier, out int packageIndex)
        {
            bool found = IsAssetAdministrationShellPresent(aasIdentifier, out AssetAdministrationShell output, out packageIndex);

            if (found)
            {
                SecurityCheck("", "aas", output);

                return output;
            }
            else
            {
                throw new Exception($"AssetAdministrationShell with id {aasIdentifier} not found.");
            }
        }

        private bool IsAssetAdministrationShellPresent(string aasIdentifier, out AssetAdministrationShell output, out int packageIndex)
        {
            output = null; packageIndex = -1;
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        var aas = env.AdministrationShells.Where(a => a.id.Equals(aasIdentifier));
                        if (aas.Any())
                        {
                            output = aas.First();
                            packageIndex = Array.IndexOf(_packages, package);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        #endregion

        #region ConceptDescription

        public void UpdateConceptDescriptionById(ConceptDescription body, string cdIdentifier)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("ConceptDescription");
            }

            var conceptDescription = GetConceptDescriptionById(cdIdentifier, out int packageIndex);
            if (conceptDescription != null)
            {
                int cdIndex = _packages[packageIndex].AasEnv.ConceptDescriptions.IndexOf(conceptDescription);
                _packages[packageIndex].AasEnv.ConceptDescriptions.Remove(conceptDescription);
                _packages[packageIndex].AasEnv.ConceptDescriptions.Insert(cdIndex, body);
                Program.signalNewData(0);
            }
        }

        public ConceptDescription CreateConceptDescription(ConceptDescription body)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("ConceptDescription");
            }

            //Check if AAS exists
            var found = IsConceptDescriptionPresent(body.Id, out _, out _);
            if (found)
            {
                throw new Exception($"ConceptDescription with id {body.Id} already exists.");
            }

            if (EmptyPackageAvailable(out int emptyPackageIndex))
            {

                _packages[emptyPackageIndex].AasEnv.ConceptDescriptions.Add(body);
                Program.signalNewData(2);
                return _packages[emptyPackageIndex].AasEnv.ConceptDescriptions[0]; //Considering it is being added to empty package.
            }
            else
            {
                throw new Exception("No empty environment package available in the server.");
            }
        }

        public void DeleteConceptDescriptionById(string cdIdentifier)
        {
            var conceptDescription = GetConceptDescriptionById(cdIdentifier, out int packageIndex);
            if ((conceptDescription != null) && (packageIndex != -1))
            {
                _packages[packageIndex].AasEnv.ConceptDescriptions.Remove(conceptDescription);
                Program.signalNewData(1);
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public ConceptDescription GetConceptDescriptionById(string cdIdentifier, out int packageIndex)
        {
            bool found = IsConceptDescriptionPresent(cdIdentifier, out ConceptDescription output, out packageIndex);
            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"ConceptDescription with id {cdIdentifier} not found.");
            }
        }

        private bool IsConceptDescriptionPresent(string cdIdentifier, out ConceptDescription output, out int packageIndex)
        {
            output = null;
            packageIndex = -1;
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        var conceptDescriptions = env.ConceptDescriptions.Where(c => c.id.Equals(cdIdentifier));
                        if (conceptDescriptions.Any())
                        {
                            output = conceptDescriptions.First();
                            packageIndex = Array.IndexOf(_packages, package);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Retrieves all concept descriptions
        /// </summary>
        /// <param name="idShort"></param>
        /// <param name="reqIsCaseOf"></param>
        /// <param name="reqDataSpecificationRef"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public List<ConceptDescription> GetAllConceptDescriptions(string idShort = null, Reference reqIsCaseOf = null, Reference reqDataSpecificationRef = null)
        {
            var output = new List<ConceptDescription>();

            //Get All Concept descriptions
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        output.AddRange(env.ConceptDescriptions);
                    }
                }
            }

            if (output.Any())
            {
                //Filter AASs based on IdShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    var cdList = output.Where(cd => cd.IdShort.Equals(idShort)).ToList();
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with IdShort {idShort} Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }
                }

                //Filter based on IsCaseOf
                if (reqIsCaseOf != null)
                {
                    var cdList = new List<ConceptDescription>();
                    foreach (var conceptDescription in output)
                    {
                        if (conceptDescription.IsCaseOf?.Count > 0)
                        {
                            foreach (var reference in conceptDescription.IsCaseOf)
                            {
                                if (reference != null && reference.Matches(reqIsCaseOf))
                                {
                                    cdList.Add(conceptDescription);
                                    break;
                                }
                            }
                        }
                    }
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with requested IsCaseOf Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }

                }

                //Filter based on DataSpecificationRef
                if (reqDataSpecificationRef != null)
                {
                    var cdList = new List<ConceptDescription>();
                    foreach (var conceptDescription in output)
                    {
                        if (conceptDescription.DataSpecifications?.Count > 0)
                        {
                            foreach (var reference in conceptDescription.DataSpecifications)
                            {
                                if (reference != null && reference.Matches(reqDataSpecificationRef))
                                {
                                    cdList.Add(conceptDescription);
                                    break;
                                }
                            }
                        }
                    }
                    if ((cdList == null) || cdList?.Count == 0)
                    {
                        throw new Exception($"Concept Description with requested DataSpecificationReference Not Found.");
                    }
                    else
                    {
                        output = cdList;
                    }
                }
            }

            return output;
        }

        #endregion


        #region Submodel

        public void UpdateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath = null)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var submodelElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out object smeParent);
            if (submodelElement != null && smeParent != null)
            {
                {
                    if (smeParent is SubmodelElementCollection collection)
                    {
                        var smeIndex = collection.Value.IndexOf(submodelElement);
                        collection.Value.Remove(submodelElement);
                        collection.Value.Insert(smeIndex, body);
                    }
                    else if (smeParent is SubmodelElementList list)
                    {
                        var smeIndex = list.Value.IndexOf(submodelElement);
                        list.Value.Remove(submodelElement);
                        list.Value.Insert(smeIndex, body);
                    }
                    //Added support for submodel here, as no other api found for this functionality
                    else if (smeParent is Submodel submodel)
                    {
                        var smeIndex = submodel.SubmodelElements.IndexOf(submodelElement);
                        submodel.SubmodelElements.Remove(submodelElement);
                        submodel.SubmodelElements.Insert(smeIndex, body);
                    }
                }


                Program.signalNewData(1);
            }
        }

        public void UpdateSubmodelById(Submodel body, string submodelIdentifier = null)
        {
            if (string.IsNullOrEmpty(body.Identification))
            {
               throw new Exception("Submodel");
            }

            var submodel = GetSubmodelById(submodelIdentifier, out int packageIndex);
            if (submodel != null)
            {
                _packages[packageIndex].AasEnv.Submodels.Remove(submodel);
                _packages[packageIndex].AasEnv.Submodels.Add(body);
                Program.signalNewData(1);
            }
        }

        public SubmodelElement CreateSubmodelElementByPathSubmodelRepo(SubmodelElement body, string submodelIdentifier, string idShortPath)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var newIdShortPath = idShortPath + "." + body.IdShort;
            var found = IsSubmodelElementPresent(submodelIdentifier, newIdShortPath, out _, out object smeParent);
            if (found)
            {
                throw new Exception($"SubmodelElement with IdShort {body.IdShort} already exists.");
            }
            else
            {
                if (smeParent != null && smeParent is Submodel submodel)
                {
                    submodel.SubmodelElements ??= new List<SubmodelElement>();

                    submodel.SubmodelElements.Add(body);

                    body.Parent = submodel;
                }
                else if (smeParent != null && smeParent is SubmodelElementCollection collection)
                {
                    collection.Value ??= new List<SubmodelElement>();

                    collection.Value.Add(body);

                    body.Parent = collection;
                }
                else if (smeParent != null && smeParent is SubmodelElementList list)
                {
                    list.Value ??= new List<SubmodelElement>();

                    list.Value.Add(body);

                    body.Parent = list;
                }
                else if (smeParent != null && smeParent is Entity entity)
                {
                    entity.Statements ??= new List<SubmodelElement>();

                    entity.Statements.Add(body);
                    body.Parent = entity;
                }
                else if (smeParent != null && smeParent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    annotatedRelationshipElement.Annotations ??= new List<IDataElement>();

                    annotatedRelationshipElement.Annotations.Add((IDataElement)body);
                    body.Parent = annotatedRelationshipElement;
                }

                Program.signalNewData(1);

                return body;
            }
        }

        public SubmodelElement CreateSubmodelElementSubmodelRepo(SubmodelElement body, string submodelIdentifier)
        {
            if (string.IsNullOrEmpty(body.IdShort))
            {
                throw new Exception("SubmodelElement");
            }

            var found = IsSubmodelElementPresent(submodelIdentifier, body.IdShort, out _, out object smeParent);
            if (found)
            {
                throw new Exception($"SubmodelElement with IdShort {body.IdShort} already exists.");
            }
            else
            {
                if (smeParent != null && smeParent is Submodel submodel)
                {
                    submodel.SubmodelElements ??= new List<SubmodelElement>();

                    submodel.SubmodelElements.Add(body);

                    body.Parent = submodel;

                    Program.signalNewData(1);

                    return body;
                }
            }

            return null;

        }

        public Submodel CreateSubmodel(Submodel body)
        {
            if (string.IsNullOrEmpty(body.Id))
            {
                throw new Exception("Submodel");
            }

            //Check if AAS exists
            var found = IsSubmodelPresent(body.Id, out _, out _);
            if (found)
            {
                throw new Exception($"Submodel with id {body.Id} already exists.");
            }

            if (EmptyPackageAvailable(out int emptyPackageIndex))
            {

                _packages[emptyPackageIndex].AasEnv.Submodels.Add(body);
                Program.signalNewData(2);
                return _packages[emptyPackageIndex].AasEnv.Submodels[0]; //Considering it is being added to empty package.
            }
            else
            {
                throw new Exception("No empty environment package available in the server.");
            }
        }

        public Submodel GetSubmodelById(string submodelIdentifier, out int packageIndex)
        {
            bool found = IsSubmodelPresent(submodelIdentifier, out Submodel output, out packageIndex);
            if (found)
            {
                // SecurityCheck(output.IdShort, "submodel", output);

                return output;
            }
            else
            {
                throw new Exception($"Submodel with id {submodelIdentifier} not found.");
            }
        }

        private bool IsSubmodelPresent(string submodelIdentifier, out Submodel output, out int packageIndex)
        {
            output = null;
            packageIndex = -1;
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        var submodels = env.Submodels.Where(a => a.id.Equals(submodelIdentifier));
                        if (submodels.Any())
                        {
                            output = submodels.First();
                            packageIndex = Array.IndexOf(_packages, package);
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        private List<SubmodelElement> filterSubmodelElements(Submodel submodel, List<SubmodelElement> output, DateTime diff = new DateTime())
        {
            List<SubmodelElement> filtered = new List<SubmodelElement>();
            if (output != null)
            {
                foreach (var o in output)
                {
                    if (o.TimeStampTree >= diff)
                    {
                        if (SecurityCheckTestOnly(submodel.IdShort + "." + o.IdShort, "submodel", submodel))
                            filtered.Add(o);
                        // if further iteration into is needed
                        /*
                        if (o is SubmodelElementCollection sc)
                        {
                            if (o.TimeStamp >= diff)
                                filtered.Add(o);
                            filtered.AddRange(filterSubmodelElements(submodel, sc.Value, diff));
                        }
                        else
                        if (o is SubmodelElementList sl)
                        {
                            if (o.TimeStamp >= diff)
                                filtered.Add(o);
                            filtered.AddRange(filterSubmodelElements(submodel, sl.Value, diff));
                        }
                        else
                            if (o.TimeStamp >= diff)
                                filtered.Add(o);
                        */
                    }
                }
            }

            return filtered;
        }
        public object GetAllSubmodelElementsFromSubmodel(string submodelIdentifier = null)
        {
            object output = null;
            //Find Submodel
            var submodel = GetSubmodelById(submodelIdentifier, out _);

            if (submodel == null)
                return null;

            output = submodel.SubmodelElements;
            if (outputModifierContext == null)
            {
                SecurityCheck(submodel.IdShort, "submodel", submodel);
            }
            else
            {
                if (!outputModifierContext.Content.Equals("path", StringComparison.OrdinalIgnoreCase))
                {
                    output = filterSubmodelElements(submodel, submodel.SubmodelElements, outputModifierContext.Diff);
                }
                else
                {
                    //Need to handle this here it self, to append idShort of Submodel to every SME in the list.
                    //level = core, then indirect children should be avoided.
                    //Hence, setting IncludeChildren = false here itself.
                    if (outputModifierContext.Level.Equals("core", StringComparison.OrdinalIgnoreCase))
                    {
                        outputModifierContext.IncludeChildren = false;
                    }
                    foreach (var submodelElement in submodel.SubmodelElements)
                    {
                        if (submodelElement.TimeStamp >= outputModifierContext.Diff || submodelElement.TimeStampTree >= outputModifierContext.Diff)
                        {
                            outputModifierContext.ParentPath = submodel.IdShort;
                            outputModifierContext.submodel = submodel;
                            outputModifierContext.aasEnvService = this;
                            PathSerializer.ToIdShortPath(submodelElement, outputModifierContext);
                        }
                    }

                    return outputModifierContext.IdShortPaths;
                }
            }

            return output;
        }

        public List<Submodel> GetAllSubmodels(Reference reqSemanticId = null, string idShort = null)
        {
            List<Submodel> output = new List<Submodel>();

            //Get All Submodels
            foreach (var package in _packages)
            {
                if (package != null)
                {
                    var env = package.AasEnv;
                    if (env != null)
                    {
                        foreach (var s in env.Submodels)
                        {
                            if (SecurityCheckTestOnly(s.idShort, "", s))
                                output.Add(s);
                        }
                    }
                }
            }

            //Apply filters
            if (output.Any())
            {
                //Filter w.r.t idShort
                if (!string.IsNullOrEmpty(idShort))
                {
                    var submodels = output.Where(s => s.IdShort.Equals(idShort)).ToList();
                    if ((submodels == null) || (submodels?.Count == 0))
                    {
                        _logger.LogInformation($"Submodels with IdShort {idShort} Not Found.");
                    }

                    output = submodels;
                }

                //Filter w.r.t. SemanticId
                if (reqSemanticId != null)
                {
                    if (output.Any())
                    {
                        var submodels = output.Where(s => s.SemanticId.Matches(reqSemanticId)).ToList();
                        if ((submodels == null) || submodels?.Count == 0)
                        {
                            _logger.LogInformation($"Submodels with requested SemnaticId Not Found.");
                        }

                        output = submodels;
                    }

                }
            }

            return output;
        }

        public void DeleteSubmodelById(string submodelIdentifier)
        {
            var submodel = GetSubmodelById(submodelIdentifier, out int packageIndex);
            if ((submodel != null) && (packageIndex != -1))
            {
                _packages[packageIndex].AasEnv.Submodels.Remove(submodel);

                //Delete submodel reference from AAS
                foreach (var aas in _packages[packageIndex].AasEnv.AdministrationShells)
                {
                    DeleteSubmodelReferenceById(aas.id, submodelIdentifier);
                }

                Program.signalNewData(1);  //TODO jtikekar : may be not needed
            }
            else
            {
                throw new Exception("Unexpected error occurred.");
            }
        }

        public SubmodelElement GetSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out object smeParent)
        {
            bool found = IsSubmodelElementPresent(submodelIdentifier, idShortPath, out SubmodelElement output, out smeParent);

            if (found)
            {
                return output;
            }
            else
            {
                throw new Exception($"Requested submodel element {idShortPath} NOT found.");
            }
        }

        private bool IsSubmodelElementPresent(string submodelIdentifier, string idShortPath, out SubmodelElement output, out object smeParent)
        {
            output = null;
            smeParent = null;
            var submodel = GetSubmodelById(submodelIdentifier, out _);

            if (submodel != null)
            {
                SecurityCheck(submodel.IdShort + "." + idShortPath, "", submodel);

                output = GetSubmodelElementByPath(submodel, idShortPath, out object parent);
                smeParent = parent;
                if (output != null)
                {
                    return true;
                }

            }

            return false;
        }

        //TODO:jtikekar refactor
        private SubmodelElement GetSubmodelElementByPath(object parent, string idShortPath, out object outParent)
        {
            outParent = parent;
            if (idShortPath.Contains('.'))
            {
                string[] idShorts = idShortPath.Split('.', 2);
                if (parent is Submodel submodel)
                {
                    var submodelElement = submodel.FindSubmodelElementByIdShort(idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is SubmodelElementCollection collection)
                {
                    var submodelElement = collection.FindFirstIdShortAs<SubmodelElement>(idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is SubmodelElementList list)
                {
                    var submodelElement = list.FindFirstIdShortAs<SubmodelElement>(idShorts[0]);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is Entity entity)
                {
                    var submodelElement = entity.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else if (parent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    var submodelElement = annotatedRelationshipElement.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return GetSubmodelElementByPath(submodelElement, idShorts[1], out outParent);
                    }
                }
                else
                {
                    throw new Exception($"Parent of type {parent.GetType()} not supported.");
                }
            }
            else
            {
                if (parent is Submodel submodel)
                {
                    var submodelElement = submodel.FindSubmodelElementByIdShort(idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is SubmodelElementCollection collection)
                {
                    var submodelElement = collection.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is SubmodelElementList list)
                {
                    var submodelElement = list.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is Entity entity)
                {
                    var submodelElement = entity.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else if (parent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    var submodelElement = annotatedRelationshipElement.FindFirstIdShortAs<SubmodelElement>(idShortPath);
                    if (submodelElement != null)
                    {
                        return submodelElement;
                    }
                }
                else
                {
                    throw new Exception($"Parent of type {parent.GetType()} not supported.");
                }
            }
            return null;
        }

        public void DeleteSubmodelElementByPathSubmodelRepo(string submodelIdentifier, string idShortPath)
        {
            var submodelElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out object smeParent);
            if (submodelElement != null)
            {
                if (smeParent is SubmodelElementCollection parentCollection)
                {
                    parentCollection.Value.Remove(submodelElement);
                }
                else if (smeParent is SubmodelElementList parentList)
                {
                    parentList.Value.Remove(submodelElement);
                }
                else if (smeParent is AnnotatedRelationshipElement annotatedRelationshipElement)
                {
                    annotatedRelationshipElement.Annotations.Remove((IDataElement)submodelElement);
                }
                else if (smeParent is Entity entity)
                {
                    entity.Statements.Remove(submodelElement);
                }
                else if (smeParent is Submodel parentSubmodel)
                {
                    parentSubmodel.SubmodelElements.Remove(submodelElement);
                }

                Program.signalNewData(1);
            }
        }

        public string GetFileByPathSubmodelRepo(string submodelIdentifier, string idShortPath, out byte[] byteArray, out long fileSize)
        {
            byteArray = null;
            string fileName = null;
            fileSize = 0;

            var submodel = GetSubmodelById(submodelIdentifier, out int packageIndex);

            var fileElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (fileElement != null)
            {
                SecurityCheck(submodel.IdShort + "." + idShortPath);

                if (fileElement is System.IO.File file)
                {
                    fileName = file.Value;

                    Stream stream = _packages[packageIndex].GetLocalStreamFromPackage(fileName);
                    byteArray = stream.ToByteArray();
                    fileSize = byteArray.Length;
                }
                else
                {
                    throw new Exception($"Submodel element {fileElement.idShort} is not of type File.");
                }
            }

            return fileName;
        }

        public void UpdateFileByPathSubmodelRepo(string submodelIdentifier, string idShortPath, string fileName, string contentType, Stream fileContent)
        {
            _ = GetSubmodelById(submodelIdentifier, out int packageIndex);

            var fileElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);
            if (fileElement != null)
            {
                if (fileElement is System.IO.File file)
                {
                    var sourcePath = Path.GetDirectoryName(file.Value);
                    var targetFile = Path.Combine(sourcePath, fileName);
                    Task task = _packages[packageIndex].ReplaceSupplementaryFileInPackageAsync(file.Value, targetFile, contentType, fileContent);
                    file.Value = FormatFileName(targetFile);
                    Program.signalNewData(2);
                }
                else
                {
                    throw new Exception($"Submodel element {fileElement.IdShort} is not of type File.");
                }
            }
        }

        private string FormatFileName(string fileName)
        {
            string fileNameTemp = fileName;

            string output = Regex.Replace(fileNameTemp, @"\\", "/");

            return output;
        }

        public OperationResult GetOperationAsyncResultSubmodelRepo(string decodedSubmodelId, string idShortPath, string handleId)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(decodedSubmodelId, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation)
                {
                    return new OperationResult();
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of type Operation.");
                }
            }

            return null;
        }

        public OperationResult InvokeOperationSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation operation)
                {
                    CheckOperationVariables(operation, operationRequest);
                    OperationResult operationResult = new OperationResult();
                    //Check the qualifier for demo
                    if (operation.FindQualifierOfType("Demo") != null)
                    {
                        operationResult.OutputArguments = new List<OperationVariable>();
                        operationResult.OutputArguments.Add(new OperationVariable(new Property(DataTypeDefXsd.String, idShort: "DemoOutputArgument")));
                        operationResult.ExecutionState = ExecutionState.CompletedEnum;
                        Result result = new Result();
                        result.Success = true;
                        operationResult.ExecutionResult = result;
                        operationResult.RequestId = operationRequest.RequestId;
                    }
                    return operationResult;
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of type Operation.");
                }
            }

            return null;
        }

        private void CheckOperationVariables(Operation operation, OperationRequest operationRequest)
        {
            if (operation.InputVariables.Count != operationRequest.InputArguments.Count)
            {
                throw new Exception($"Incorrect number of InputVariables in OperationRequest.");
            }
            else if (operation.InoutputVariables.Count != operationRequest.InoutputArguments.Count)
            {
                throw new Exception($"Incorrect number of InOutputVariables in OperationRequest.");
            }
        }

        public OperationResult InvokeOperationAsyncSubmodelRepo(string submodelIdentifier, string idShortPath, OperationRequest operationRequest)
        {
            var operationElement = GetSubmodelElementByPathSubmodelRepo(submodelIdentifier, idShortPath, out _);

            if (operationElement != null)
            {
                if (operationElement is Operation operation)
                {
                    CheckOperationVariables(operation, operationRequest);
                    OperationResult operationHandle = new OperationResult();
                    
                    return operationHandle;
                }
                else
                {
                    throw new Exception($"Submodel element {operationElement.IdShort} is not of type Operation.");
                }
            }

            return null;
        }





        #endregion

        #region Others

        private bool EmptyPackageAvailable(out int emptyPackageIndex)
        {
            emptyPackageIndex = -1;

            for (int envi = 0; envi < _packages.Length; envi++)
            {
                if (_packages[envi] == null)
                {
                    emptyPackageIndex = envi;
                    _packages[emptyPackageIndex] = new AdminShellPackageEnv();
                    return true;
                }
            }

            return false;
        }





        #endregion











    }
}
