using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class LlmTokenLimitCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.LlmTokenLimit);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<LlmTokenLimitConfig>(context, "llm-token-limit", out var values))
        {
            return;
        }

        var element = new XElement("llm-token-limit");

        if (!element.AddAttribute(values, nameof(LlmTokenLimitConfig.Limit), "limit"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "llm-token-limit",
                nameof(LlmTokenLimitConfig.Limit)
            ));
            return;
        }

        element.AddAttribute(values, nameof(LlmTokenLimitConfig.Action), "action");
        element.AddAttribute(values, nameof(LlmTokenLimitConfig.VariableName), "variable-name");

        context.AddPolicy(element);
    }
}
