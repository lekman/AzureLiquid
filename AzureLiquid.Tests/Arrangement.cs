// <copyright file="Arrangement.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-18 07:46
// </copyright>

using AzureLiquid.Tests.Resources;

namespace AzureLiquid.Tests;

/// <summary>
/// Contains arranged values used for testing, containing mock instances and expected return values.
/// </summary>
public class Arrangement
{
    /// <summary>
    /// Initializes a new instance of the <see cref="Arrangement"/> class.
    /// </summary>
    public Arrangement()
    {
        Basic = new BasicObject("John Doe");
        Deep = new TemplateFact<DeepObject>
        {
            Content = new DeepObject("Jane Doe"),
            Template = "<p>{{content.nested.title}}</p>",
            Expected = "<p>Jane Doe</p>"
        };

        var simple = "Simple Template";
        SimpleTemplate = new TemplateFact<BasicObject>
        {
            Content = new BasicObject(simple),
            Template = Templates.SimpleTemplate,
            Expected = Templates.SimpleResult
        };

        Event = new TemplateFact<string>
        {
            Content = Templates.EventContent,
            Template = Templates.EventTemplate,
            Expected = Templates.EventResult
        };

        Albums = new TemplateFact<string>
        {
            Content = Templates.AlbumsContent,
            Template = Templates.AlbumsTemplate,
            Expected = Templates.AlbumsResult
        };
    }

    /// <summary>
    /// Gets the albums fact.
    /// </summary>
    public TemplateFact<string> Albums { get; }

    /// <summary>
    /// Gets the event fact.
    /// </summary>
    public TemplateFact<string> Event { get; }

    /// <summary>
    /// Gets the simple template fact.
    /// </summary>
    public TemplateFact<BasicObject> SimpleTemplate { get; }

    /// <summary>
    /// Gets the deep object fact.
    /// </summary>
    public TemplateFact<DeepObject> Deep { get; }

    /// <summary>
    /// Gets the basic object fact.
    /// </summary>
    public BasicObject Basic { get; }

}
