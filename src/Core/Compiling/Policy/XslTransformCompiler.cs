using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class XslTransformCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.XslTransform);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<XslTransformConfig>(context, "xsl-transform", out var values))
        {
            return;
        }

        var element = new XElement("xsl-transform");

        if (!element.AddAttribute(values, nameof(XslTransformConfig.Apply), "apply"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "xsl-transform",
                nameof(XslTransformConfig.Apply)
            ));
            return;
        }

        element.AddAttribute(values, nameof(XslTransformConfig.Xsl), "xsl");
        element.AddAttribute(values, nameof(XslTransformConfig.OutputVariable), "output-variable");

        context.AddPolicy(element);
    }
}
