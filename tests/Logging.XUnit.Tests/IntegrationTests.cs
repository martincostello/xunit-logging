// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace MartinCostello.Logging.XUnit;

public static class IntegrationTests
{
    [Fact]
    public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelper()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var logger = BootstrapBuilder((builder) => builder.AddXUnit(outputHelper));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogInformation("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(2).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelper_With_Configuration()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();

        var logger = BootstrapBuilder(
            (builder) =>
            {
                builder.AddXUnit(
                    outputHelper,
                    (options) => options.Filter = (_, level) => level >= LogLevel.Error);
            });

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogTrace("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerBuilderAccessor_TestOutputHelper()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();

        var accessor = Substitute.For<ITestOutputHelperAccessor>();
        accessor.OutputHelper.Returns(outputHelper);

        var logger = BootstrapBuilder((builder) => builder.AddXUnit(accessor));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogInformation("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(2).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelperAccessor_With_Configuration()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();

        var accessor = Substitute.For<ITestOutputHelperAccessor>();
        accessor.OutputHelper.Returns(outputHelper);

        var logger = BootstrapBuilder(
            (builder) =>
            {
                builder.AddXUnit(
                    outputHelper,
                    (options) => options.Filter = (_, level) => level >= LogLevel.Error);
            });

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogTrace("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var logger = BootstrapFactory((builder) => builder.AddXUnit(outputHelper));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogInformation("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(2).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory_With_Filter()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var logger = BootstrapFactory((builder) => builder.AddXUnit(outputHelper, (_, level) => level >= LogLevel.Error));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogWarning("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory_With_Minimum_Level()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var logger = BootstrapFactory((builder) => builder.AddXUnit(outputHelper, LogLevel.Information));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogTrace("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory_With_Options()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();

        var options = new XUnitLoggerOptions()
        {
            Filter = (_, level) => level >= LogLevel.Error,
        };

        var logger = BootstrapFactory((builder) => builder.AddXUnit(outputHelper, options));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogWarning("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory_With_Options_Factory()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();

        var options = new XUnitLoggerOptions()
        {
            Filter = (_, level) => level >= LogLevel.Error,
            Formatter = new DefaultXUnitLogFormatter(new()
            {
                TimestampFormat = "yyyy-MM-dd HH:mm:ss.fff",
            }),
        };

        var logger = BootstrapFactory((builder) => builder.AddXUnit(outputHelper, () => options));

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogWarning("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerFactory_With_Configure_Options()
    {
        // Arrange
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var logger = BootstrapFactory(
            (builder) =>
            {
                builder.AddXUnit(
                    outputHelper,
                    (options) => options.Filter = (_, level) => level >= LogLevel.Error);
            });

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogWarning("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(1).WriteLine(Arg.Is<string>((p) => p != null));
    }

    [Fact]
    public static void Can_Configure_xunit_For_ILoggerBuilder()
    {
        // Arrange
        var serviceProvider = new ServiceCollection()
            .AddLogging((builder) => builder.AddXUnit())
            .BuildServiceProvider();

        var outputHelper = Substitute.For<ITestOutputHelper>();

        serviceProvider.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = outputHelper;

        var logger = serviceProvider.GetRequiredService<ILogger<XUnitLogger>>();

        // Act
        logger.LogError("This is a brand new problem, a problem without any clues.");
        logger.LogInformation("If you know the clues, it's easy to get through.");

        // Assert
        outputHelper.Received(2).WriteLine(Arg.Is<string>((p) => p != null));
    }

    private static ILogger BootstrapBuilder(Action<ILoggingBuilder> configure)
    {
        return new ServiceCollection()
            .AddLogging(configure)
            .BuildServiceProvider()
            .GetRequiredService<ILogger<XUnitLogger>>();
    }

    private static ILogger BootstrapFactory(Action<ILoggerFactory> configure)
    {
        var services = new ServiceCollection()
            .AddLogging()
            .BuildServiceProvider();

        var factory = services.GetRequiredService<ILoggerFactory>();

        configure(factory);

        return factory.CreateLogger<XUnitLogger>();
    }
}
