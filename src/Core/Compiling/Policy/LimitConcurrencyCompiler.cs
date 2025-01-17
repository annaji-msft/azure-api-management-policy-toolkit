using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.Api.Management.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class LimitConcurrencyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.LimitConcurrency);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (node.ArgumentList.Arguments.Count != 1)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.ArgumentCountMissMatchForPolicy,
                node.ArgumentList.GetLocation(),
                "limit-concurrency"
            ));
            return;
        }

        var maxConcurrentRequests = node.ArgumentList.Arguments[0].Expression.ProcessParameter(context);
        context.AddPolicy(new XElement("limit-concurrency", new XAttribute("max-concurrent-requests", maxConcurrentRequests)));
    }
}
