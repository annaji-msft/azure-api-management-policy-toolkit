﻿// Copyright (c) Microsoft Corporation.
// Licensed under the MIT License.

namespace Azure.ApiManagement.PolicyToolkit.Authoring;

public record BodyConfig : SetBodyConfig
{
    public required object? Content { get; init; }
}