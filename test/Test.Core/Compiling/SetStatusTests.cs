using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SetStatusCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetStatus);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<StatusConfig>(context, "set-status", out var values))
        {
            return;
        }

        var element = new XElement("set-status");

        if (!element.AddAttribute(values, nameof(StatusConfig.Code), "code"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "set-status",
                nameof(StatusConfig.Code)
            ));
            return;
        }

        element.AddAttribute(values, nameof(StatusConfig.Reason), "reason");

        context.AddPolicy(element);
    }
}
