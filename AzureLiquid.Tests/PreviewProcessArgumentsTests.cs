// <copyright file="PreviewProcessArgumentsTests.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

using AzureLiquid.Preview;
using FluentAssertions;
using Xunit;

namespace AzureLiquid.Tests;

/// <summary>
/// Unit tests for the <see cref="PreviewProcessArguments" /> class.
/// </summary>
public class PreviewProcessArgumentsTests
{
    /// <summary>
    /// Tests the <see cref="PreviewProcessArguments.GetArgumentIndex" /> method.
    /// </summary>
    /// <param name="args">The array of arguments.</param>
    /// <param name="key">The key to search for.</param>
    /// <param name="expectedIndex">The expected index of the key in the arguments array.</param>
    [Theory]
    [InlineData(new[] { "--template", "template.liquid" }, "template", 0)]
    [InlineData(new[] { "--content", "content.json" }, "content", 0)]
    [InlineData(new[] { "--output", "output.txt" }, "output", 0)]
    [InlineData(new[] { "--watch" }, "watch", 0)]
    [InlineData(new[] { "--template", "template.liquid" }, "content", -1)]
    public void GetArgumentIndex_ShouldReturnCorrectIndex(string[] args, string key, int expectedIndex)
    {
        // Act
        var index = PreviewProcessArguments.GetArgumentIndex(args, key);

        // Assert
        index.Should().Be(expectedIndex);
    }

    /// <summary>
    /// Tests the <see cref="PreviewProcessArguments.IsArgMatch" /> method.
    /// </summary>
    /// <param name="arg">The argument to check.</param>
    /// <param name="key">The key to match against.</param>
    /// <param name="expectedResult">The expected result of the match.</param>
    [Theory]
    [InlineData("--template", "template", true)]
    [InlineData("--content", "content", true)]
    [InlineData("--output", "output", true)]
    [InlineData("--watch", "watch", true)]
    [InlineData("--template", "content", false)]
    public void IsArgMatch_ShouldReturnCorrectResult(string arg, string key, bool expectedResult)
    {
        // Act
        var result = PreviewProcessArguments.IsArgMatch(arg, key);

        // Assert
        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Tests the <see cref="PreviewProcessArguments.HasArgument" /> method.
    /// </summary>
    /// <param name="args">The array of arguments.</param>
    /// <param name="key">The key to search for.</param>
    /// <param name="expectedResult">The expected result of the search.</param>
    [Theory]
    [InlineData(new[] { "--template", "template.liquid" }, "template", true)]
    [InlineData(new[] { "--content", "content.json" }, "content", true)]
    [InlineData(new[] { "--output", "output.txt" }, "output", true)]
    [InlineData(new[] { "--watch" }, "watch", true)]
    [InlineData(new[] { "--template", "template.liquid" }, "content", false)]
    public void HasArgument_ShouldReturnCorrectResult(string[] args, string key, bool expectedResult)
    {
        // Act
        var result = PreviewProcessArguments.HasArgument(args, key);

        // Assert
        result.Should().Be(expectedResult);
    }

    /// <summary>
    /// Tests the <see cref="PreviewProcessArguments.ParsePath" /> method.
    /// </summary>
    /// <param name="args">The array of arguments.</param>
    /// <param name="key">The key to search for.</param>
    /// <param name="expectedPath">The expected path associated with the key.</param>
    [Theory]
    [InlineData(new[] { "--template", "template.liquid" }, "template", "template.liquid")]
    [InlineData(new[] { "--content", "content.json" }, "content", "content.json")]
    [InlineData(new[] { "--output", "output.txt" }, "output", "output.txt")]
    public void ParsePath_ShouldReturnCorrectPath(string[] args, string key, string expectedPath)
    {
        // Arrange
        var previewArgs = new PreviewProcessArguments();

        // Act
        var path = previewArgs.ParsePath(args, key);

        // Assert
        path.Should().Contain(expectedPath);
    }
}