using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Tests;

[TestClass]
public class RetryTests
{
    [TestMethod]
    public void ShouldCompileRetryPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Retry(new RetryConfig
                    {
                        Count = 3,
                        Interval = "00:00:05"
                    });
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <retry count="3" interval="00:00:05" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldReportErrorWhenCountIsNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Retry(new RetryConfig
                    {
                        Interval = "00:00:05"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM2006", "Required 'Count' parameter was not defined for 'retry' policy");
    }

    [TestMethod]
    public void ShouldReportErrorWhenIntervalIsNotDefined()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Retry(new RetryConfig
                    {
                        Count = 3
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM2006", "Required 'Interval' parameter was not defined for 'retry' policy");
    }
}
