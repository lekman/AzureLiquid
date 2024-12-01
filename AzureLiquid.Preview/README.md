# Azure Liquid Preview

Using the Live Preview console application, we can set a file watcher that automatically renders the output as any
changes occur to the source data or Liquid template.

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
|------------|---------------------------------------------------------------------------|
| --help     | Shows help description within the console                                 |
| --watch    | Switch parameter to enable file watcher and produce output on file update |
| --template | Relative path to the .liquid template source file                         |
| --content  | Relative path to the XML or JSON data source file                         |
| --output   | Relative path to the output result file                                   |

For example:

```bash
liquidpreview --watch --content ./albums.xml --template ./albums.liquid --output ./albums.json
```

I have simply arranged the three files in VSCode and get preview on the right hand side whenever the template or XML
source is changed.

I then use the terminal to start the watcher. The albums.json file is generated automatically and kept up to date
whenever the source data or liquid template is changed.

![VSCode preview](https://raw.githubusercontent.com/lekman/AzureLiquid/main/Documentation/live-preview-console-vscode.png)