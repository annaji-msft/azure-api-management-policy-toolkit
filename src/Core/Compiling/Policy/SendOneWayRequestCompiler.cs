﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class SendOneWayRequestCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.SendOneWayRequest);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<SendRequestConfig>(context, "send-one-way-request", out var values))
        {
            return;
        }

        var element = new XElement("send-one-way-request");

        if (!element.AddAttribute(values, nameof(SendRequestConfig.Url), "url"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "send-one-way-request",
                nameof(SendRequestConfig.Url)
            ));
            return;
        }

        element.AddAttribute(values, nameof(SendRequestConfig.Method), "method");
        element.AddAttribute(values, nameof(SendRequestConfig.Timeout), "timeout");
        element.AddAttribute(values, nameof(SendRequestConfig.IgnoreError), "ignore-error");

        if (values.TryGetValue(nameof(SendRequestConfig.Headers), out var headers))
        {
            SetHeaderCompiler.HandleHeaders(context, element, headers);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Body), out var body))
        {
            SetBodyCompiler.HandleBody(context, element, body);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Authentication), out var authentication))
        {
            HandleAuthentication(context, element, authentication);
        }

        if (values.TryGetValue(nameof(SendRequestConfig.Proxy), out var proxy))
        {
            HandleProxy(context, element, proxy);
        }

        context.AddPolicy(element);
    }

    private void HandleProxy(ICompilationContext context, XElement element, InitializerValue value)
    {
        if (!value.TryGetValues<ProxyConfig>(out var values))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.PolicyArgumentIsNotOfRequiredType,
                value.Node.GetLocation(),
                $"{element.Name}.proxy",
                nameof(ProxyConfig)
            ));
            return;
        }

        var certificateElement = new XElement("proxy");
        if (!certificateElement.AddAttribute(values, nameof(ProxyConfig.Url), "url"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                value.Node.GetLocation(),
                $"{element.Name}.proxy",
                nameof(ProxyConfig.Url)
            ));
            return;
        }

        certificateElement.AddAttribute(values, nameof(ProxyConfig.Username), "username");
        certificateElement.AddAttribute(values, nameof(ProxyConfig.Password), "password");
        element.Add(certificateElement);
    }

    private void HandleAuthentication(ICompilationContext context, XElement element, InitializerValue authentication)
    {
        var values = authentication.NamedValues;
        if (values is null)
        {
            return;
        }

        switch (authentication.Type)
        {
            case nameof(CertificateAuthenticationConfig):
                HandleCertificateAuthentication(context, element, values, authentication.Node);
                break;
            case nameof(BasicAuthenticationConfig):
                HandleBasicAuthentication(context, element, values, authentication.Node);
                break;
            case nameof(ManagedIdentityAuthenticationConfig):
                HandleManagedIdentityAuthentication(context, element, values, authentication.Node);
                break;
            default:
                context.Report(Diagnostic.Create(
                    CompilationErrors.NotSupportedType,
                    authentication.Node.GetLocation(),
                    $"{element.Name}",
                    authentication.Type
                ));
                break;
        }
    }

    private void HandleBasicAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var basicElement = new XElement("authentication-basic");
        if (!basicElement.AddAttribute(values, nameof(BasicAuthenticationConfig.Username), "username"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                $"{element.Name}.authentication-basic",
                nameof(BasicAuthenticationConfig.Username)
            ));
            return;
        }

        if (!basicElement.AddAttribute(values, nameof(BasicAuthenticationConfig.Password), "password"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                $"{element.Name}.authentication-basic",
                nameof(BasicAuthenticationConfig.Password)
            ));
            return;
        }

        element.Add(basicElement);
    }

    private void HandleCertificateAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var certElement = new XElement("authentication-certificate");
        certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Password), "password");

        if (new[] 
            {
                certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Thumbprint), "thumbprint"),
                certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.CertificateId), "certificate-id"),
                certElement.AddAttribute(values, nameof(CertificateAuthenticationConfig.Body), "body")
            }.Count(b => b) != 1)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.OnlyOneOfTreeShouldBeDefined,
                node.GetLocation(),
                $"{element.Name}.authentication-certificate",
                nameof(CertificateAuthenticationConfig.Thumbprint),
                nameof(CertificateAuthenticationConfig.CertificateId),
                nameof(CertificateAuthenticationConfig.Body)
            ));
            return;
        }

        element.Add(certElement);
    }

    private void HandleManagedIdentityAuthentication(
        ICompilationContext context,
        XElement element,
        IReadOnlyDictionary<string, InitializerValue> values,
        SyntaxNode node)
    {
        var certElement = new XElement("authentication-managed-identity");
        if (!certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.Resource), "resource"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                $"{element.Name}.authentication-managed-identity",
                nameof(ManagedIdentityAuthenticationConfig.Resource)
            ));
        }
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.ClientId), "client-id");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.OutputTokenVariableName),
            "output-token-variable-name");
        certElement.AddAttribute(values, nameof(ManagedIdentityAuthenticationConfig.IgnoreError), "ignore-error");
        element.Add(certElement);
    }
}
