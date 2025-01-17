﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using System.Xml.Linq;

using Azure.ApiManagement.PolicyToolkit.Authoring;
using Azure.ApiManagement.PolicyToolkit.Compiling.Diagnostics;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.ApiManagement.PolicyToolkit.Compiling.Policy;

public class QuotaByKeyCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.QuotaByKey);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<QuotaConfig>(context, "quota-by-key", out var values))
        {
            return;
        }

        var element = new XElement("quota-by-key");

        var isCallsAdded = element.AddAttribute(values, nameof(QuotaConfig.Calls), "calls");
        var isBandwidthAdded = element.AddAttribute(values, nameof(QuotaConfig.Bandwidth), "bandwidth");

        if (!isCallsAdded && !isBandwidthAdded)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.OnlyOneOfTwoShouldBeDefined,
                node.GetLocation(),
                "quota-by-key",
                nameof(QuotaConfig.Calls),
                nameof(QuotaConfig.Bandwidth)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(QuotaConfig.RenewalPeriod), "renewal-period"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "quota-by-key",
                nameof(QuotaConfig.RenewalPeriod)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(QuotaConfig.CounterKey), "counter-key"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "quota-by-key",
                nameof(QuotaConfig.CounterKey)
            ));
            return;
        }

        if (values.TryGetValue(nameof(QuotaConfig.Apis), out var apis))
        {
            foreach (var api in apis.UnnamedValues!)
            {
                if (api.Type != nameof(ApiQuota))
                {
                    context.Report(Diagnostic.Create(
                        CompilationErrors.PolicyArgumentIsNotOfRequiredType,
                        api.Node.GetLocation(),
                        "quota-by-key.api",
                        nameof(ApiQuota)
                    ));
                    continue;
                }

                if (!Handle(context, "api", api, out var apiElement))
                {
                    continue;
                }

                element.Add(apiElement);

                if (api.NamedValues!.TryGetValue(nameof(ApiQuota.Operations), out var operations))
                {
                    foreach (var operation in operations.UnnamedValues!)
                    {
                        if (operation.Type != nameof(OperationQuota))
                        {
                            context.Report(Diagnostic.Create(
                                CompilationErrors.PolicyArgumentIsNotOfRequiredType,
                                operation.Node.GetLocation(),
                                "quota-by-key.api.operation",
                                nameof(OperationQuota)
                            ));
                            continue;
                        }

                        if (Handle(context, "operation", operation, out var operationElement))
                        {
                            apiElement.Add(operationElement);
                        }
                    }
                }
            }
        }

        context.AddPolicy(element);
    }

    private bool Handle(ICompilationContext context, string name, InitializerValue value, out XElement element)
    {
        element = new XElement(name);
        var values = value.NamedValues!;

        var isNameAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Name), "name");
        var isIdAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Id), "id");

        if (!isNameAdded && !isIdAdded)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.AtLeastOneOfTwoShouldBeDefined,
                value.Node.GetLocation(),
                name,
                nameof(EntityQuotaConfig.Name),
                nameof(EntityQuotaConfig.Id)
            ));
            return false;
        }

        var isCallsAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Calls), "calls");
        var isBandwidthAdded = element.AddAttribute(values, nameof(EntityQuotaConfig.Bandwidth), "bandwidth");

        if (!isCallsAdded && !isBandwidthAdded)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.AtLeastOneOfTwoShouldBeDefined,
                value.Node.GetLocation(),
                name,
                nameof(EntityQuotaConfig.Calls),
                nameof(EntityQuotaConfig.Bandwidth)
            ));
            return false;
        }

        return true;
    }
}
