using System.Xml.Linq;
using Azure.Api.Management.PolicyToolkit.Authoring;
using Azure.Api.Management.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class ValidateContentCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateContent);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateContentConfig>(context, "validate-content", out var values))
        {
            return;
        }

        var element = new XElement("validate-content");

        if (!element.AddAttribute(values, nameof(ValidateContentConfig.ContentType), "content-type"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-content",
                nameof(ValidateContentConfig.ContentType)
            ));
            return;
        }

        element.AddAttribute(values, nameof(ValidateContentConfig.MaxSize), "max-size");
        element.AddAttribute(values, nameof(ValidateContentConfig.MaxSizeUnit), "max-size-unit");
        element.AddAttribute(values, nameof(ValidateContentConfig.MaxSizeErrorMessage), "max-size-error-message");
        element.AddAttribute(values, nameof(ValidateContentConfig.MaxSizeErrorCode), "max-size-error-code");

        context.AddPolicy(element);
    }
}
