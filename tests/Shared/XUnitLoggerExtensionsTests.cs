// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using NSubstitute;

namespace MartinCostello.Logging.XUnit;

public static class XUnitLoggerExtensionsTests
{
    [Fact]
    public static void AddXUnit_TestOutputHelper_For_ILoggerBuilder_Validates_Parameters()
    {
        // Arrange
        var builder = Substitute.For<ILoggingBuilder>();
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var accessor = Substitute.For<ITestOutputHelperAccessor>();

        ILoggingBuilder nullBuilder = null!;
        ITestOutputHelperAccessor nullAccessor = null!;
        ITestOutputHelper nullHelper = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>("builder", nullBuilder.AddXUnit);
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(outputHelper));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(outputHelper, ConfigureAction));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(accessor));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(accessor, ConfigureAction));
        Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit(nullAccessor));
        Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit(nullAccessor, ConfigureAction));
        Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(nullHelper));
        Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(nullHelper, ConfigureAction));
        Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(outputHelper, null!));
        Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(accessor, null!));
    }

    [Fact]
    public static void AddXUnit_MessageSink_For_ILoggerBuilder_Validates_Parameters()
    {
        // Arrange
        var builder = Substitute.For<ILoggingBuilder>();
        var messageSink = Substitute.For<IMessageSink>();
        var accessor = Substitute.For<IMessageSinkAccessor>();

        ILoggingBuilder nullBuilder = null!;
        IMessageSinkAccessor nullAccessor = null!;
        IMessageSink nullSink = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(messageSink));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(messageSink, ConfigureAction));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(accessor));
        Assert.Throws<ArgumentNullException>("builder", () => nullBuilder.AddXUnit(accessor, ConfigureAction));
        Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit(nullAccessor));
        Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit(nullAccessor, ConfigureAction));
        Assert.Throws<ArgumentNullException>("messageSink", () => builder.AddXUnit(nullSink));
        Assert.Throws<ArgumentNullException>("messageSink", () => builder.AddXUnit(nullSink, ConfigureAction));
        Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(messageSink, null!));
        Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(accessor, null!));
    }

    [Fact]
    public static void AddXUnit_TestOutputHelper_For_ILoggerFactory_Validates_Parameters()
    {
        // Arrange
        ILoggerFactory factory = NullLoggerFactory.Instance;
        var logLevel = LogLevel.Information;
        var outputHelper = Substitute.For<ITestOutputHelper>();
        var options = new XUnitLoggerOptions();

        ILoggerFactory nullFactory = null!;
        ITestOutputHelper nullHelper = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper, options));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper, ConfigureAction));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper, ConfigureFunction));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper, Filter));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(outputHelper, logLevel));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper, ConfigureAction));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper, ConfigureFunction));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper, Filter));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper, logLevel));
        Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(nullHelper, options));
        Assert.Throws<ArgumentNullException>("options", () => factory.AddXUnit(outputHelper, (null as XUnitLoggerOptions)!));
        Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, (null as Action<XUnitLoggerOptions>)!));
        Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, (null as Func<XUnitLoggerOptions>)!));
        Assert.Throws<ArgumentNullException>("filter", () => factory.AddXUnit(outputHelper, (null as Func<string, LogLevel, bool>)!));
    }

    [Fact]
    public static void AddXUnit_MessageSink_For_ILoggerFactory_Validates_Parameters()
    {
        // Arrange
        ILoggerFactory factory = NullLoggerFactory.Instance;
        var logLevel = LogLevel.Information;
        var messageSink = Substitute.For<IMessageSink>();
        var options = new XUnitLoggerOptions();

        ILoggerFactory nullFactory = null!;
        IMessageSink nullSink = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink, options));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink, ConfigureAction));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink, ConfigureFunction));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink, Filter));
        Assert.Throws<ArgumentNullException>("factory", () => nullFactory.AddXUnit(messageSink, logLevel));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink, ConfigureAction));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink, ConfigureFunction));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink, Filter));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink, logLevel));
        Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit(nullSink, options));
        Assert.Throws<ArgumentNullException>("options", () => factory.AddXUnit(messageSink, (null as XUnitLoggerOptions)!));
        Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(messageSink, (null as Action<XUnitLoggerOptions>)!));
        Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(messageSink, (null as Func<XUnitLoggerOptions>)!));
        Assert.Throws<ArgumentNullException>("filter", () => factory.AddXUnit(messageSink, (null as Func<string, LogLevel, bool>)!));
    }

    [Fact]
    public static void ToLoggerFactory_Validates_Parameters()
    {
        // Arrange
        ITestOutputHelper outputHelper = null!;
        IMessageSink messageSink = null!;

        // Act and Assert
        Assert.Throws<ArgumentNullException>("outputHelper", outputHelper.ToLoggerFactory);
        Assert.Throws<ArgumentNullException>("messageSink", messageSink.ToLoggerFactory);
    }

    [Fact]
    public static void AddXUnit_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();

        // Act
        services.AddLogging(c => c.AddXUnit());

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ILoggerProvider>().ShouldBeOfType<XUnitLoggerProvider>();
        serviceProvider.GetService<ITestOutputHelperAccessor>().ShouldBeOfType<AmbientTestOutputHelperAccessor>();
    }

    [Fact]
    public static void AddXUnit_ITestOutputHelperAccessor_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        var accessor = Substitute.For<ITestOutputHelperAccessor>();

        // Act
        services.AddLogging(c => c.AddXUnit(accessor));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ILoggerProvider>().ShouldBeOfType<XUnitLoggerProvider>();
        serviceProvider.GetService<ITestOutputHelperAccessor>().ShouldBe(accessor);
    }

    [Fact]
    public static void AddXUnit_IMessageSinkAccessor_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        var accessor = Substitute.For<IMessageSinkAccessor>();

        // Act
        services.AddLogging(c => c.AddXUnit(accessor));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ILoggerProvider>().ShouldBeOfType<XUnitLoggerProvider>();
        serviceProvider.GetService<IMessageSinkAccessor>().ShouldBe(accessor);
    }

    [Fact]
    public static void AddXUnit_ITestOutputHelper_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        var testOutputHelper = Substitute.For<ITestOutputHelper>();

        // Act
        services.AddLogging(c => c.AddXUnit(testOutputHelper));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ILoggerProvider>().ShouldBeOfType<XUnitLoggerProvider>();
    }

    [Fact]
    public static void AddXUnit_IMessageSink_Registers_Services()
    {
        // Arrange
        var services = new ServiceCollection();
        var messageSink = Substitute.For<IMessageSink>();

        // Act
        services.AddLogging(c => c.AddXUnit(messageSink));

        // Assert
        var serviceProvider = services.BuildServiceProvider();
        serviceProvider.GetService<ILoggerProvider>().ShouldBeOfType<XUnitLoggerProvider>();
    }

    [Fact]
    public static void AddXUnit_IMessageSink_With_LogLevel_Works()
    {
        // Arrange
        var factory = NullLoggerFactory.Instance;
        var messageSink = Substitute.For<IMessageSink>();
        var minLevel = LogLevel.Debug;

        // Act
        factory.AddXUnit(messageSink, minLevel);

        // Assert
        ILogger logger = factory.CreateLogger("SomeLogger");
        logger.LogInformation("Some message");
    }

    [Fact]
    public static void AddXUnit_IMessageSink_With_Filter_Works()
    {
        // Arrange
        var factory = NullLoggerFactory.Instance;
        var messageSink = Substitute.For<IMessageSink>();

        // Act
        factory.AddXUnit(messageSink, (_) => { });

        // Assert
        ILogger logger = factory.CreateLogger("SomeLogger");
        logger.LogInformation("Some message");
    }

    [Fact]
    public static void AddXUnit_IMessageSink_With_Options_Works()
    {
        // Arrange
        var factory = NullLoggerFactory.Instance;
        var messageSink = Substitute.For<IMessageSink>();
        var options = new XUnitLoggerOptions();

        // Act
        factory.AddXUnit(messageSink, options);

        // Assert
        ILogger logger = factory.CreateLogger("SomeLogger");
        logger.LogInformation("Some message");
    }

    private static void ConfigureAction(XUnitLoggerOptions options)
    {
    }

    private static XUnitLoggerOptions ConfigureFunction() => new();

    private static bool Filter(string? categoryName, LogLevel level) => true;
}
