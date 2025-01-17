using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class FindAndReplaceCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.FindAndReplace);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 2)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.ArgumentCountMissMatchForPolicy,
                node.ArgumentList.GetLocation(),
                "find-and-replace"));
            return;
        }

        var find = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        var replace = node.ArgumentList.Arguments[1].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("find-and-replace", new XAttribute("find", find), new XAttribute("replace", replace)));
    }
}
