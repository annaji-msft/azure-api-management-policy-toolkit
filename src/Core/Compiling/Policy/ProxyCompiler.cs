using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class ProxyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.Proxy);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ProxyConfig>(context, "proxy", out var values))
        {
            return;
        }

        var element = new XElement("proxy");

        if (!element.AddAttribute(values, nameof(ProxyConfig.Url), "url"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "proxy",
                nameof(ProxyConfig.Url)
            ));
            return;
        }

        element.AddAttribute(values, nameof(ProxyConfig.Username), "username");
        element.AddAttribute(values, nameof(ProxyConfig.Password), "password");

        context.AddPolicy(element);
    }
}
