// <copyright file="PreviewProcess.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace AzureLiquid.Preview;

/// <summary>
/// Starts a preview of Liquid template rendering results, and optionally continuously render when source files
/// changes.
/// </summary>
public class PreviewProcess
{
    /// <summary>
    /// The argument parser.
    /// </summary>
    private readonly PreviewProcessArguments _args;

    /// <summary>
    /// Handles writing console output to private persisted log.
    /// </summary>
    private readonly StringWriter _writer = new();

    /// <summary>
    /// Detects file system changes to the content file.
    /// </summary>
    private FileSystemWatcher? _contentWatcher;

    /// <summary>
    /// Detects file system changes to the template file.
    /// </summary>
    private FileSystemWatcher? _templateWatcher;

    /// <summary>
    /// Initializes a new instance of the <see cref="PreviewProcess" /> class.
    /// </summary>
    public PreviewProcess()
    {
        _args = new PreviewProcessArguments();
        Template = string.Empty;
        Content = string.Empty;
        Output = "./preview.txt";
    }

    /// <summary>
    /// Gets or sets the template.
    /// </summary>
    /// <value>
    /// The template.
    /// </value>
    public string Template { get; set; }

    /// <summary>
    /// Gets or sets the content.
    /// </summary>
    /// <value>
    /// The content.
    /// </value>
    public string Content { get; set; }

    /// <summary>
    /// Gets the console output from the last render.
    /// </summary>
    /// <returns>The console output.</returns>
    public string Log => _writer.ToString();

    /// <summary>
    /// Gets or sets the output.
    /// </summary>
    /// <value>
    /// The output.
    /// </value>
    public string Output { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether the process should watch for changes to template or content files.
    /// </summary>
    /// <value>
    /// <c>true</c> if the process should watch for changes; otherwise, <c>false</c>.
    /// </value>
    [ExcludeFromCodeCoverage]
    private bool ShouldWatch { get; set; }

    /// <summary>
    /// Gets a value indicating whether this instance can parse the inputs and render the output.
    /// </summary>
    /// <returns>
    /// <c>true</c> if this instance can parse; otherwise, <c>false</c>.
    /// </returns>
    private bool CanRender => File.Exists(Template) && File.Exists(Content) && !string.IsNullOrEmpty(Output);

    /// <summary>
    /// Start a new instance of the <see cref="PreviewProcess" /> class using the incoming arguments.
    /// </summary>
    /// <param name="args">The process arguments.</param>
    /// <returns>A new instance of the <see cref="PreviewProcess" /> class.</returns>
    [ExcludeFromCodeCoverage]
    public static PreviewProcess Create(string[] args)
    {
        var preview = new PreviewProcess();

        preview.Template = preview._args.ParsePath(args, "template");
        preview.Content = preview._args.ParsePath(args, "content");
        preview.Output = preview._args.ParsePath(args, "output");
        preview.ShouldWatch = PreviewProcessArguments.HasArgument(args, "watch");

        HandleNoArgumentsPassed(args, preview);

        if (preview.CanRender)
        {
            RenderAndWatch(preview);
        }
        else
        {
            LogMissingFiles(preview);
        }

        return preview;
    }

    /// <summary>
    /// Renders the output and watches for changes if specified.
    /// </summary>
    /// <param name="preview">The current preview process.</param>
    private static void RenderAndWatch(PreviewProcess preview)
    {
        preview.Render();

        if (!preview.ShouldWatch)
        {
            return;
        }

        preview.StartWatch();
        preview.LogMessage("Press any key to exit file watch...");
        _ = Console.ReadKey();
        preview.StopWatch();
        preview.LogMessage();
    }

    /// <summary>
    /// Logs the missing files if found.
    /// </summary>
    /// <param name="preview">The current preview process.</param>
    private static void LogMissingFiles(PreviewProcess preview)
    {
        if (string.IsNullOrEmpty(preview.Content) || string.IsNullOrEmpty(preview.Template))
        {
            return;
        }

        Console.ForegroundColor = ConsoleColor.Yellow;
        preview.LogMessage("  Unable to render as input files are not found");
        preview.LogMessage();
    }

    /// <summary>
    /// Handles the scenario where no arguments are passed to the application.
    /// </summary>
    /// <param name="args">The array of arguments passed to the application.</param>
    /// <param name="preview">The instance of <see cref="PreviewProcess" /> to handle the output.</param>
    private static void HandleNoArgumentsPassed(string[] args, PreviewProcess preview)
    {
        if (args == null || args.Length == 0)
        {
            preview.WriteHelpOutput();
        }
    }

    /// <summary>
    /// Writes the help output.
    /// </summary>
    private void WriteHelpOutput()
    {
        LogMessage();
        LogMessage("Arguments:");
        LogMessage("  --template     : Relative path to the .liquid template source file");
        LogMessage("  --content      : Relative path to the XML or JSON data source file");
        LogMessage("  --output       : Relative path to the output result file");
        LogMessage(
            "  --watch        : Switch parameter to enable file watcher and produce output on file update");
        LogMessage("  --help         : Switch parameter to show this information");
        LogMessage();
    }

    /// <summary>
    /// Writes an error line.
    /// </summary>
    /// <param name="error">The error.</param>
    [ExcludeFromCodeCoverage]
    private static void WriteErrorLine(string error)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"  {error}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    /// <summary>
    /// Renders the output using the specified properties of the instance.
    /// </summary>
    /// <returns>The output from the template data and content.</returns>
    public string Render()
    {
        if (!CanRender)
        {
            WriteErrorLine("Unable to render as inputs or outputs not found or not specified");
            return string.Empty;
        }

        var content = ReadFileContent(Content);
        if (string.IsNullOrEmpty(content))
        {
            return string.Empty;
        }

        var template = ReadFileContent(Template);
        if (string.IsNullOrEmpty(template))
        {
            return string.Empty;
        }

        var parser = new LiquidParser();
        return !SetParserContent(parser, content) ? string.Empty : RenderTemplate(parser, template);
    }

    /// <summary>
    /// Reads the file content.
    /// </summary>
    /// <param name="filePath">The file path.</param>
    /// <param name="retry">The operation is being retried.</param>
    /// <returns>The file content.</returns>
    internal string ReadFileContent(string filePath, bool retry = false)
    {
        try
        {
            return File.ReadAllText(filePath);
        }
        catch
        {
            // Lock issue, wait and retry
            Thread.Sleep(TimeSpan.FromSeconds(1));
            if (!retry)
            {
                return ReadFileContent(filePath, true);
            }

            LogWarning($"Unable to read file: {filePath}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Sets the parser content.
    /// </summary>
    /// <param name="parser">The parser.</param>
    /// <param name="content">The content.</param>
    /// <returns>
    /// <c>true</c> if the content was set; otherwise, <c>false</c>.
    /// </returns>
    private bool SetParserContent(LiquidParser parser, string content)
    {
        try
        {
            if (Content.ToLowerInvariant().EndsWith(".json"))
            {
                parser.SetContentJson(content);
            }
            else if (Content.ToLowerInvariant().EndsWith(".xml"))
            {
                parser.SetContentXml(content);
            }
            else
            {
                WriteErrorLine("Unsupported content type");
                return false;
            }
        }
        catch (Exception e)
        {
            LogWarning("Unable to set parser content", e);
            return false;
        }

        return true;
    }

    /// <summary>
    /// Renders the template.
    /// </summary>
    /// <param name="parser">The parser.</param>
    /// <param name="template">The template.</param>
    /// <returns>The output from the template.</returns>
    private string RenderTemplate(LiquidParser parser, string template)
    {
        try
        {
            var output = parser.Parse(template).Render();
            File.WriteAllText(Output, output);
            return output;
        }
        catch (Exception e)
        {
            WriteErrorLine($"Error: {e.Message}");
            return string.Empty;
        }
    }

    /// <summary>
    /// Starts watching for changes.
    /// </summary>
    public void StartWatch()
    {
        if (!CanRender)
        {
            return;
        }

        if (_contentWatcher == null)
        {
            _contentWatcher = StartWatch(Content);
            _templateWatcher = StartWatch(Template);
        }

        _contentWatcher!.EnableRaisingEvents = true;
        _templateWatcher!.EnableRaisingEvents = true;
    }

    /// <summary>
    /// Starts watching for changes.
    /// </summary>
    /// <param name="path">The path to the specific file.</param>
    /// <returns>The watcher instance so it can be stopped later.</returns>
    private FileSystemWatcher StartWatch(string path)
    {
        var watcher = new FileSystemWatcher
        {
            IncludeSubdirectories = false,
            Path = Path.GetDirectoryName(path)!,
            Filter = Path.GetFileName(path)
        };

        watcher.Changed += OnChanged;
        return watcher;
    }

    /// <summary>
    /// Stops watching for changes.
    /// </summary>
    public void StopWatch()
    {
        if (_contentWatcher != null)
        {
            _contentWatcher.EnableRaisingEvents = false;
        }

        if (_templateWatcher != null)
        {
            _templateWatcher.EnableRaisingEvents = false;
        }

        LogMessage();
    }

    /// <summary>
    /// Writes the text message to log and console.
    /// </summary>
    /// <param name="text">The message.</param>
    private void LogMessage(string text = "")
    {
        Console.WriteLine(text);
        _writer.WriteLine(text);
    }

    /// <summary>
    /// Writes the warning message to log and console.
    /// </summary>
    /// <param name="text">The message.</param>
    /// <param name="e">The exception.</param>
    [ExcludeFromCodeCoverage]
    private void LogWarning(string text = "", Exception? e = null)
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        LogMessage(text);
        LogMessage($"    Info: {e?.Message}");
        Console.ForegroundColor = ConsoleColor.White;
    }

    /// <summary>
    /// Event is called when a source file has changed and calls to process output using that input.
    /// </summary>
    /// <param name="source">The source.</param>
    /// <param name="e">The <see cref="FileSystemEventArgs" /> instance containing the event data.</param>
    [ExcludeFromCodeCoverage]
    private void OnChanged(object source, FileSystemEventArgs e)
    {
        Render();
    }
}