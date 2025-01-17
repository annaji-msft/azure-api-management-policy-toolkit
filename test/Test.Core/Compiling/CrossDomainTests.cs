using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class CrossDomainTests
{
    [TestMethod]
    public void ShouldCompileCrossDomainPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.CrossDomain("https://example.com");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <cross-domain domain="https://example.com" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
