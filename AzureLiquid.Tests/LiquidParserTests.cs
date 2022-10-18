// <copyright file="LiquidParserTests.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-13 13:22
// </copyright>

using AzureLiquid;
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
            var instance = new Arrangement();

            // Act
            var result = new LiquidParser()
                .SetContent(instance.Deep.Content, true)
                .Parse(instance.Deep.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.Deep.Expected, "The expected result should be returned");
        }

        /// <summary>
        /// Ensures the template parsing works when using a liquid file.
        /// </summary>
        [Fact]
        public void EnsureTemplateParsing()
        {
            // Arrange
            var instance = new Arrangement();

            // Act
            var result = new LiquidParser()
                .SetContent(instance.SimpleTemplate.Content)
                .Parse(instance.SimpleTemplate.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.SimpleTemplate.Expected, "The expected result should be returned");
        }

        /// <summary>
        /// Ensures loading JSON from a file and parsing with a liquid file template works.
        /// </summary>
        [Fact]
        public void EnsureJsonBodyTemplateParsing()
        {
            // Arrange
            var instance = new Arrangement();

            // Act
            var result = new LiquidParser()
                .SetContentJson(instance.Event.Content!)
                .Parse(instance.Event.Template)
                .Render();

            // Assert
            result.Should().NotBeEmpty("A result should have been returned");
            result.Should().Be(instance.Event.Expected, "The expected result should be returned");
        }
    }
}