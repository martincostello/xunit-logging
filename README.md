# xunit Logging

[![NuGet](https://buildstats.info/nuget/MartinCostello.Logging.XUnit?includePreReleases=true)](http://www.nuget.org/packages/MartinCostello.Logging.XUnit "Download MartinCostello.Logging.XUnit from NuGet")

| | Windows | Linux/OS X | Linux/macOS/Windows |
|:-:|:-:|:-:|:-:|
| **Build Status** | [![Windows build status](https://img.shields.io/appveyor/ci/martincostello/xunit-logging/master.svg)](https://ci.appveyor.com/project/martincostello/xunit-logging) [![Code coverage](https://codecov.io/gh/martincostello/xunit-logging/branch/master/graph/badge.svg)](https://codecov.io/gh/martincostello/xunit-logging) | [![Linux build status](https://img.shields.io/travis-ci/com/martincostello/xunit-logging/master.svg)](https://travis-ci.com/martincostello/xunit-logging) | [![Azure Pipelines build status](https://dev.azure.com/martincostello/xunit-logging/_apis/build/status/CI)](https://dev.azure.com/martincostello/xunit-logging/_build/latest?definitionId=67) |
| **Build History** | [![Windows build history](https://buildstats.info/appveyor/chart/martincostello/xunit-logging?branch=master&includeBuildsFromPullRequest=false)](https://ci.appveyor.com/project/martincostello/xunit-logging) | [![Linux build history](https://buildstats.info/travisci/chart/martincostello/xunit-logging?branch=master&includeBuildsFromPullRequest=false)](https://travis-ci.com/martincostello/xunit-logging) | _Not supported_ |

## Introduction

`MartinCostello.Logging.XUnit` provides extensions to hook into the `ILogger` infrastructure to output logs from your xunit tests to the test output.

### Installation

To install the library from [NuGet](https://www.nuget.org/packages/MartinCostello.Logging.XUnit/ "MartinCostello.Logging.XUnit on NuGet.org") using the .NET SDK run:

```
dotnet add package MartinCostello.Logging.XUnit
```

### Usage

```csharp
using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Xunit;
using Xunit.Abstractions;

namespace MyApp.Calculator
{
    public class CalculatorTests
    {
        public CalculatorTests(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        private ITestOutputHelper OutputHelper { get; }

        [Fact]
        public void Calculator_Sums_Two_Integers()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddLogging((builder) => builder.AddXUnit(OutputHelper))
                .AddSingleton<Calculator>();

            var calculator = services
                .BuildServiceProvider()
                .GetRequiredService<Calculator>();

            // Act
            int actual = calculator.Sum(1, 2);

            // Assert
            Assert.AreEqual(3, actual);
        }
    }

    public sealed class Calculator
    {
        private readonly ILogger _logger;

        public Calculator(ILogger<Calculator> logger)
        {
            _logger = logger;
        }

        public int Sum(int x, int y)
        {
            int sum = x + y;

            _logger.LogInformation("The sum of {x} and {y} is {sum}.", x, y, sum);

            return sum;
        }
    }
}
```

See below for links to more examples:
  1. [Unit tests](https://github.com/martincostello/xunit-logging/blob/master/tests/Logging.XUnit.Tests/Examples.cs "Unit test examples")
  1. [Integration tests for an ASP.NET Core HTTP application](https://github.com/martincostello/xunit-logging/blob/master/tests/Logging.XUnit.Tests/Integration/HttpApplicationTests.cs "Integration test examples")

## Feedback

Any feedback or issues can be added to the issues for this project in [GitHub](https://github.com/martincostello/xunit-logging/issues "Issues for this project on GitHub.com").

## Repository

The repository is hosted in [GitHub](https://github.com/martincostello/xunit-logging "This project on GitHub.com"): https://github.com/martincostello/xunit-logging.git

## License

This project is licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license") license.

## Building and Testing

Compiling the library yourself requires Git and the [.NET Core SDK](https://www.microsoft.com/net/download/core "Download the .NET Core SDK") to be installed (version `2.1.402` or later).

To build and test the library locally from a terminal/command-line, run one of the following set of commands:

**Windows**

```powershell
git clone https://github.com/martincostello/xunit-logging.git
cd xunit-logging
.\Build.ps1
```

**Linux/macOS**

```sh
git clone https://github.com/martincostello/xunit-logging.git
cd xunit-logging
./build.sh
```
