// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    public class Example
    {
        public Example(ITestOutputHelper outputHelper)
        {
            OutputHelper = outputHelper;
        }

        private ITestOutputHelper OutputHelper { get; }

        [Fact]
        public void Calculator_Sums_Two_Different_Integers()
        {
            // Arrange
            var services = new ServiceCollection()
                .AddLogging((builder) => builder.AddXUnit(OutputHelper))
                .AddSingleton<Calculator>();

            IServiceProvider provider = services.BuildServiceProvider();

            var calculator = provider.GetRequiredService<Calculator>();

            // Act
            int actual = calculator.Sum(1, 2);

            // Assert
            actual.ShouldBe(3);
        }

        [Fact]
        public void Calculator_Sums_Two_Equal_Integers()
        {
            // Arrange
            var calculator = new Calculator(OutputHelper.ToLogger<Calculator>());

            // Act
            int actual = calculator.Sum(2, 2);

            // Assert
            actual.ShouldBe(4);
        }

        private sealed class Calculator
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
}
