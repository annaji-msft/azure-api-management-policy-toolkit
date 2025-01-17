using Azure.ApiManagement.PolicyToolkit.Compiling;
using FluentAssertions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Tests.Compiling;

[TestClass]
public class FindAndReplaceTests
{
    [TestMethod]
    public void ShouldCompileFindAndReplacePolicy()
    {
        var code =
            """
            [Document]
            public class PolicyDocument : IDocument
            {
                public void Inbound(IInboundContext context)
                {
                    context.FindAndReplace("find", "replace");
                }
            }
            """;
        var expectedXml =
            """
            <policies>
                <inbound>
                    <find-and-replace find="find" replace="replace" />
                </inbound>
            </policies>
            """;

        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
