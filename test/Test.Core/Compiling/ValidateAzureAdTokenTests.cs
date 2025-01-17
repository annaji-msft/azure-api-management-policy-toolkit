using Microsoft.VisualStudio.TestTools.UnitTesting;
using FluentAssertions;

namespace Azure.ApiManagement.PolicyToolkit.Compiling;

[TestClass]
public class ValidateAzureAdTokenTests
{
    [TestMethod]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.ValidateAzureAdToken(new ValidateAzureAdTokenConfig
                {
                    HeaderName = "Authorization",
                    AllowedAudiences = new[] { "audience1", "audience2" },
                    AllowedIssuers = new[] { "issuer1", "issuer2" },
                    FailedValidationHttpCode = 401,
                    FailedValidationErrorMessage = "Unauthorized",
                    OutputTokenVariableName = "token"
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <validate-azure-ad-token header-name="Authorization" failed-validation-httpcode="401" failed-validation-error-message="Unauthorized" output-token-variable-name="token">
                    <allowed-audiences>
                        <audience>audience1</audience>
                        <audience>audience2</audience>
                    </allowed-audiences>
                    <allowed-issuers>
                        <issuer>issuer1</issuer>
                        <issuer>issuer2</issuer>
                    </allowed-issuers>
                </validate-azure-ad-token>
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile validate-azure-ad-token policy with all parameters"
    )]
    [DataRow(
        """
        [Document]
        public class PolicyDocument : IDocument
        {
            public void Inbound(IInboundContext context)
            {
                context.ValidateAzureAdToken(new ValidateAzureAdTokenConfig
                {
                    HeaderName = "Authorization"
                });
            }
        }
        """,
        """
        <policies>
            <inbound>
                <validate-azure-ad-token header-name="Authorization" />
            </inbound>
        </policies>
        """,
        DisplayName = "Should compile validate-azure-ad-token policy with only required parameters"
    )]
    public void ShouldCompileValidateAzureAdTokenPolicy(string code, string expectedXml)
    {
        code.CompileDocument().Should().BeSuccessful().And.DocumentEquivalentTo(expectedXml);
    }
}
