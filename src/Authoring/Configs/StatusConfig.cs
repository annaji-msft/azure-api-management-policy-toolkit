namespace Mielek.Azure.ApiManagement.PolicyToolkit.Authoring;

/// <summary>
/// Configuration to set the HTTP status code and reason to the specified value
/// </summary>
public record StatusConfig
{
    /// <summary>
    ///  The HTTP status code to return. Policy expressions are allowed.
    /// </summary>
    public required uint Code { get; init; }
    
    /// <summary>
    /// A description of the reason for returning the status code. Policy expressions are allowed.
    /// </summary>
    public required string Reason { get; init; }
};