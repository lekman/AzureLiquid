// <copyright file="Program.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Preview
// Created: 2022-10-18 07:46
// </copyright>

using AzureLiquid.Preview;

var preview = new PreviewProcess();

// deepcode ignore XmlInjection: XML is not used by this application, it is passed back to the user, deepcode ignore XXE: <please specify a reason of ignoring this>
preview.ParseArguments(args);

if (args?.Length == 0)
{
    PreviewProcess.WriteHelpOutput();
}

if (preview.CanRender)
{
    preview.Render();

    if (preview.ShouldWatch)
    {
        preview.StartWatch();
        Console.WriteLine("Press any key to exit file watch...");
        _ = Console.ReadKey();
        preview.StopWatch();
        Console.WriteLine("");
    }
}
else
{
    if (!string.IsNullOrEmpty(preview.Content) && !string.IsNullOrEmpty(preview.Template))
    {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  Unable to render as input files are not found");
        Console.WriteLine("");
    }
}