using CustomAuthenticationExtensions.API.Models;
using CustomAuthenticationExtensions.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

namespace CustomAuthenticationExtensions.API;

public class ValidateEmailDomain
{
    private readonly ILogger<ValidateEmailDomain> _logger;
    private readonly IDynamicsClientDomainService _clientDomainService;

    public ValidateEmailDomain(ILogger<ValidateEmailDomain> logger, IDynamicsClientDomainService clientDomainService)
    {
        _logger = logger;
        _clientDomainService = clientDomainService;
    }

    [Function("OnAttributeCollectionSubmit_ValidateEmailDomain")]
    public async Task<IActionResult> RunAsync([HttpTrigger(AuthorizationLevel.Function, "post")] HttpRequest req)
    {
        _logger.LogInformation("OnAttributeCollectionSubmit_ValidateEmailDomain endpoint triggered");

        foreach(var header in req.Headers)
        {
            _logger.LogInformation($"Header: {header.Key} = {header.Value}");
        }

        // Get the request body
        string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
        JsonNode jsonPayload = JsonNode.Parse(requestBody)!;

        // Get identities node from payload
        JsonNode identities = jsonPayload["data"]!["userSignUpInfo"]!["identities"]!;

        // Initialise response data
        ResponseObject responseData = new ResponseObject("microsoft.graph.onAttributeCollectionSubmitResponseData");
        var attributeErrors = new Dictionary<string, string>();

        if (identities == null)
        {
            return new BadRequestObjectResult("Identities node is missing from payload");
        }

        if (identities.AsArray().First()["issuerAssignedId"] == null)
        {
            return new BadRequestObjectResult("Identities node does not contain issuerAssignedId");
        }

        var userEmailAddress = identities.AsArray().First()["issuerAssignedId"].AsValue().ToString();
        var userDomain = userEmailAddress.Split('@').LastOrDefault();

        if (userDomain == null)
        {
            return new BadRequestObjectResult("Could not derive user email domain from issuerAssignedId");
        }

        if (userDomain.Equals("barclays.com", StringComparison.OrdinalIgnoreCase))
        {
            responseData.Data.Actions = new List<ResponseAction>() { new ResponseAction(
            "microsoft.graph.attributeCollectionSubmit.continueWithDefaultBehavior") };
        }
        else
        {
            responseData.Data.Actions = new List<ResponseAction>() { new ResponseAction(
                "microsoft.graph.attributeCollectionSubmit.showBlockPage",
                "You do not belong to an AOShearman client domain.",
                "You can't register") };
        }

        return new OkObjectResult(responseData);
    }
}