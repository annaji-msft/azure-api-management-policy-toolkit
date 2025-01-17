using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class WaitCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Wait);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<WaitConfig>(context, "wait", out var values))
        {
            return;
        }

        var element = new XElement("wait");

        if (!element.AddAttribute(values, nameof(WaitConfig.Duration), "duration"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "wait",
                nameof(WaitConfig.Duration)
            ));
            return;
        }

        context.AddPolicy(element);
    }
}
