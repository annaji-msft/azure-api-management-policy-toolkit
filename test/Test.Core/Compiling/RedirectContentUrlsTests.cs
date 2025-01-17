using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Compiling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class RedirectContentUrlsTests
{
    [TestMethod]
    public void ShouldCompileRedirectContentUrlsPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.RedirectContentUrls("https://example.com");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <redirect-content-urls url="https://example.com" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldReportErrorForInvalidArgumentCount()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.RedirectContentUrls();
                }
            }
            """;

        var result = code.CompileDocument();
        result.Should().HaveDiagnostics().And.HaveError("APIM2001", "Argument count miss match for 'redirect-content-urls' policy");
    }
}
