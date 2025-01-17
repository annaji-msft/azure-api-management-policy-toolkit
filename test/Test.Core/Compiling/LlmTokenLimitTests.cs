using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class LlmTokenLimitTests
{
    [TestMethod]
    public void ShouldCompileLlmTokenLimitPolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.LlmTokenLimit(new LlmTokenLimitConfig
                    {
                        Limit = 1000,
                        Action = "block",
                        VariableName = "tokenCount"
                    });
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <llm-token-limit limit="1000" action="block" variable-name="tokenCount" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
