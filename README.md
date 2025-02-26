# xunit Logging

[![Build status][build-badge]][build-status]
[![codecov][coverage-badge]][coverage-report]
[![OpenSSF Scorecard][scorecard-badge]][scorecard-report]

| **xunit version** | **Package** | **NuGet Version** |
|:------------------|:------------|:------------------|
| xunit v2          | [MartinCostello.Logging.XUnit][package-download-v2] | [![NuGet][package-badge-version-v2]][package-download-v2] [![NuGet Downloads][package-badge-downloads-v2]][package-download-v2] |
| xunit v3          | [MartinCostello.Logging.XUnit.v3][package-download-v3] | [![NuGet][package-badge-version-v3]][package-download-v3] [![NuGet Downloads][package-badge-downloads-v3]][package-download-v3] |

## Introduction

`MartinCostello.Logging.XUnit` and `MartinCostello.Logging.XUnit.v3` provide extensions to hook into
the `ILogger` infrastructure to output logs from your xunit tests to the test output.

Projects using xunit v2 should use the `MartinCostello.Logging.XUnit` package, while projects using
xunit v3 should use the `MartinCostello.Logging.XUnit.v3` package.

> [!NOTE]
> This library is designed for the Microsoft logging implementation of `ILoggerFactory`.
> For other logging implementations, such as [Serilog][serilog], consider using packages such as [Serilog.Sinks.XUnit][serilog-sinks-xunit] instead.

### Installation

To install the library from NuGet using the .NET SDK run one of the following commands.

#### For xunit v2

```console
dotnet add package MartinCostello.Logging.XUnit
```

#### For xunit v3

```console
dotnet add package MartinCostello.Logging.XUnit.v3
```

### Usage

#### Dependency Injection
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions; // For xunit v2 - not required for xunit v3

namespace MyApp.Calculator;

public class CalculatorTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Calculator_Sums_Two_Integers()
    {
        // Arrange
        using var serviceProvider = new ServiceCollection()
            .AddLogging((builder) => builder.AddXUnit(outputHelper))
            .AddSingleton<Calculator>()
            .BuildServiceProvider();

        var calculator = services.GetRequiredService<Calculator>();

        // Act
        int actual = calculator.Sum(1, 2);

        // Assert
        Assert.AreEqual(3, actual);
    }
}

public sealed class Calculator(ILogger<Calculator> logger)
{
    public int Sum(int x, int y)
    {
        int sum = x + y;

        logger.LogInformation("The sum of {x} and {y} is {sum}.", x, y, sum);

        return sum;
    }
}
```

#### Standalone Logging Components
```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions; // For xunit v2 - not required for xunit v3

namespace MyApp.Calculator;

public class CalculatorTests(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Calculator_Sums_Two_Integers()
    {
        // Arrange
        var loggerFactory = LoggerFactory.Create(builder => builder
            .AddProvider(new XUnitLoggerProvider(outputHelper, xunitLoggerOptions))
            .SetMinimumLevel(LogLevel.Trace));

        var logger = loggerFactory.CreateLogger<Calculator>();

        var calculator = new Calculator(logger);

        // Act
        int actual = calculator.Sum(1, 2);

        // Assert
        Assert.AreEqual(3, actual);
    }
}

public sealed class Calculator(ILogger<Calculator> logger)
{
    public int Sum(int x, int y)
    {
        int sum = x + y;

        logger.LogInformation("The sum of {x} and {y} is {sum}.", x, y, sum);

        return sum;
    }
}
```


See below for links to more examples:

- [Unit tests][example-unit-tests]
- [Integration tests for an ASP.NET Core HTTP application][example-integration-tests]

## Migrating to xunit v3

[Xunit v3][xunit-v3-whats-new] contains many major architectural changes which means the same package
that supports logging for xunit v2 cannot be used with xunit v3. The equivalent NuGet package to support
logging for xunit v3 is the new [MartinCostello.Logging.XUnit.v3][package-download-v3] package.

To migrate usage of `MartinCostello.Logging.XUnit` to `MartinCostello.Logging.XUnit.v3` for xunit v3:

1. Follow the relevant steps to migrate any test projects from [xunit v2 to v3][xunit-v3-migration].
    - The most relevant change in xunit v3 is that the `ITestOutputHelper` type has moved from the `Xunit.Abstractions` namespace to `Xunit`.
1. Change any package references from `MartinCostello.Logging.XUnit` to `MartinCostello.Logging.XUnit.v3`.

    ```diff
    - <PackageReference Include="MartinCostello.Logging.XUnit" Version="0.5.0" />
    + <PackageReference Include="MartinCostello.Logging.XUnit.v3" Version="0.5.0" />
    ```

## Feedback

Any feedback or issues can be added to the issues for this project in [GitHub][issues].

## Repository

The repository is hosted in [GitHub][repo]: <https://github.com/martincostello/xunit-logging.git>

## License

This project is licensed under the [Apache 2.0][license] license.

## Building and Testing

Compiling the solution yourself requires Git and the [.NET SDK][dotnet-sdk] to be installed (version `9.0.100` or later).

To build and test the solution locally from a terminal/command-line, run the following set of commands:

```powershell
git clone https://github.com/martincostello/xunit-logging.git
cd xunit-logging
./build.ps1
```

[build-badge]: https://github.com/martincostello/xunit-logging/actions/workflows/build.yml/badge.svg?branch=main&event=push
[build-status]: https://github.com/martincostello/xunit-logging/actions?query=workflow%3Abuild+branch%3Amain+event%3Apush "Continuous Integration for this project"
[coverage-badge]: https://codecov.io/gh/martincostello/xunit-logging/branch/main/graph/badge.svg
[coverage-report]: https://codecov.io/gh/martincostello/xunit-logging "Code coverage report for this project"
[scorecard-badge]: https://api.securityscorecards.dev/projects/github.com/martincostello/xunit-logging/badge
[scorecard-report]: https://securityscorecards.dev/viewer/?uri=github.com/martincostello/xunit-logging "OpenSSF Scorecard for this project"
[dotnet-sdk]: https://dot.net/download "Download the .NET SDK"
[example-integration-tests]: https://github.com/martincostello/xunit-logging/blob/main/tests/Shared/Integration/HttpApplicationTests.cs "Integration test examples"
[example-unit-tests]: https://github.com/martincostello/xunit-logging/blob/main/tests/Shared/Examples.cs "Unit test examples"
[issues]: https://github.com/martincostello/xunit-logging/issues "Issues for this project on GitHub.com"
[license]: https://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license"
[package-badge-downloads-v2]: https://img.shields.io/nuget/dt/MartinCostello.Logging.XUnit?logo=nuget&label=Downloads&color=blue
[package-badge-downloads-v3]: https://img.shields.io/nuget/dt/MartinCostello.Logging.XUnit.v3?logo=nuget&label=Downloads&color=blue
[package-badge-version-v2]: https://img.shields.io/nuget/v/MartinCostello.Logging.XUnit?logo=nuget&label=Latest&color=blue
[package-badge-version-v3]: https://img.shields.io/nuget/v/MartinCostello.Logging.XUnit.v3?logo=nuget&label=Latest&color=blue
[package-download-v2]: https://www.nuget.org/packages/MartinCostello.Logging.XUnit "Download MartinCostello.Logging.XUnit from NuGet"
[package-download-v3]: https://www.nuget.org/packages/MartinCostello.Logging.XUnit.v3 "Download MartinCostello.Logging.XUnit.v3 from NuGet"
[repo]: https://github.com/martincostello/xunit-logging "This project on GitHub.com"
[serilog]: https://serilog.net/ "Serilog website"
[serilog-sinks-xunit]: https://github.com/trbenning/serilog-sinks-xunit "Serilog.Sinks.XUnit on GitHub"
[xunit-v3-migration]: https://xunit.net/docs/getting-started/v3/migration "Migrating from xunit v2 to v3"
[xunit-v3-whats-new]: https://xunit.net/docs/getting-started/v3/whats-new "What's New in v3"
