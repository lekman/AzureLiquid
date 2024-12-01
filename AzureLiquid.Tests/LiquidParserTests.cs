// <copyright file="LiquidParserTests.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace AzureLiquid.Tests;

/// <summary>
/// Test fixture for verifying the <see cref="LiquidParser" /> class.
/// </summary>
public class LiquidParserTests
{
    /// <summary>
    /// Ensures the basic parsing works from a simple object.
    /// </summary>
    [Fact]
    public void EnsureBasicParsing()
    {
        // Arrange
        var arrangement = new Arrangement();

        // Act
        var result = new LiquidParser()
            .SetContent(arrangement.Basic, true)
            .Parse("{{ content.title | Prepend: \"Page title - \" }}")
            .Render();

        // Assert
        result.Should().NotBeEmpty("A result should have been returned");
        result.Should().Be("Page title - " + arrangement.Basic.Title, "The expected result should be returned");
    }

    /// <summary>
    /// Ensures the deep parsing works with nested objects.
    /// </summary>
    [Fact]
    public void EnsureDeepParsing()
    {
        Arrange(new Arrangement().Deep, true);
    }

    /// <summary>
    /// Ensures the template parsing works when using a liquid file.
    /// </summary>
    [Fact]
    public void EnsureTemplateParsing()
    {
        Arrange(new Arrangement().SimpleTemplate);
    }

    /// <summary>
    /// Ensures loading JSON from a file and parsing with a liquid file template works.
    /// </summary>
    [Fact]
    public void EnsureJsonBodyTemplateParsing()
    {
        Arrange(new Arrangement().Event);
    }

    /// <summary>
    /// Compares two text snippets but ignores differences in whitespace.
    /// </summary>
    /// <param name="text1">The first text.</param>
    /// <param name="text2">The second text.</param>
    /// <returns><c>true</c> if the texts match, otherwise <c>false</c>.</returns>
    private static bool CompareTextsNoWhitespace(string text1, string text2)
    {
        var spaces = new Regex(@"[\s]*");
        return string.CompareOrdinal(spaces.Replace(text1, string.Empty), spaces.Replace(text2, string.Empty)) == 0;
    }

    /// <summary>
    /// Creates a <see cref="LiquidParser" /> instance with the specified template fact.
    /// </summary>
    /// <typeparam name="T">The type of the template fact content.</typeparam>
    /// <param name="fact">The template fact to use.</param>
    /// <param name="forceCamelCase">Whether to force camel case on the content.</param>
    private static LiquidParser CreateParser<T>(TemplateFact<T> fact, bool forceCamelCase = false)
    {
        var parser = new LiquidParser();

        if (fact.Content is string content)
        {
            if (content!.Contains("<?xml"))
            {
                parser.SetContentXml(content);
            }
            else
            {
                parser.SetContentJson(content);
            }
        }

        if (fact.Content is not string)
        {
            parser.SetContent(fact.Content, forceCamelCase);
        }

        return parser;
    }

    /// <summary>
    /// Ensures loading XML or JSON from a file and parsing with a liquid file template works.
    /// </summary>
    /// <typeparam name="T">The type of the template fact content.</typeparam>
    /// <param name="fact">The template fact to test.</param>
    /// <param name="forceCamelCase">Whether to force camel case on the content.</param>
    private static void Arrange<T>(TemplateFact<T> fact, bool forceCamelCase = false)
    {
        // Arrange
        var parser = CreateParser(fact, forceCamelCase);

        // Act
        var result = parser.Parse(fact.Template).Render();

        // Assert
        result.Should().NotBeEmpty("A result should have been returned");
        CompareTextsNoWhitespace(result, fact.Expected!).Should()
            .BeTrue("The expected result should be returned");
    }
}