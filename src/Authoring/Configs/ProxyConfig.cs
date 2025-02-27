﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record ProxyConfig
{
    public required string Url { get; init; }
    public string? Username { get; init; }
    public string? Password { get; init; }
}