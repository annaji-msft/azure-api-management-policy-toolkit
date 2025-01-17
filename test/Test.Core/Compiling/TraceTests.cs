using Azure.ApiManagement.PolicyToolkit.Compiling;

namespace Azure.ApiManagement.PolicyToolkit.Tests;

[TestClass]
public class TraceTests
{
    [TestMethod]
    public void ShouldCompileTracePolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Trace("This is a trace message");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <trace message="This is a trace message" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
