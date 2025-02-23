
namespace AdminShell
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Logging;
    using Newtonsoft.Json;
    using Swashbuckle.AspNetCore.Annotations;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.IO;
    using System.Net.Mime;
    using System.Text;

    [ApiController]
    public class AssetAdministrationShellEnvironmentAPIController : ControllerBase
    {
        private readonly ILogger _logger;
        private readonly AssetAdministrationShellEnvironmentService _aasEnvService;

        public AssetAdministrationShellEnvironmentAPIController(ILoggerFactory logger, AssetAdministrationShellEnvironmentService aasEnvService)
        {
            _logger = logger.CreateLogger("AssetAdministrationShellEnvironmentAPIController");
            _aasEnvService = aasEnvService;
        }

        /// <summary>
        /// Deletes an Asset Administration Shell
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Asset Administration Shell deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("DeleteAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteAssetAdministrationShellById([FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            _aasEnvService.DeleteAssetAdministrationShellById(decodedAasId);

            return NoContent();
        }

        /// <summary>
        /// Deletes a Concept Description
        /// </summary>
        /// <param name="cdIdentifier">The Concept Description’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Concept Description deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("DeleteConceptDescriptionById")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteConceptDescriptionById([FromRoute][Required] string cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(cdIdentifier));

            _aasEnvService.DeleteConceptDescriptionById(decodedCdId);

            return NoContent();
        }

        /// <summary>
        /// Deletes a Submodel
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Submodel deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/Submodels/{submodelIdentifier}")]
        [SwaggerOperation("DeleteSubmodelById")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelById([FromRoute][Required] string submodelIdentifier)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.DeleteSubmodelById(decodedSubmodelId);

            return NoContent();
        }

        /// <summary>
        /// Deletes a submodel element at a specified path within the submodel elements hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="204">Submodel element deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}")]
        [SwaggerOperation("DeleteSubmodelElementByPath")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelElementByPath([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.DeleteSubmodelElementByPath(decodedAasId, decodedSubmodelId, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Deletes a submodel element at a specified path within the submodel elements hierarchy
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="204">Submodel element deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}")]
        [SwaggerOperation("DeleteSubmodelElementByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelElementByPathSubmodelRepo([FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.DeleteSubmodelElementByPathSubmodelRepo(decodedSubmodelId, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Deletes the submodel reference from the Asset Administration Shell
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Submodel reference deleted successfully</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpDelete]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}")]
        [SwaggerOperation("DeleteSubmodelReferenceById")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult DeleteSubmodelReferenceById([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.DeleteSubmodelReferenceById(decodedAasId, decodedSubmodelId);

            return NoContent();

        }

        /// <summary>
        /// Returns all Asset Administration Shells
        /// </summary>
        /// <param name="assetIds">A list of specific Asset identifiers</param>
        /// <param name="idShort">The Asset Administration Shell’s IdShort</param>
        /// <response code="200">Requested Asset Administration Shells</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells")]
        [SwaggerOperation("GetAllAssetAdministrationShells")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<AssetAdministrationShell>), description: "Requested Asset Administration Shells")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllAssetAdministrationShells([FromQuery] List<string> assetIds, [FromQuery] string idShort)
        {
            var output = _aasEnvService.GetAllAssetAdministrationShells(assetIds, idShort);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all Concept Descriptions
        /// </summary>
        /// <param name="idShort">The Concept Description’s IdShort</param>
        /// <param name="isCaseOf">IsCaseOf reference (UTF8-BASE64-URL-encoded)</param>
        /// <param name="dataSpecificationRef">DataSpecification reference (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Concept Descriptions</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/concept-descriptions")]
        [SwaggerOperation("GetAllConceptDescriptions")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<ConceptDescription>), description: "Requested Concept Descriptions")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllConceptDescriptions([FromQuery] string idShort, [FromQuery] string isCaseOf, [FromQuery] string dataSpecificationRef)
        {
            Reference reqIsCaseOf = JsonConvert.DeserializeObject<Reference>(isCaseOf);
            Reference reqDataSpecificationRef = JsonConvert.DeserializeObject<Reference>(dataSpecificationRef);

            var output = _aasEnvService.GetAllConceptDescriptions(idShort, reqIsCaseOf, reqDataSpecificationRef);
            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <param name="diff">Filters response, only elements changed after DateTime</param>
        /// <response code="200">List of found submodel elements</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements")]
        [SwaggerOperation("GetAllSubmodelElements")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SubmodelElement>), description: "List of found submodel elements")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElements([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            //Need to handle path here, as Submodel IdShort needs to be appended before every SME from the list

            var output = _aasEnvService.GetAllSubmodelElements(decodedAasId, decodedSubmodelId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all submodel elements including their hierarchy
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <param name="diff">Filters response, only elements changed after DateTime</param>
        /// <response code="200">List of found submodel elements</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels/{submodelIdentifier}/submodelelements")]
        [SwaggerOperation("GetAllSubmodelElementsSubmodelRepo")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<SubmodelElement>), description: "List of found submodel elements")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelElementsSubmodelRepo([FromRoute][Required] string submodelIdentifier)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            //Need to handle path here, as Submodel IdShort needs to be appended before every SME from the list

            var output = _aasEnvService.GetAllSubmodelElementsFromSubmodel(decodedSubmodelId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all submodel references
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested submodel references</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels")]
        [SwaggerOperation("GetAllSubmodelReferences")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Reference>), description: "Requested submodel references")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodelReferences([FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            var output = _aasEnvService.GetAllSubmodelReferences(decodedAasId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns all Submodels
        /// </summary>
        /// <param name="semanticId">The Value of the semantic Id reference (BASE64-URL-encoded)</param>
        /// <param name="idShort">The Submodel’s idShort</param>
        /// <response code="200">Requested Submodels</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels")]
        [SwaggerOperation("GetAllSubmodels")]
        [SwaggerResponse(statusCode: 200, type: typeof(List<Submodel>), description: "Requested Submodels")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAllSubmodels([FromQuery] string semanticId, [FromQuery] string idShort)
        {
            Reference reqSemanticId = JsonConvert.DeserializeObject<Reference>(semanticId);

            var output = _aasEnvService.GetAllSubmodels(reqSemanticId, idShort);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Asset Administration Shell
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Asset Administration Shell</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("GetAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 200, type: typeof(AssetAdministrationShell), description: "Requested Asset Administration Shell")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetAdministrationShellById([FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            var output = _aasEnvService.GetAssetAdministrationShellById(decodedAasId, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the Asset Information
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Asset Information</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/asset-information")]
        [SwaggerOperation("GetAssetInformation")]
        [SwaggerResponse(statusCode: 200, type: typeof(AssetInformation), description: "Requested Asset Information")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetAssetInformation([FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            var output = _aasEnvService.GetAssetInformationFromAas(decodedAasId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Concept Description
        /// </summary>
        /// <param name="cdIdentifier">The Concept Description’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Concept Description</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("GetConceptDescriptionById")]
        [SwaggerResponse(statusCode: 200, type: typeof(ConceptDescription), description: "Requested Concept Description")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetConceptDescriptionById([FromRoute][Required] string cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(cdIdentifier));

            var output = _aasEnvService.GetConceptDescriptionById(decodedCdId, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Downloads file content from a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Requested file</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method not allowed - Download only valid for File submodel element</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}/attachment")]
        [SwaggerOperation("GetFileByPath")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Requested file")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 405, type: typeof(Result), description: "Method not allowed - Download only valid for File submodel element")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetFileByPath([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var fileName = _aasEnvService.GetFileByPath(decodedAasId, decodedSubmodelId, idShortPath, out byte[] content, out long fileSize);

            //content-disposition so that the aasx file can be doenloaded from the web browser.
            ContentDisposition contentDisposition = new()
            {
                FileName = fileName
            };

            HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            HttpContext.Response.ContentLength = fileSize;
            HttpContext.Response.Body.WriteAsync(content);
            return new EmptyResult();
        }

        /// <summary>
        /// Downloads file content from a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <response code="200">Requested file</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method not allowed - Download only valid for File submodel element</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}/attachment")]
        [SwaggerOperation("GetFileByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 200, type: typeof(string), description: "Requested file")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 405, type: typeof(Result), description: "Method not allowed - Download only valid for File submodel element")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetFileByPathSubmodelRepo([FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var fileName = _aasEnvService.GetFileByPathSubmodelRepo(decodedSubmodelId, idShortPath, out byte[] content, out long fileSize);

            //content-disposition so that the aasx file can be doenloaded from the web browser.
            ContentDisposition contentDisposition = new()
            {
                FileName = fileName
            };

            HttpContext.Response.Headers.Add("Content-Disposition", contentDisposition.ToString());
            HttpContext.Response.ContentLength = fileSize;
            HttpContext.Response.Body.WriteAsync(content);
            return new EmptyResult();
        }

        /// <summary>
        /// Returns the Operation result of an asynchronous invoked Operation
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated), in this case an operation</param>
        /// <param name="handleId">The returned handle Id of an operation’s asynchronous invocation used to request the current state of the operation’s execution (UTF8-BASE64-URL-encoded)</param>
        /// <param name="content"></param>
        /// <response code="200">Operation result object</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}/operation-results/{handleId}")]
        [SwaggerOperation("GetOperationAsyncResult")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetOperationAsyncResult([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath, [FromRoute][Required] string handleId, [FromQuery] string content)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));
            var decodedHandleId = Encoding.UTF8.GetString(Convert.FromBase64String(handleId));

            var output = _aasEnvService.GetOperationAsyncResult(decodedAasId, decodedSubmodelId, idShortPath, decodedHandleId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the Operation result of an asynchronous invoked Operation
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated), in this case an operation</param>
        /// <param name="handleId">The returned handle Id of an operation’s asynchronous invocation used to request the current state of the operation’s execution (UTF8-BASE64-URL-encoded)</param>
        /// <param name="content"></param>
        /// <response code="200">Operation result object</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}/operation-results/{handleId}")]
        [SwaggerOperation("GetOperationAsyncResultSubmodelRepo")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetOperationAsyncResultSubmodelRepo([FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath, [FromRoute][Required] string handleId, [FromQuery] string content)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));
            var decodedHandleId = Encoding.UTF8.GetString(Convert.FromBase64String(handleId));

            var output = _aasEnvService.GetOperationAsyncResultSubmodelRepo(decodedSubmodelId, idShortPath, decodedHandleId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns the Submodel
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested Submodel</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel")]
        [SwaggerOperation("GetSubmodel")]
        [SwaggerResponse(statusCode: 200, type: typeof(Submodel), description: "Requested Submodel")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodel([FromRoute][Required] string aasIdentifier,[FromRoute][Required] string submodelIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.GetSubmodel(decodedAasId, decodedSubmodelId);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific Submodel
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="200">Requested Submodel</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels/{submodelIdentifier}")]
        [SwaggerOperation("GetSubmodelById")]
        [SwaggerResponse(statusCode: 200, type: typeof(Submodel), description: "Requested Submodel")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelById([FromRoute][Required] string submodelIdentifier)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.GetSubmodelById(decodedSubmodelId, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested submodel element</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}")]
        [SwaggerOperation("GetSubmodelElementByPath")]
        [SwaggerResponse(statusCode: 200, type: typeof(SubmodelElement), description: "Requested submodel element")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPath([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier,
            [FromRoute][Required] string idShortPath)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.GetSubmodelElementByPath(decodedAasId, decodedSubmodelId, idShortPath);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Returns a specific submodel element from the Submodel at a specified path
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="200">Requested submodel element</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}")]
        [SwaggerOperation("GetSubmodelElementByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 200, type: typeof(SubmodelElement), description: "Requested submodel element")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GetSubmodelElementByPathSubmodelRepo([FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.GetSubmodelElementByPathSubmodelRepo(decodedSubmodelId, idShortPath, out _);

            return new ObjectResult(output);
        }

        /// <summary>
        /// Synchronously or asynchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated), in this case an operation</param>
        /// <param name="_async">Determines whether an operation invocation is performed asynchronously or synchronously</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method not allowed - Invoke only valid for Operation submodel element</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}/invoke")]
        [SwaggerOperation("InvokeOperation")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 405, type: typeof(Result), description: "Method not allowed - Invoke only valid for Operation submodel element")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperation([FromBody] OperationRequest body, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath, [FromQuery] bool? _async, [FromQuery] string content)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            // TODO: implement

            return StatusCode(200, default(OperationResult));
        }

        /// <summary>
        /// Synchronously or asynchronously invokes an Operation at a specified path
        /// </summary>
        /// <param name="body">Operation request object</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated), in this case an operation</param>
        /// <param name="_async">Determines whether an operation invocation is performed asynchronously or synchronously</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <response code="200">Operation result object</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="405">Method not allowed - Invoke only valid for Operation submodel element</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}/invoke")]
        [SwaggerOperation("InvokeOperationSubmodelRepo")]
        [SwaggerResponse(statusCode: 200, type: typeof(OperationResult), description: "Operation result object")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 405, type: typeof(Result), description: "Method not allowed - Invoke only valid for Operation submodel element")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult InvokeOperationSubmodelRepo([FromBody] OperationRequest body, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath, [FromQuery] bool? _async, [FromQuery] string content)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            //Check async
            if (_async != null && _async == false)
            {
                var output = _aasEnvService.InvokeOperationSubmodelRepo(decodedSubmodelId, idShortPath, body);
                return new ObjectResult(output);
            }

            var outputAsync = _aasEnvService.InvokeOperationAsyncSubmodelRepo(decodedSubmodelId, idShortPath, body);
            return new ObjectResult(outputAsync);

        }

        /// <summary>
        /// Creates a new Asset Administration Shell
        /// </summary>
        /// <param name="body">Asset Administration Shell object</param>
        /// <response code="201">Asset Administration Shell created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells")]
        [SwaggerOperation("PostAssetAdministrationShell")]
        [SwaggerResponse(statusCode: 201, type: typeof(AssetAdministrationShell), description: "Asset Administration Shell created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostAssetAdministrationShell([FromBody] AssetAdministrationShell body)
        {
            var output = _aasEnvService.CreateAssetAdministrationShell(body);

            return CreatedAtAction(nameof(PostAssetAdministrationShell), output);
        }

        /// <summary>
        /// Creates a new Concept Description
        /// </summary>
        /// <param name="body">Concept Description object</param>
        /// <response code="201">Concept Description created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/concept-descriptions")]
        [SwaggerOperation("PostConceptDescription")]
        [SwaggerResponse(statusCode: 201, type: typeof(ConceptDescription), description: "Concept Description created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostConceptDescription([FromBody] ConceptDescription body)
        {
            var output = _aasEnvService.CreateConceptDescription(body);

            return CreatedAtAction(nameof(PostConceptDescription), output);
        }

        /// <summary>
        /// Creates a new Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <response code="201">Submodel created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/Submodels")]
        [SwaggerOperation("PostSubmodel")]
        [SwaggerResponse(statusCode: 201, type: typeof(Submodel), description: "Submodel created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodel([FromBody] Submodel body)
        {
            var output = _aasEnvService.CreateSubmodel(body);

            return CreatedAtAction(nameof(PostSubmodel), output);
        }

        /// <summary>
        /// Creates a new submodel element
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements")]
        [SwaggerOperation("PostSubmodelElement")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElement([FromBody] SubmodelElement body, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.CreateSubmodelElement(body, decodedAasId, decodedSubmodelId);

            return CreatedAtAction(nameof(PostSubmodelElement), output);
        }

        /// <summary>
        /// Creates a new submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}")]
        [SwaggerOperation("PostSubmodelElementByPath")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElementByPath([FromBody] SubmodelElement body, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.CreateSubmodelElementByPath(body, decodedAasId, decodedSubmodelId, idShortPath);

            return CreatedAtAction(nameof(PostSubmodelElementByPath), output);
        }

        /// <summary>
        /// Creates a new submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}")]
        [SwaggerOperation("PostSubmodelElementByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElementByPathSubmodelRepo([FromBody] SubmodelElement body, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.CreateSubmodelElementByPathSubmodelRepo(body, decodedSubmodelId, idShortPath);

            return CreatedAtAction(nameof(PostSubmodelElementByPathSubmodelRepo), output);
        }

        /// <summary>
        /// Creates a new submodel element
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="201">Submodel element created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/Submodels/{submodelIdentifier}/submodelelements")]
        [SwaggerOperation("PostSubmodelElementSubmodelRepo")]
        [SwaggerResponse(statusCode: 201, type: typeof(SubmodelElement), description: "Submodel element created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelElementSubmodelRepo([FromBody] SubmodelElement body, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var output = _aasEnvService.CreateSubmodelElementSubmodelRepo(body, decodedSubmodelId);

            return CreatedAtAction(nameof(PostSubmodelElementSubmodelRepo), output);
        }

        /// <summary>
        /// Creates a submodel reference at the Asset Administration Shell
        /// </summary>
        /// <param name="body">Reference to the Submodel</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="201">Submodel reference created successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPost]
        [Route("/shells/{aasIdentifier}/Submodels")]
        [SwaggerOperation("PostSubmodelReference")]
        [SwaggerResponse(statusCode: 201, type: typeof(Reference), description: "Submodel reference created successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PostSubmodelReference([FromBody] Reference body, [FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            var output = _aasEnvService.CreateSubmodelReference(body, decodedAasId);

            return CreatedAtAction(nameof(PostSubmodelReference), output);
        }

        /// <summary>
        /// Updates an existing Asset Administration Shell
        /// </summary>
        /// <param name="body">Asset Administration Shell object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Asset Administration Shell updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}")]
        [SwaggerOperation("PutAssetAdministrationShellById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutAssetAdministrationShellById([FromBody] AssetAdministrationShell body, [FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            _aasEnvService.UpdateAssetAdministrationShellById(body, decodedAasId);

            return NoContent();
        }

        /// <summary>
        /// Updates the Asset Information
        /// </summary>
        /// <param name="body">Asset Information object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Asset Information updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/asset-information")]
        [SwaggerOperation("PutAssetInformation")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutAssetInformation([FromBody] AssetInformation body, [FromRoute][Required] string aasIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));

            _aasEnvService.UpdateAssetInformation(body, decodedAasId);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing Concept Description
        /// </summary>
        /// <param name="body">Concept Description object</param>
        /// <param name="cdIdentifier">The Concept Description’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Concept Description updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/concept-descriptions/{cdIdentifier}")]
        [SwaggerOperation("PutConceptDescriptionById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutConceptDescriptionById([FromBody] ConceptDescription body, [FromRoute][Required] string cdIdentifier)
        {
            var decodedCdId = Encoding.UTF8.GetString(Convert.FromBase64String(cdIdentifier));

            _aasEnvService.UpdateConceptDescriptionById(body, decodedCdId);

            return NoContent();
        }

        /// <summary>
        /// Updates the Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel")]
        [SwaggerOperation("PutSubmodel")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodel([FromBody] Submodel body, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.UpdateSubmodel(body, decodedAasId, decodedSubmodelId);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing Submodel
        /// </summary>
        /// <param name="body">Submodel object</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <response code="204">Submodel updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/Submodels/{submodelIdentifier}")]
        [SwaggerOperation("PutSubmodelById")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodelById([FromBody] Submodel body, [FromRoute][Required] string submodelIdentifier)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.UpdateSubmodelById(body, decodedSubmodelId);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodel/submodelelements/{idShortPath}")]
        [SwaggerOperation("PutSubmodelElementByPath")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodelElementByPath([FromBody] SubmodelElement body, [FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.UpdateSubmodelElementByPath(body, decodedAasId, decodedSubmodelId, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Updates an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="body">Requested submodel element</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="level">Determines the structural depth of the respective resource content</param>
        /// <param name="content">Determines the request or response Kind of the resource</param>
        /// <param name="extent">Determines to which extent the resource is being serialized</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}")]
        [SwaggerOperation("PutSubmodelElementByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutSubmodelElementByPathSubmodelRepo([FromBody] SubmodelElement body, [FromRoute][Required] string submodelIdentifier, [FromRoute][Required] string idShortPath)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            _aasEnvService.UpdateSubmodelElementByPathSubmodelRepo(body, decodedSubmodelId, idShortPath);

            return NoContent();
        }

        /// <summary>
        /// Uploads file content to an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="aasIdentifier">The Asset Administration Shell’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="file">File to upload</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/shells/{aasIdentifier}/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}/attachment")]
        [SwaggerOperation("PutFileByPath")]
        [SwaggerResponse(statusCode: 204, type: typeof(Result), description: "Submodel element updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutFileByPath([FromRoute][Required] string aasIdentifier, [FromRoute][Required] string submodelIdentifier, [FromRoute] string idShortPath, [Required][FromForm] IFormFile file)
        {
            var decodedAasId = Encoding.UTF8.GetString(Convert.FromBase64String(aasIdentifier));
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var stream = new MemoryStream();
            file.CopyTo(stream);
            string fileName = file.FileName;
            string contentType = file.ContentType;

            _aasEnvService.UpdateFileByPath(decodedAasId, decodedSubmodelId, idShortPath, fileName, contentType, stream);

            return NoContent();
        }

        /// <summary>
        /// Uploads file content to an existing submodel element at a specified path within submodel elements hierarchy
        /// </summary>
        /// <param name="submodelIdentifier">The Submodel’s unique Id (UTF8-BASE64-URL-encoded)</param>
        /// <param name="idShortPath">IdShort path to the submodel element (dot-separated)</param>
        /// <param name="file">File to upload</param>
        /// <response code="204">Submodel element updated successfully</response>
        /// <response code="400">Bad Request</response>
        /// <response code="404">Not Found</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpPut]
        [Route("/Submodels/{submodelIdentifier}/submodelelements/{idShortPath}/attachment")]
        [SwaggerOperation("PutFileByPathSubmodelRepo")]
        [SwaggerResponse(statusCode: 204, type: typeof(Result), description: "Submodel element updated successfully")]
        [SwaggerResponse(statusCode: 400, type: typeof(Result), description: "Bad Request")]
        [SwaggerResponse(statusCode: 404, type: typeof(Result), description: "Not Found")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult PutFileByPathSubmodelRepo([FromRoute][Required] string submodelIdentifier, [FromRoute] string idShortPath, [Required][FromForm] IFormFile file)
        {
            var decodedSubmodelId = Encoding.UTF8.GetString(Convert.FromBase64String(submodelIdentifier));

            var stream = new MemoryStream();
            file.CopyTo(stream);
            string fileName = file.FileName;
            string contentType = file.ContentType;

            _aasEnvService.UpdateFileByPathSubmodelRepo(decodedSubmodelId, idShortPath, fileName, contentType, stream);

            return NoContent();
        }
    }
}
