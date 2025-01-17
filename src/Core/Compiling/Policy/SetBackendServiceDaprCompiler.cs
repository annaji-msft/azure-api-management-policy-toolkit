using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SetBackendServiceDaprCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SetBackendServiceDapr);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<SetBackendServiceConfig>(context, "set-backend-service (Dapr)", out var values))
        {
            return;
        }

        var element = new XElement("set-backend-service");

        var baseUrlDefined = element.AddAttribute(values, nameof(SetBackendServiceConfig.BaseUrl), "base-url");
        var backendIdDefined = element.AddAttribute(values, nameof(SetBackendServiceConfig.BackendId), "backend-id");
        if (!(baseUrlDefined ^ backendIdDefined))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.OnlyOneOfTwoShouldBeDefined,
                node.GetLocation(),
                "set-backend-service (Dapr)",
                nameof(SetBackendServiceConfig.BaseUrl),
                nameof(SetBackendServiceConfig.BackendId)
            ));
            return;
        }

        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfResolveCondition), "sf-resolve-condition");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfServiceInstanceName), "sf-service-instance-name");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfPartitionKey), "sf-partition-key");
        element.AddAttribute(values, nameof(SetBackendServiceConfig.SfListenerName), "sf-listener-name");

        context.AddPolicy(element);
    }
}
