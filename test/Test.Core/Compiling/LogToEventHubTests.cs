using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class LogToEventHubTests
{
    [TestMethod]
    public void ShouldCompileLogToEventHubPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.LogToEventHub(new LogToEventHubConfig
                    {
                        LoggerId = "logger-id",
                        EventHubName = "eventhub-name",
                        PartitionKey = "partition-key",
                        Message = "message"
                    });
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <log-to-eventhub logger-id="logger-id" eventhub-name="eventhub-name" partition-key="partition-key" message="message" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldReportMissingLoggerId()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.LogToEventHub(new LogToEventHubConfig
                    {
                        EventHubName = "eventhub-name",
                        PartitionKey = "partition-key",
                        Message = "message"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Diagnostics.Should().ContainSingle(d => d.Id == "APIM2006" && d.GetMessage().Contains("LoggerId"));
    }

    [TestMethod]
    public void ShouldReportMissingEventHubName()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.LogToEventHub(new LogToEventHubConfig
                    {
                        LoggerId = "logger-id",
                        PartitionKey = "partition-key",
                        Message = "message"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Diagnostics.Should().ContainSingle(d => d.Id == "APIM2006" && d.GetMessage().Contains("EventHubName"));
    }
}
