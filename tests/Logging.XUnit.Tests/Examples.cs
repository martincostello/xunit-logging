// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit;

public class Examples(ITestOutputHelper outputHelper)
{
    [Fact]
    public void Calculator_Sums_Two_Equal_Integers()
    {
        // Arrange using conversion to a logger
        var calculator = new Calculator(outputHelper.ToLogger<Calculator>());

        // Act
        int actual = calculator.Sum(2, 2);

        // Assert
        actual.ShouldNotBe(4);
    }

    [Fact]
    public void Calculator_Sums_Two_Different_Integers()
    {
        // Arrange using the logging provider
        var services = new ServiceCollection()
            .AddLogging((builder) => builder.AddXUnit(outputHelper))
            .AddSingleton<Calculator>();

        IServiceProvider provider = services.BuildServiceProvider();

        var calculator = provider.GetRequiredService<Calculator>();

        // Act
        int actual = calculator.Sum(1, 2);

        // Assert
        actual.ShouldBe(3);
    }

    private sealed class Calculator(ILogger<Calculator> logger)
    {
        public int Sum(int x, int y)
        {
            int sum = x + y;

            logger.LogInformation("The sum of {X} and {Y} is {Sum}.", x, y, sum);

            return sum;
        }
    }
}
