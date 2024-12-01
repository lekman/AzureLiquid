# Azure Liquid Tests

This project contains a set of unit tests for the [Azure Liquid Parser](../AzureLiquid/README.md).

## Running the Tests

First, install the dotnet tool for generating code coverage reports.

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

The tests can be run using the `dotnet test` command.

```bash
rm -r -f ./coverage
dotnet test . --configuration Debug --verbosity normal --collect:"XPlat Code Coverage" --results-directory ./coverage
export COVERAGE_FILE=$(find ./coverage -name "coverage.cobertura.xml" -type f -print -quit)
reportgenerator -reports:$COVERAGE_FILE -targetdir:"coveragereport" -reporttypes:Html
open ./coveragereport/index.htm
```

Test coverage is published to the `./coverage` folder and its path is stored in the `$COVERAGE_FILE` environment
variable. The `open` command is used to view the coverage report in a local browser.
