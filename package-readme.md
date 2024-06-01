# xunit Logging

## Introduction

`MartinCostello.Logging.XUnit` provides extensions to hook into the `ILogger` infrastructure to output logs from your xunit tests to the test output.

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

## Feedback

Any feedback or issues can be added to the issues for this project in [GitHub](https://github.com/martincostello/xunit-logging/issues "Issues for this project on GitHub.com").

## License

This project is licensed under the [Apache 2.0](http://www.apache.org/licenses/LICENSE-2.0.txt "The Apache 2.0 license") license.
