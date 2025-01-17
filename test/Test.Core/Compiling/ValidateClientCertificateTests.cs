using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class ValidateClientCertificateTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.ValidateClientCertificate(new ValidateClientCertificateConfig
                {
                    CertificateThumbprint = "thumbprint"
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <validate-client-certificate certificate-thumbprint="thumbprint" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile validate-client-certificate policy with thumbprint"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.ValidateClientCertificate(new ValidateClientCertificateConfig
                {
                    CertificateThumbprint = "thumbprint",
                    CertificateIssuerName = "issuer",
                    CertificateSubjectName = "subject",
                    CertificateStoreName = "store",
                    CertificateStoreLocation = "location"
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <validate-client-certificate certificate-thumbprint="thumbprint" certificate-issuer-name="issuer" certificate-subject-name="subject" certificate-store-name="store" certificate-store-location="location" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile validate-client-certificate policy with all attributes"
    )]
    public void ShouldCompileValidateClientCertificatePolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
