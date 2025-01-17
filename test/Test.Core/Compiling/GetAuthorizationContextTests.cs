using Azure.ApiManagement.PolicyToolkit.Compiling;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class GetAuthorizationContextTests
{
    [TestMethod]
    public void ShouldCompileGetAuthorizationContextPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.GetAuthorizationContext("auth-context");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <get-authorization-context authorization-context="auth-context" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
