using System.Xml.Linq;
using Azure.Api.Management.PolicyToolkit.Authoring;
using Azure.Api.Management.PolicyToolkit.Compiling.Diagnostics;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Azure.Api.Management.PolicyToolkit.Compiling.Policy;

public class ValidateParametersCompiler : IMethodPolicyHandler
{
    public string MethodName => nameof(IInboundContext.ValidateParameters);

    public void Handle(ICompilationContext context, InvocationExpressionSyntax node)
    {
        if (!node.TryExtractingConfigParameter<ValidateParametersConfig>(context, "validate-parameters", out var values))
        {
            return;
        }

        var element = new XElement("validate-parameters");

        if (!element.AddAttribute(values, nameof(ValidateParametersConfig.Name), "name"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.Name)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateParametersConfig.FailCheckHttpCode), "failed-check-httpcode"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.FailCheckHttpCode)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateParametersConfig.FailCheckErrorMessage),
                "failed-check-error-message"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.FailCheckErrorMessage)
            ));
            return;
        }

        if (!element.AddAttribute(values, nameof(ValidateParametersConfig.IgnoreCase), "ignore-case"))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.IgnoreCase)
            ));
            return;
        }

        if (!values.TryGetValue(nameof(ValidateParametersConfig.Values), out var parameterValues))
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterNotDefined,
                node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.Values)
            ));
            return;
        }

        var elements = (parameterValues.UnnamedValues ?? [])
            .Select(origin => new XElement("value", origin.Value!))
            .ToArray<object>();
        if (elements.Length == 0)
        {
            context.Report(Diagnostic.Create(
                CompilationErrors.RequiredParameterIsEmpty,
                parameterValues.Node.GetLocation(),
                "validate-parameters",
                nameof(ValidateParametersConfig.Values)
            ));
            return;
        }

        element.Add(elements);

        context.AddPolicy(element);
    }
}
