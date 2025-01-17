using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class LogToEventHubCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.LogToEventHub);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<LogToEventHubConfig>(context, "log-to-eventhub", out var values))
        {
            return;
        }

        var element = new XElement("log-to-eventhub");

        if (!element.AddAttribute(values, nameof(LogToEventHubConfig.LoggerId), "logger-id"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "log-to-eventhub",
                nameof(LogToEventHubConfig.LoggerId)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(LogToEventHubConfig.EventHubName), "eventhub-name"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "log-to-eventhub",
                nameof(LogToEventHubConfig.EventHubName)
            ));
            return;
        }

        element.AddAttribute(values, nameof(LogToEventHubConfig.PartitionKey), "partition-key");
        element.AddAttribute(values, nameof(LogToEventHubConfig.Message), "message");

        context.AddPolicy(element);
    }
}
