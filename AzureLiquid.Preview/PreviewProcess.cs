// <copyright file="PreviewProcess.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Preview
// Created: 2022-10-18 07:46
// </copyright>

using System.Diagnostics.CodeAnalysis;

namespace AzureLiquid.Preview
{
    /// <summary>
    /// Starts a preview of Liquid template rendering results, and optionally continuously render when source files changes.
    /// </summary>
    public class PreviewProcess
    {
        private FileSystemWatcher? _contentWatcher;

        private FileSystemWatcher? _templateWatcher;

        private readonly StringWriter _writer = new();

        /// <summary>
        /// Initializes a new instance of the <see cref="PreviewProcess"/> class.
        /// </summary>
        public PreviewProcess()
        {
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
        ///   <c>true</c> if should watch; otherwise, <c>false</c>.
        /// </value>
        internal bool ShouldWatch { get; private set; }

        /// <summary>
        /// Determines whether this instance can parse the inputs and render the output.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if this instance can parse; otherwise, <c>false</c>.
        /// </returns>
        public bool CanRender => File.Exists(Template) && File.Exists(Content) && !string.IsNullOrEmpty(Output);

        /// <summary>
        /// Start a new instance of the <see cref="PreviewProcess"/> class using the incoming arguments.
        /// </summary>
        /// <param name="args">The process arguments</param>
        /// <returns>A new instance of the <see cref="PreviewProcess"/> class.</returns>
        [ExcludeFromCodeCoverage]
        public static PreviewProcess Create(string[] args)
        {
            var preview = new PreviewProcess();

            // deepcode ignore XmlInjection: XML is not used by this application, it is passed back to the user, deepcode ignore XXE: <please specify a reason of ignoring this>
            preview.ParseArguments(args);

            if (args?.Length == 0)
            {
                preview.WriteHelpOutput();
            }

            if (preview.CanRender)
            {
                preview.Render();

                if (preview.ShouldWatch)
                {
                    preview.StartWatch();
                    preview.LogMessage("Press any key to exit file watch...");
                    _ = Console.ReadKey();
                    preview.StopWatch();
                    preview.LogMessage("");
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(preview.Content) && !string.IsNullOrEmpty(preview.Template))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    preview.LogMessage("  Unable to render as input files are not found");
                    preview.LogMessage("");
                }
            }

            return preview;
        }

        /// <summary>Parses the arguments and sets process options.</summary>
        /// <param name="args">The arguments. Values are expected to be "--template", "--help", "--content", "--output" or "--watch".</param>
        internal void ParseArguments(string[] args)
        {
            for (var index = 0; index < args.Length; index++)
            {
                var arg = args[index];
                var path = Directory.GetCurrentDirectory();

                // Parse incoming template file
                if (IsArgMatch(arg, "template") && index - 1 < args.Length)
                {
                    try
                    {
                        Template = Path.GetFullPath(args[index + 1], path);
                    }
                    catch
                    {
                        WriteErrorLine($"Invalid template path: {args[index + 1]}");
                    }
                }

                // Parse incoming content file
                if (IsArgMatch(arg, "content") && index - 1 < args.Length)
                {
                    try
                    {
                        Content = Path.GetFullPath(args[index + 1], path);
                    }
                    catch
                    {
                        WriteErrorLine($"Invalid content path: {args[index + 1]}");
                    }
                }

                // Parse outgoing results file
                if (IsArgMatch(arg, "output") && index - 1 < args.Length)
                {
                    try
                    {
                        Output = Path.GetFullPath(args[index + 1], path);
                    }
                    catch
                    {
                        WriteErrorLine($"Invalid output path: {args[index + 1]}");
                    }
                }

                if (IsArgMatch(arg, "watch"))
                {
                    ShouldWatch = true;
                }

                // Show help info
                if (IsArgMatch(arg, "help"))
                {
                    WriteHelpOutput();
                }
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
        private static void WriteErrorLine(string error)
        {
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine($"  {error}");
            Console.ForegroundColor = ConsoleColor.White;
        }

        /// <summary>
        /// Determines whether the argument matches the partial argument key name.
        /// </summary>
        /// <param name="arg">The argument.</param>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if argument found; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsArgMatch(string arg, string key) =>
            string.CompareOrdinal(arg, "--" + key) == 0;

        /// <summary>
        /// Renders the output using the specified properties of the instance.
        /// </summary>
        /// <returns>The output from the template data and content.</returns>
        public string Render()
        {
            if (!CanRender)
            {
                WriteErrorLine("Unable to render as inputs our outputs not found or not specified");
                return string.Empty;
            }

            string content;
            try
            {
                content = File.ReadAllText(Content);
            }
            catch (IOException)
            {
                // Lock issue, wait and retry
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return Render();
            }

            string template;
            try
            {
                template = File.ReadAllText(Template);
            }
            catch (IOException)
            {
                // Lock issue, wait and retry
                Thread.Sleep(TimeSpan.FromSeconds(1));
                return Render();
            }

            var parser = new LiquidParser();

            if (Content.ToLowerInvariant().EndsWith(".json"))
            {
                try
                {
                    parser.SetContentJson(content);
                }
                catch (Exception e)
                {
                    LogWarning("  Unable to read input JSON file", e);
                    return string.Empty;
                }
            }

            if (Content.ToLowerInvariant().EndsWith(".xml"))
            {
                try
                {
                    parser.SetContentXml(content);
                }
                catch (Exception ex)
                {
                    LogWarning("  Unable to read input XML file", ex);
                    return string.Empty;
                }
            }

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
            if (CanRender)
            {
                if (_contentWatcher == null)
                {
                    _contentWatcher = StartWatch(Content);
                    _templateWatcher = StartWatch(Template);
                }

                _contentWatcher!.EnableRaisingEvents = true;
                _templateWatcher!.EnableRaisingEvents = true;
            }
        }

        /// <summary>
        /// Gets the console output from the last render.
        /// </summary>
        /// <returns>The console output.</returns>
        public string Log => _writer.ToString();

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

            LogMessage("");
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
        /// Called when a source file has changed and calls to process output using that input.
        /// </summary>
        /// <param name="source">The source.</param>
        /// <param name="e">The <see cref="FileSystemEventArgs"/> instance containing the event data.</param>
        private void OnChanged(object source, FileSystemEventArgs e) => Render();
    }
}