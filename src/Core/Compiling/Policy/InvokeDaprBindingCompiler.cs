using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.Api.Management.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class InvokeDaprBindingCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.InvokeDaprBinding);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.ArgumentCountMissMatchForPolicy,
                node.ArgumentList.GetLocation(),
                "invoke-dapr-binding"
            ));
            return;
        }

        var bindingName = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("invoke-dapr-binding", new XAttribute("binding-name", bindingName)));
    }
}
