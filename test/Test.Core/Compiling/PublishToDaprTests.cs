using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Compiling.Policy;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class PublishToDaprTests
{
    [TestMethod]
    public void ShouldCompilePublishToDaprPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.PublishToDapr("daprConfig");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <publish-to-dapr config="daprConfig" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
