# xunit Logging

[![NuGet](https://buildstats.info/nuget/MartinCostello.Logging.XUnit?includePreReleases=true)](https://www.nuget.org/packages/MartinCostello.Logging.XUnit "Download MartinCostello.Logging.XUnit from NuGet")

[![Build status](https://github.com/martincostello/xunit-logging/workflows/build/badge.svg?branch=main&event=push)](https://github.com/martincostello/xunit-logging/actions?query=workflow%3Abuild+branch%3Amain+event%3Apush)
[![codecov](https://codecov.io/gh/martincostello/xunit-logging/branch/main/graph/badge.svg)](https://codecov.io/gh/martincostello/xunit-logging)
[![OpenSSF Scorecard](https://api.securityscorecards.dev/projects/github.com/martincostello/xunit-logging/badge)](https://securityscorecards.dev/viewer/?uri=github.com/martincostello/xunit-logging)

## Introduction

`MartinCostello.Logging.XUnit` provides extensions to hook into the `ILogger` infrastructure to output logs from your xunit tests to the test output.

> ℹ️ This library is designed for the Microsoft logging implementation of `ILoggerFactory`. For other logging implementations, such as [Serilog](https://serilog.net/), consider using packages such as [Serilog.Sinks.XUnit](https://github.com/trbenning/serilog-sinks-xunit) instead.

### Installation

To install the library from [NuGet](https://www.nuget.org/packages/MartinCostello.Logging.XUnit/ "MartinCostello.Logging.XUnit on NuGet.org") using the .NET SDK run:

```console
dotnet add package MartinCostello.Logging.XUnit
```

### Usage

```csharp
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

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

See below for links to more examples:

- [Unit tests](https://github.com/martincostello/xunit-logging/blob/main/tests/Logging.XUnit.Tests/Examples.cs "Unit test examples")
- [Integration tests for an ASP.NET Core HTTP application](https://github.com/martincostello/xunit-logging/blob/main/tests/Logging.XUnit.Tests/Integration/HttpApplicationTests.cs "Integration test examples")

## Feedback

Any feedback or issues can be added to the issues for this project in [GitHub](https://github.com/martincostello/xunit-logging/issues "Issues for this project on GitHub.com").

## Repository

The repository is hosted in [GitHub](https://github.com/martincostello/xunit-logging "This project on GitHub.com"): <https://github.com/martincostello/xunit-logging.git>

## License

This project is licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license") license.

## Building and Testing

Compiling the library yourself requires Git and the [.NET SDK](https://www.microsoft.com/net/download/core "Download the .NET SDK") to be installed (version `8.0.100` or later).

To build and test the library locally from a terminal/command-line, run one of the following set of commands:

```powershell
git clone https://github.com/martincostello/xunit-logging.git
cd xunit-logging
./build.ps1
```
