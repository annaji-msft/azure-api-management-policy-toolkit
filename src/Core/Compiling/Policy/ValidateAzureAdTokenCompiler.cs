using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class ValidateAzureAdTokenCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateAzureAdToken);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateAzureAdTokenConfig>(context, "validate-azure-ad-token", out var values))
        {
            return;
        }

        var element = new XElement("validate-azure-ad-token");

        if (!element.AddAttribute(values, nameof(ValidateAzureAdTokenConfig.HeaderName), "header-name"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-azure-ad-token",
                nameof(ValidateAzureAdTokenConfig.HeaderName)
            ));
            return;
        }

        element.AddAttribute(values, nameof(ValidateAzureAdTokenConfig.FailedValidationHttpCode), "failed-validation-httpcode");
        element.AddAttribute(values, nameof(ValidateAzureAdTokenConfig.FailedValidationErrorMessage), "failed-validation-error-message");
        element.AddAttribute(values, nameof(ValidateAzureAdTokenConfig.OutputTokenVariableName), "output-token-variable-name");

        if (values.TryGetValue(nameof(ValidateAzureAdTokenConfig.AllowedAudiences), out var allowedAudiences))
        {
            var audiencesElement = new XElement("allowed-audiences");
            foreach (var audience in allowedAudiences.UnnamedValues ?? [])
            {
                audiencesElement.Add(new XElement("audience", audience.Value!));
            }
            element.Add(audiencesElement);
        }

        if (values.TryGetValue(nameof(ValidateAzureAdTokenConfig.AllowedIssuers), out var allowedIssuers))
        {
            var issuersElement = new XElement("allowed-issuers");
            foreach (var issuer in allowedIssuers.UnnamedValues ?? [])
            {
                issuersElement.Add(new XElement("issuer", issuer.Value!));
            }
            element.Add(issuersElement);
        }

        context.AddPolicy(element);
    }
}
