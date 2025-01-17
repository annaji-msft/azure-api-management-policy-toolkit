using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class InvokeDaprBindingTests
{
    [TestMethod]
    public void ShouldCompileInvokeDaprBindingPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.InvokeDaprBinding("my-binding");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <invoke-dapr-binding binding-name="my-binding" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
