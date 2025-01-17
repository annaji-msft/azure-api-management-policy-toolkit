using Azure.ApiManagement.PolicyToolkit.Compiling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class ProxyTests
{
    [TestMethod]
    public void ShouldCompileProxyPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Proxy(new ProxyConfig
                    {
                        Url = "https://example.com",
                        Username = "user",
                        Password = "pass"
                    });
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <proxy url="https://example.com" username="user" password="pass" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }

    [TestMethod]
    public void ShouldReportErrorWhenUrlIsNotProvided()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.Proxy(new ProxyConfig
                    {
                        Username = "user",
                        Password = "pass"
                    });
                }
            }
            """;

        var result = code.CompileDocument();
        result.Diagnostics.Should().Contain(d => d.Id == "APIM2006" && d.GetMessage().Contains("proxy") && d.GetMessage().Contains("Url"));
    }
}
