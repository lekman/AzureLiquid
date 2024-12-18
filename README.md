# Azure Liquid

[![NuGet](https://img.shields.io/nuget/v/AzureLiquid.svg)](https://www.nuget.org/packages/AzureLiquid)
[![Code Scanning](https://github.com/lekman/AzureLiquid/actions/workflows/codeql.yml/badge.svg)](https://github.com/lekman/AzureLiquid/actions/workflows/codeql.yml)
[![Qodana](https://github.com/lekman/AzureLiquid/actions/workflows/qodana_code_quality.yml/badge.svg)](https://github.com/lekman/AzureLiquid/actions/workflows/qodana_code_quality.yml)
[![Coverage](https://codecov.io/gh/lekman/AzureLiquid/branch/main/graph/badge.svg?token=6449B7XRCS)](https://codecov.io/gh/lekman/AzureLiquid)

Allows programmatic parsing, unit testing and live preview of Liquid templates, specifically designed for the Azure cloud services.

The project was primarily built to aid in developing and testing Liquid template parsing solutions for Microsoft Azure cloud services.

The Liquid template engine that is used in Microsoft Azure is based on the [DotLiquid](https://github.com/dotliquid/dotliquid) library.

> DotLiquid is a .Net port of the popular Ruby Liquid template language. It is a separate project that aims to retain the same template syntax as the original, while using .NET coding conventions where possible. For more information about the original Liquid project, see [https://shopify.github.io/liquid/](https://shopify.github.io/liquid/).

This library uses my [.NET port](https://github.com/lekman/dotliquid-net6) of the same library, to allow for cross-platform compilation and tooling support.

Azure uses a set of predefined feature uses of DotLiquid. For example, an Azure Logic App mapping service uses the "content" accessor for any data submitted using a workflow action. The [LiquidParser](https://github.com/lekman/AzureLiquid/blob/main/AzureLiquid/LiquidParser.cs") class exposes a set of SetContent methods used to either set:

- objects (will render down to JSON)
- JSON string
- XML string (will parse as XDocument then to JSON)

The object can then be accessed under the "content" variable in the Liquid template.

```jinja
{% assign albums = content.CATALOG.CD -%} [{%- for album in albums limit:3 %}  
{     "artist": "{{ album.ARTIST }}",     "title": "{{ album.TITLE}}"   }{% if
forloop.last == false %},{% endif %}   {%- endfor -%} ]
```

Our object data is in this case XML, and has been added as a hierarchical selector object, here named "CATALOG", containing an array of "CD" objects. These are loaded under the hood by parsing text to an XmlDocument then back to JSON using the **SetContentXml** method.

Similarly, we can load JSON data either using a string with the **SetContentJson** method, or using object serialization with the **SetContent** method. Note this method's parameter **forceCamlCase** allows us to ensure that camel JSON formatting is preserved in the selectors.

## Azure Specific Differences

It is important to understand how the DotLiquid implementations of the library differ from Shopify Liquid.

- Liquid templates follow the [file size limits for maps](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-limits-and-config#artifact-capacity-limits) in Azure Logic Apps.
- When using the date filter, DotLiquid supports both Ruby and .NET date format strings (but not both at the same time). By default, it will use [.NET date format strings](<http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx>).
- The JSON filter from the Shopify extension filters is currently [not implemented in DotLiquid](https://github.com/dotliquid/dotliquid/issues/384).

- The standard Replace filter in the DotLiquid implementation uses regular expression (RegEx) matching, while the Shopify implementation uses simple string matching.

- Liquid by default uses Ruby casing for output fields and filters such as {{ some_field | escape }}. The Azure implementation of DotLiquid uses C# naming convention, in which case output fields and filters would be referenced like so {{ SomeField | Escape }}.

For further details, see the [Microsoft documentation](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-enterprise-integration-liquid-transform?tabs=consumption#liquid-template-considerations).

## How To Unit Test Liquid

There are a few different examples made:

```csharp
// Arrange
var myObj = new MyObj( Title = "Title here");

// Act
var result = new LiquidParser()
    .SetContent(myObj, true) // 'true' => camelCase property names
    .Parse("{{ content.title }}")
    .Render();

// Assert
result.Should().NotBeEmpty("A result should have been returned");
result.Should().Be(myObj.Title, "The expected result should be returned");
```

[See the full example](https://github.com/lekman/AzureLiquid/blob/feature/nuget-release/AzureLiquid.Tests/LiquidParserTests.cs)

Another example can be made where we use a string body to transform the data.

```csharp
/// <summary>
/// Ensures loading JSON from a file and parsing with a liquid file template works.
/// </summary>
[Fact]
public void EnsureJsonBodyTemplateParsing()
{
    // Arrange
    var parser = new LiquidParser()
        .SetContentJson(Resources.JsonTestContent)
        .Parse(Resources.JsonTestLiquidTemplate)
    var expected = Resources.TestExpectedOutput;

    // Act
    var result = parser.Render();

    // Assert
    result.Should().NotBeEmpty("A result should have been returned");
    result.Should().Be(expected, "The expected result should be returned");
}
```

You could similarly load the content and templates from a set of file. See the [full test project](https://github.com/lekman/Liquid.Parser/tree/main/Liquid.Tests) for several such examples.

## Enabling Live Preview During Development

Using the Live Preview console application, we can set a file watcher that automatically renders the output as any changes occur to the source data or Liquid template.

Install the tool using Nuget and register as a global dotnet tool.

```bash
dotnet tool install -g AzureLiquid.Preview
```

To update to a newer version, use:

```bash
dotnet tool update -g AzureLiquid.Preview
```

The process is started from the terminal using a set of arguments.

| Argument   | Description                                                               |
| ---------- | ------------------------------------------------------------------------- |
| --help     | Shows help description within the console                                 |
| --watch    | Switch parameter to enable file watcher and produce output on file update |
| --template | Relative path to the .liquid template source file                         |
| --content  | Relative path to the XML or JSON data source file                         |
| --output   | Relative path to the output result file                                   |

For example:

```bash
liquidpreview --watch --content ./albums.xml --template ./albums.liquid --output ./albums.json
```

I have simply arranged the three files in VSCode and get preview on the right hand side whenever the template or XML source is changed.

I then use the terminal to start the watcher. The albums.json file is generated automatically and kept up to date whenever the source data or liquid template is changed.

![VSCode preview](Documentation/live-preview-console-vscode.png)
