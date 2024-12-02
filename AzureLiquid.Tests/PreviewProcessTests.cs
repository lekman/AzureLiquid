// <copyright file="PreviewProcessTests.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

using System.IO;
using System.Reflection;
using AzureLiquid.Preview;
using FluentAssertions;
using Xunit;

namespace AzureLiquid.Tests;

/// <summary>
/// Test fixture for verifying the <see cref="PreviewProcess" /> class.
/// </summary>
public class PreviewProcessTests
{
    /// <summary>
    /// Ensures the basic parsing works from a set of templates and that output files are updated.
    /// </summary>
    [Fact]
    public void EnsurePreviewParsing()
    {
        // Arrange
        var instance = new Arrangement().Preview;

        // Act
        var result = instance.Render();
        var fileContent = File.ReadAllText(instance.Output);

        // Assert
        result.Should().NotBeEmpty("A result should have been returned");
        result.Should().Be(fileContent, "The expected result should be written to output file");
    }

    /// <summary>
    /// Ensures XML parsing works.
    /// </summary>
    [Fact]
    public void EnsurePreviewXml()
    {
        // Arrange
        var instance = new Arrangement().XmlPreview;

        // Act
        var result = instance.Render();
        var fileContent = File.ReadAllText(instance.Output);

        // Assert
        result.Should().NotBeEmpty("A result should have been returned");
        result.Should().Be(fileContent, "The expected result should be written to output file");
    }

    /// <summary>
    /// Ensures argument parsing works according to the CSharp specification.
    /// </summary>
    [Fact]
    public void EnsurePreviewParsingCSharpArguments()
    {
        // Arrange
        var instance = new Arrangement().ArgumentPreview;

        // Act
        var result = instance.Render();
        var fileContent = File.ReadAllText(instance.Output);

        // Assert
        result.Should().NotBeEmpty("A result should have been returned");
        result.Should().Be(fileContent, "The expected result should be written to output file");
    }

    /// <summary>
    /// Ensure the preview process can be created from a set of arguments.
    /// </summary>
    /// <param name="shouldLog">Determine if a log should be produced.</param>
    /// <param name="arg1">First argument.</param>
    /// <param name="arg2">Second argument.</param>
    /// <param name="arg3">Third argument.</param>
    /// <param name="arg4">Fourth argument.</param>
    /// <param name="arg5">Fifth argument.</param>
    /// <param name="arg6">Sixth argument.</param>
    /// <param name="arg7">Seventh argument.</param>
    /// <param name="arg8">Eighth argument.</param>
    [Theory]
    [InlineData(false, "", "", "", "", "", "", "", "")]
    [InlineData(false, "--template", "./Resources/event.liquid", "--content", "./Resources/event.json", "--output",
        "./Resources/preview.txt", "", "")]
    [InlineData(false, "--template", "./Resources/event.liquid", "", "", "", "", "", "")]
    [InlineData(false, "--watch", "", "", "", "", "", "", "")]
    [InlineData(true, "--help", "", "", "", "", "", "", "")]
    [InlineData(true, "--template", "./Resources/event_not_found.liquid", "--content", "./Resources/event.xml",
        "--output", "./Resources/preview.txt", "", "")]
    public void EnsureArgumentParsing(bool shouldLog, string arg1, string arg2, string arg3, string arg4, string arg5,
        string arg6, string arg7, string arg8)
    {
        // Arrange
        var args = new[] { arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8 };

        // Act
        var preview = PreviewProcess.Create(args);

        // Assert
        preview.Should().NotBeNull("A preview process should have been created");

        if (shouldLog)
        {
            preview.Log.Should().NotBeEmpty("A log should have been created");
        }
        else
        {
            preview.Log.Should().BeEmpty("No log should have been created");
        }
    }

    [Fact]
    public void EnsureObjectCreation()
    {
        // Arrange
        var instance = new PreviewProcess
        {
            Template = Arrangement.GetPath("./Resources/albums.liquid"),
            Content = Arrangement.GetPath("./Resources/albums.xml"),
            Output = Arrangement.GetPath("./Resources/albums.json")
        };

        // Act
        instance.Render();

        // Assert
        instance.Log.Should().BeEmpty("A log should not have been created");
        instance.Template.Should().NotBeEmpty("The template should be empty");
        instance.Content.Should().NotBeEmpty("The content should be empty");
        instance.Output.Should().NotBeEmpty("The output should be empty");
    }

    /// <summary>
    /// Ensure the preview process can use a file watcher.
    /// </summary>
    [Fact]
    public void EnsureWatcher()
    {
        // Arrange
        var instance = PreviewProcess.Create([
            "--template", "./Resources/event.liquid", "--content", "./Resources/event.json", "--output",
            "./Resources/preview.txt"
        ]);

        // Act
        instance.StartWatch();
        instance.StopWatch();

        // Assert
        instance.Log.Should().NotBeEmpty("A log should have been created");
    }

    #region Nested type: Arrangement

    /// <summary>
    /// Contains arranged values used for testing, containing mock instances and expected return values.
    /// </summary>
    private class Arrangement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Arrangement" /> class.
        /// </summary>
        public Arrangement()
        {
            Preview = new PreviewProcess
            {
                Template = GetPath("./Resources/event.liquid"),
                Content = GetPath("./Resources/event.json"),
                Output = GetPath("./Resources/preview.txt")
            };

            ArgumentPreview = new PreviewProcess
            {
                Template = GetPath("./Resources/append.liquid"),
                Content = GetPath("./Resources/append.100.json"),
                Output = GetPath("./Resources/append.temp.txt")
            };

            XmlPreview = new PreviewProcess
            {
                Template = GetPath("./Resources/albums.liquid"),
                Content = GetPath("./Resources/albums.xml"),
                Output = GetPath("./Resources/albums.json")
            };
        }

        /// <summary>
        /// Gets the preview process object instance.
        /// </summary>
        /// <value>
        /// The preview process.
        /// </value>
        public PreviewProcess ArgumentPreview { get; }

        /// <summary>
        /// Gets the preview process object instance.
        /// </summary>
        /// <value>
        /// The preview process.
        /// </value>
        public PreviewProcess Preview { get; }

        /// <summary>
        /// Gets the preview process object instance.
        /// </summary>
        /// <value>
        /// The preview process.
        /// </value>
        public PreviewProcess XmlPreview { get; }

        /// <summary>
        /// Gets the full path.
        /// </summary>
        /// <param name="path">The relative path.</param>
        /// <returns>Full path.</returns>
        public static string GetPath(string path)
        {
            var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
            return Path.GetFullPath(path, basePath);
        }
    }

    #endregion
}