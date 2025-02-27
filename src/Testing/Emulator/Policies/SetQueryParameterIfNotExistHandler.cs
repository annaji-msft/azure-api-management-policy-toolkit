// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

using Azure.ApiManagement.PolicyToolkit.Authoring;

namespace Azure.ApiManagement.PolicyToolkit.Testing.Emulator.Policies;

[Section(nameof(IInboundContext))]
internal class SetQueryParameterIfNotExistHandler : PolicyHandler<string, string[]>
{
    public override string PolicyName => nameof(IInboundContext.SetQueryParameterIfNotExist);

    protected override void Handle(GatewayContext context, string name, string[] values) =>
        context.Request.Url.Query.TryAdd(name, values);
}