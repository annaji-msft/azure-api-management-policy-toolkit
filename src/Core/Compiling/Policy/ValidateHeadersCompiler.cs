// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class ValidateHeadersCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateHeaders);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateHeadersConfig>(context, "validate-headers", out var values))
        {
            return;
        }

        var element = new XElement("validate-headers");

        if (!element.AddAttribute(values, nameof(ValidateHeadersConfig.Name), "name"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.Name)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateHeadersConfig.FailCheckHttpCode), "failed-check-httpcode"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.FailCheckHttpCode)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateHeadersConfig.FailCheckErrorMessage),
                "failed-check-error-message"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.FailCheckErrorMessage)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateHeadersConfig.IgnoreCase), "ignore-case"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.IgnoreCase)
            ));
            return;
        }

        if (!values.TryGetValue(nameof(ValidateHeadersConfig.Values), out var headerValues))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.Values)
            ));
            return;
        }

        var elements = (headerValues.UnnamedValues ?? [])
            .Select(origin => new XElement("value", origin.Value!))
            .ToArray<object>();
        if (elements.Length == 0)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterIsEmpty,
                headerValues.Node.GetLocation(),
                "validate-headers",
                nameof(ValidateHeadersConfig.Values)
            ));
            return;
        }

        element.Add(elements);

        context.AddPolicy(element);
    }
}
