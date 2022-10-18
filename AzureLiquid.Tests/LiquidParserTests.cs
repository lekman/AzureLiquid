// <copyright file="LiquidParserTests.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-18 07:48
// </copyright>

using System.Text.RegularExpressions;
using FluentAssertions;
using Xunit;

namespace AzureLiquid.Tests
{
    /// <summary>
    /// Test fixture for verifying the <see cref="LiquidParser"/> class.
    /// </summary>
    public partial class LiquidParserTests
    {
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
        /// Ensures the basic parsing works from a simple object.
        /// </summary>
        [Fact]
        public void EnsureBasicParsing()
        {
            // Arrange
            var instance = new Arrangement();

            // Act
            var result = new LiquidParser()
                .SetContent(instance.Basic, true)
                .Parse("{{ content.title }}")
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.Basic.Title, "The expected result should be returned");
        }

        /// <summary>
        /// Ensures the deep parsing works with nested objects.
        /// </summary>
        [Fact]
        public void EnsureDeepParsing()
        {
            // Arrange
            var instance = new Arrangement().Deep;

            // Act
            var result = new LiquidParser()
                .SetContent(instance.Content, true)
                .Parse(instance.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.Expected, "The expected result should be returned");
        }

        /// <summary>
        /// Ensures the template parsing works when using a liquid file.
        /// </summary>
        [Fact]
        public void EnsureTemplateParsing()
        {
            // Arrange
            var instance = new Arrangement().SimpleTemplate;

            // Act
            var result = new LiquidParser()
                .SetContent(instance.Content)
                .Parse(instance.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.Expected, "The expected result should be returned");
        }

        /// <summary>
        /// Ensures loading JSON from a file and parsing with a liquid file template works.
        /// </summary>
        [Fact]
        public void EnsureJsonBodyTemplateParsing()
        {
            // Arrange
            var instance = new Arrangement().Event;

            // Act
            var result = new LiquidParser()
                .SetContentJson(instance.Content!)
                .Parse(instance.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            CompareTextsNoWhitespace(result, instance.Expected!).Should()
                .BeTrue("The expected result should be returned");
        }

        [Fact]
        public void EnsureXmlStringParsing()
        {
            // Arrange
            var instance = new Arrangement().Albums;

            // Act
            var result = new LiquidParser()
                .SetContentXml(instance.Content!)
                .Parse(instance.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            CompareTextsNoWhitespace(result, instance.Expected!).Should()
                .BeTrue("The expected result should be returned");
        }
    }
}