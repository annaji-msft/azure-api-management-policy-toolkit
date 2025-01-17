using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class RetryCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Retry);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<RetryConfig>(context, "retry", out var values))
        {
            return;
        }

        var element = new XElement("retry");

        if (!element.AddAttribute(values, nameof(RetryConfig.Count), "count"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "retry",
                nameof(RetryConfig.Count)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(RetryConfig.Interval), "interval"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "retry",
                nameof(RetryConfig.Interval)
            ));
            return;
        }

        context.AddPolicy(element);
    }
}
