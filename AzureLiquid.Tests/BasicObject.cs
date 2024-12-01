// <copyright file="BasicObject.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-18 07:46
// </copyright>

namespace AzureLiquid.Tests;

/// <summary>
/// Basic test of an object to serialize and render.
/// </summary>
/// <remarks>
/// Initializes a new instance of the <see cref="BasicObject"/> class.
/// </remarks>
/// <param name="title">The title.</param>
public class BasicObject(string title)
{
    /// <summary>
    /// Gets the title.
    /// </summary>
    /// <value>
    /// The title.
    /// </value>
    public string Title { get; } = title;
}
