using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class XmlToJsonCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.XmlToJson);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<XmlToJsonConfig>(context, "xml-to-json", out var values))
        {
            return;
        }

        var element = new XElement("xml-to-json");
        if (!element.AddAttribute(values, nameof(XmlToJsonConfig.Apply), "apply"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "xml-to-json",
                nameof(XmlToJsonConfig.Apply)
            ));
            return;
        }

        element.AddAttribute(values, nameof(XmlToJsonConfig.ConsiderAcceptHeader), "consider-accept-header");
        element.AddAttribute(values, nameof(XmlToJsonConfig.ParseDate), "parse-date");
        element.AddAttribute(values, nameof(XmlToJsonConfig.NamespaceSeparator), "namespace-separator");
        element.AddAttribute(values, nameof(XmlToJsonConfig.NamespacePrefix), "namespace-prefix");
        element.AddAttribute(values, nameof(XmlToJsonConfig.AttributeBlockName), "attribute-block-name");

        context.AddPolicy(element);
    }
}
