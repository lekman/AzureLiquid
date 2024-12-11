# Azure Liquid

The project was primarily built to aid in developing and testing Liquid template parsing solutions for Microsoft Azure
cloud services.

The Liquid template engine that is used in Microsoft Azure is based on
the [DotLiquid](https://github.com/dotliquid/dotliquid) library.

> DotLiquid is a .Net port of the popular Ruby Liquid templating language. It is a separate project that aims to retain
> the same template syntax as the original, while using .NET coding conventions where possible. For more information
> about
> the original Liquid project, see [https://shopify.github.io/liquid/](https://shopify.github.io/liquid/).

This library uses my [.NET port](https://github.com/lekman/dotliquid-net6) of the same library, to allow for
cross-platform compilation and tooling support.

Azure uses a set of predefined feature uses of DotLiquid. For example, an Azure Logic App mapping service uses the "
content" accessor for any data submitted using a workflow action.
The [LiquidParser](https://github.com/lekman/AzureLiquid/blob/main/AzureLiquid/LiquidParser.cs") class exposes a set of
SetContent methods used to either set:

- objects (will render down to JSON)
- JSON string
- XML string (will parse as XDocument then to JSON)

The object can then be accessed under the "content" variable in the Liquid template.

```html
{% assign albums = content.CATALOG.CD -%} [{%- for album in albums limit:3 %}  
{     "artist": "{{ album.ARTIST }}",     "title": "{{ album.TITLE}}"   }{% if
forloop.last == false %},{% endif %}   {%- endfor -%} ]
```

Our object data is in this case XML, and has been added as a hierarchical selector object, here named "CATALOG",
containing an array of "CD" objects. These are loaded under the hood by parsing text to an XmlDocument then back to JSON
using the **SetContentXml** method.

Similarly, we can load JSON data either using a string with the **SetContentJson** method, or using object serialization
with the **SetContent** method. Note this method's parameter **forceCamlCase** allows us to ensure that camel JSON
formatting is preserved in the selectors.

## Azure Specific Differences

For example, an Azure LogicApp mapping service uses the content accessor. The LiquidParser exposes a set of SetContent
methods used to either set:

- objects (will render down to JSON)
- JSON string
- XML string (will parse as XDocument then to JSON)

The object can then be accessed under the 'content' variable in the Liquid template. This is implemented by using the
LiquidParser object and using it for unit testing or for live previews/file rendering.

A key within this project is that I created it to be as compatible and usable for Azure implementations as possible.
Therefore, it is important to understand how the DotLiquid and Azure implementations of the library differ from Shopify
Liquid.

- Liquid templates follow
  the [file size limits for maps](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-limits-and-config#artifact-capacity-limits)
  in Azure Logic Apps.
- When using the date filter, DotLiquid supports both Ruby and .NET date format strings (but not both at the same time).
  By default, it will use [.NET date format strings](<http://msdn.microsoft.com/en-us/library/8kb3ddd4(v=vs.110).aspx>).
- The JSON filter from the Shopify extension filters is
  currently [not implemented in DotLiquid](https://github.com/dotliquid/dotliquid/issues/384).

- The standard Replace filter in the DotLiquid implementation uses regular expression (RegEx) matching, while the
  Shopify implementation uses simple string matching.

- Liquid by default uses Ruby casing for output fields and filters such as {{ some_field | escape }}. The Azure
  implementation of DotLiquid uses C# naming convention, in which case output fields and filters would be referenced
  like so {{ SomeField | Escape }}.

For further details, see
the [Microsoft documentation](https://learn.microsoft.com/en-us/azure/logic-apps/logic-apps-enterprise-integration-liquid-transform?tabs=consumption#liquid-template-considerations).

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

[See the full example](https://github.com/lekman/Liquid.Parser/blob/main/Liquid.Tests/LiquidParserTests.cs#L22)

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

You could similarly load the content and templates from a set of file. See
the [full test project](https://github.com/lekman/Liquid.Parser/tree/main/Liquid.Tests) for several such examples.