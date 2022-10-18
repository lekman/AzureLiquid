// <copyright file="PreviewProcessTests.cs"">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Tests
// Created: 2022-10-17 05:19
// </copyright>

using System.IO;
using System.Reflection;
using FluentAssertions;
using AzureLiquid.Preview;
using Xunit;

namespace AzureLiquid.Tests
{
    /// <summary>
    /// Test fixture for verifying the <see cref="PreviewProcess"/> class. 
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
        /// Contains arranged values used for testing, containing mock instances and expected return values.
        /// </summary>
        private class Arrangement
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Arrangement"/> class.
            /// </summary>
            public Arrangement()
            {
                Preview = new PreviewProcess
                {
                    Template = GetPath("./Resources/event.liquid"),
                    Content = GetPath("./Resources/event.json"),
                    Output = GetPath("./Resources/preview.txt")
                };
            }

            /// <summary>
            /// Gets the preview process object instance.
            /// </summary>
            /// <value>
            /// The preview process.
            /// </value>
            public PreviewProcess Preview { get; }

            /// <summary>
            /// Gets the full path.
            /// </summary>
            /// <param name="path">The relative path.</param>
            /// <returns>Full path.</returns>
            private static string GetPath(string path)
            {
                var basePath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location)!;
                return Path.GetFullPath(path, basePath);
            }
        }
    }
}