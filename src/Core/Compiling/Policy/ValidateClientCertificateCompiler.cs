using System.Xml.Linq;
using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class ValidateClientCertificateCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateClientCertificate);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateClientCertificateConfig>(context, "validate-client-certificate", out var values))
        {
            return;
        }

        var element = new XElement("validate-client-certificate");

        if (!element.AddAttribute(values, nameof(ValidateClientCertificateConfig.CertificateThumbprint), "certificate-thumbprint"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-client-certificate",
                nameof(ValidateClientCertificateConfig.CertificateThumbprint)
            ));
            return;
        }

        element.AddAttribute(values, nameof(ValidateClientCertificateConfig.CertificateIssuerName), "certificate-issuer-name");
        element.AddAttribute(values, nameof(ValidateClientCertificateConfig.CertificateSubjectName), "certificate-subject-name");
        element.AddAttribute(values, nameof(ValidateClientCertificateConfig.CertificateStoreName), "certificate-store-name");
        element.AddAttribute(values, nameof(ValidateClientCertificateConfig.CertificateStoreLocation), "certificate-store-location");

        context.AddPolicy(element);
    }
}
