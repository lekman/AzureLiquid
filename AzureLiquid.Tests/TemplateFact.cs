// <copyright file="TemplateFact.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

namespace AzureLiquid.Tests;

/// <summary>
/// A fact check for a rendered template, with data content and expected result.
/// </summary>
/// <typeparam name="T">The underlying type of the data content.</typeparam>
public class TemplateFact<T>
{
    /// <summary>
    /// Gets the template.
    /// </summary>
    /// <value>
    /// The template.
    /// </value>
    public string? Template { get; init; }

    /// <summary>
    /// Gets the expected result.
    /// </summary>
    /// <value>
    /// The expected result.
    /// </value>
    public string? Expected { get; init; }

    /// <summary>
    /// Gets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    public T? Content { get; init; }
}