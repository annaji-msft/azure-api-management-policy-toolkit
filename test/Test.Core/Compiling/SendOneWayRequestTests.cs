// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class SendOneWayRequestTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig { Url = "https://example.com" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig { Url = "https://example.com", Method = "POST" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com" method="POST" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and method"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig { Url = "https://example.com", Timeout = "30" });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com" timeout="30" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and timeout"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig { Url = "https://example.com", IgnoreError = true });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com" ignore-error="true" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and ignore-error"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Headers = new []
                    {
                        new HeaderConfig { Name = "Authorization", Value = "Bearer token" }
                    }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <set-header name="Authorization" exists-action="override">
                        <value>Bearer token</value>
                    </set-header>
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and headers"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Body = new BodyConfig { Content = "Request body" }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <set-body>@("Request body")</set-body>
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and body"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Authentication = new BasicAuthenticationConfig { Username = "user", Password = "pass" }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <authentication-basic username="user" password="pass" />
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and basic authentication"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Authentication = new CertificateAuthenticationConfig { Thumbprint = "THUMBPRINT" }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <authentication-certificate thumbprint="THUMBPRINT" />
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and certificate authentication"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Authentication = new ManagedIdentityAuthenticationConfig { Resource = "resource" }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <authentication-managed-identity resource="resource" />
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and managed identity authentication"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.SendOneWayRequest(new SendRequestConfig
                {
                    Url = "https://example.com",
                    Proxy = new ProxyConfig { Url = "https://proxy.com" }
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <send-one-way-request url="https://example.com">
                    <proxy url="https://proxy.com" />
                </send-one-way-request>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile send-one-way-request policy with URL and proxy"
    )]
    public void ShouldCompileSendOneWayRequestPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
