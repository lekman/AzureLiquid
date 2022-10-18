// <copyright file="Program.cs">
// Licensed under the open source Apache License, Version 2.0.
// Project: AzureLiquid.Preview
// Created: 2022-10-18 07:46
// </copyright>

using AzureLiquid.Preview;

var preview = new PreviewProcess();
preview.ParseArguments(args);

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
    Console.ForegroundColor = ConsoleColor.Yellow;
    Console.WriteLine("  Unable to render as input files are not found or not specified");
    Console.WriteLine("");
}