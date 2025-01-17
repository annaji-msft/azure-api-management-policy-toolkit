using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.Api.Management.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class ValidateStatusCodeCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateStatusCode);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateStatusCodeConfig>(context, "validate-status-code", out var values))
        {
            return;
        }

        var element = new XElement("validate-status-code");

        if (!element.AddAttribute(values, nameof(ValidateStatusCodeConfig.StatusCode), "status-code"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-status-code",
                nameof(ValidateStatusCodeConfig.StatusCode)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateStatusCodeConfig.FailCheckHttpCode), "failed-check-httpcode"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-status-code",
                nameof(ValidateStatusCodeConfig.FailCheckHttpCode)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateStatusCodeConfig.FailCheckErrorMessage),
                "failed-check-error-message"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-status-code",
                nameof(ValidateStatusCodeConfig.FailCheckErrorMessage)
            ));
            return;
        }

        context.AddPolicy(element);
    }
}
