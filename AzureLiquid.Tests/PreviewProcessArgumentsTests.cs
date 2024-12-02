using System.IO;
using Xunit;
using FluentAssertions;

namespace AzureLiquid.Preview.Tests
{
    public class PreviewProcessArgumentsTests
    {
        [Theory]
        [InlineData(new string[] { "--template", "template.liquid" }, "template", 0)]
        [InlineData(new string[] { "--content", "content.json" }, "content", 0)]
        [InlineData(new string[] { "--output", "output.txt" }, "output", 0)]
        [InlineData(new string[] { "--watch" }, "watch", 0)]
        [InlineData(new string[] { "--template", "template.liquid" }, "content", -1)]
        public void GetArgumentIndex_ShouldReturnCorrectIndex(string[] args, string key, int expectedIndex)
        {
            // Act
            var index = PreviewProcessArguments.GetArgumentIndex(args, key);

            // Assert
            index.Should().Be(expectedIndex);
        }

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

        [Theory]
        [InlineData(new string[] { "--template", "template.liquid" }, "template", true)]
        [InlineData(new string[] { "--content", "content.json" }, "content", true)]
        [InlineData(new string[] { "--output", "output.txt" }, "output", true)]
        [InlineData(new string[] { "--watch" }, "watch", true)]
        [InlineData(new string[] { "--template", "template.liquid" }, "content", false)]
        public void HasArgument_ShouldReturnCorrectResult(string[] args, string key, bool expectedResult)
        {
            // Act
            var result = PreviewProcessArguments.HasArgument(args, key);

            // Assert
            result.Should().Be(expectedResult);
        }

        [Theory]
        [InlineData(new string[] { "--template", "template.liquid" }, "template", "template.liquid")]
        [InlineData(new string[] { "--content", "content.json" }, "content", "content.json")]
        [InlineData(new string[] { "--output", "output.txt" }, "output", "output.txt")]
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
}