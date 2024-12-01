// <copyright file="DeepObject.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-18 07:46
// </copyright>

namespace AzureLiquid.Tests;

/// <summary>
/// Test sample for an object with nested types, ensure that deeper accessors are valid.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="DeepObject"/> class.
/// </remarks>
/// <param name="title">The title.</param>
public class DeepObject(string title)
{
    /// <summary>
    /// Gets the nested object.
    /// </summary>
    /// <value>
    /// The nested object.
    /// </value>
    public BasicObject Nested { get; } = new BasicObject(title);
}
