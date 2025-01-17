using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class SetBackendServiceDaprTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SetBackendServiceDapr(new SetBackendServiceConfig { BaseUrl = "https://internal.contoso.example" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <set-backend-service base-url="https://internal.contoso.example" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile set-backend-service (Dapr) policy with base-url"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SetBackendServiceDapr(new SetBackendServiceConfig { BackendId = "backend-id" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <set-backend-service backend-id="backend-id" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile set-backend-service (Dapr) policy with backend-id"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SetBackendServiceDapr(new SetBackendServiceConfig { BaseUrl = "https://internal.contoso.example", SfResolveCondition = "condition" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <set-backend-service base-url="https://internal.contoso.example" sf-resolve-condition="condition" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile set-backend-service (Dapr) policy with base-url and sf-resolve-condition"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SetBackendServiceDapr(new SetBackendServiceConfig { BackendId = "backend-id", SfServiceInstanceName = "instance-name" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <set-backend-service backend-id="backend-id" sf-service-instance-name="instance-name" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile set-backend-service (Dapr) policy with backend-id and sf-service-instance-name"
    )]
    public void ShouldCompileSetBackendServiceDaprPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
