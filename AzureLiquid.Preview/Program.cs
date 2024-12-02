// <copyright file="Program.cs">
// Licensed under the open source Apache License, Version 2.0.
// </copyright>

using System.Runtime.CompilerServices;
using AzureLiquid.Preview;

[assembly: InternalsVisibleTo("AzureLiquid.Tests")]

PreviewProcess.Create(args);