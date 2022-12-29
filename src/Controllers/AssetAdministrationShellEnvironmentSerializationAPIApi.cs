/*
 * DotAAS Part 2 | HTTP/REST | Asset Administration Shell Environment API
 *
 * An exemplary API combination for the use case of an Asset Administration Shell Environment
 *
 * OpenAPI spec version: V1.0RC03
 * 
 * Generated by: https://github.com/swagger-api/swagger-codegen.git
 */
using IO.Swagger.Models;
using IO.Swagger.V1RC03.Attributes;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.Annotations;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace IO.Swagger.V1RC03.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [ApiController]
    public class AssetAdministrationShellEnvironmentSerializationAPIController : ControllerBase, IAssetAdministrationShellEnvironmentSerializationAPIController
    {
        private readonly ILogger<AssetAdministrationShellEnvironmentSerializationAPIController> _logger;

        public AssetAdministrationShellEnvironmentSerializationAPIController(ILogger<AssetAdministrationShellEnvironmentSerializationAPIController> logger)
        {
            _logger = logger;
        }

        /// <summary>
        /// Returns an appropriate serialization based on the specified format (see SerializationFormat)
        /// </summary>
        /// <param name="aasIds">The Asset Administration Shells&#x27; unique ids (UTF8-BASE64-URL-encoded)</param>
        /// <param name="submodelIds">The Submodels&#x27; unique ids (UTF8-BASE64-URL-encoded)</param>
        /// <param name="includeConceptDescriptions">Include Concept Descriptions?</param>
        /// <response code="200">Requested serialization based on SerializationFormat</response>
        /// <response code="0">Default error handling for unmentioned status codes</response>
        [HttpGet]
        [Route("/serialization")]
        [ValidateModelState]
        [SwaggerOperation("GenerateSerializationByIds")]
        [SwaggerResponse(statusCode: 200, type: typeof(byte[]), description: "Requested serialization based on SerializationFormat")]
        [SwaggerResponse(statusCode: 0, type: typeof(Result), description: "Default error handling for unmentioned status codes")]
        public virtual IActionResult GenerateSerializationByIds([FromQuery][Required()] List<string> aasIds, [FromQuery][Required()] List<string> submodelIds, [FromQuery][Required()] bool? includeConceptDescriptions)
        {
            var decodedAasIds = new List<string>();
            foreach (var aasId in aasIds)
            {
                decodedAasIds.Add(Base64UrlEncoder.Decode(aasId));
            }

            var decodedSubmodelIds = new List<string>();
            foreach (var submodelId in submodelIds)
            {
                decodedSubmodelIds.Add(Base64UrlEncoder.Decode(submodelId));
            }

            var output = _serializationService.GenerateSerializationByIds(decodedAasIds, decodedSubmodelIds, (bool)includeConceptDescriptions);

            return new ObjectResult(output);
        }
    }
}
