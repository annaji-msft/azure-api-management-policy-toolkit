﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record CacheLookupValueConfig
{
    public required string Key { get; init; }
    public required string VariableName { get; init; }
    public object? DefaultValue { get; init; }
    public string? CachingType { get; init; }
}