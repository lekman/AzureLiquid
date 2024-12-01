namespace AzureLiquid.Tests;

/// <summary>
/// A fact check for a rendered template, with data content and expected result.
/// </summary>
/// <typeparam name="T">The underlying type of the data content.</typeparam>
public class TemplateFact<T>
{
    /// <summary>
    /// Gets or sets the template.
    /// </summary>
    /// <value>
    /// The template.
    /// </value>
    public string? Template { get; set; }

    /// <summary>
    /// Gets or sets the expected result.
    /// </summary>
    /// <value>
    /// The expected result.
    /// </value>
    public string? Expected { get; set; }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    public T? Content { get; set; }
}