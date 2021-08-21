// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;

namespace MartinCostello.Logging.XUnit
{
    public static class XUnitLoggerExtensionsTests
    {
        [Fact]
        public static void AddXUnit_TestOutputHelper_For_ILoggerBuilder_Validates_Parameters()
        {
            // Arrange
            var builder = Mock.Of<ILoggingBuilder>();
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var accessor = Mock.Of<ITestOutputHelperAccessor>();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit());
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(outputHelper, ConfigureAction));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(accessor));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(accessor, ConfigureAction));
            Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit((null as ITestOutputHelperAccessor)!));
            Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit((null as ITestOutputHelperAccessor)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit((null as ITestOutputHelper)!));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit((null as ITestOutputHelper)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(outputHelper, (null as Action<XUnitLoggerOptions>)!));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(accessor, (null as Action<XUnitLoggerOptions>)!));
        }

        [Fact]
        public static void AddXUnit_MessageSink_For_ILoggerBuilder_Validates_Parameters()
        {
            // Arrange
            var builder = Mock.Of<ILoggingBuilder>();
            var messageSink = Mock.Of<IMessageSink>();
            var accessor = Mock.Of<IMessageSinkAccessor>();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(messageSink));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(messageSink, ConfigureAction));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(accessor));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder)!.AddXUnit(accessor, ConfigureAction));
            Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit((null as IMessageSinkAccessor)!));
            Assert.Throws<ArgumentNullException>("accessor", () => builder.AddXUnit((null as IMessageSinkAccessor)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("messageSink", () => builder.AddXUnit((null as IMessageSink)!));
            Assert.Throws<ArgumentNullException>("messageSink", () => builder.AddXUnit((null as IMessageSink)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(messageSink, (null as Action<XUnitLoggerOptions>)!));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(accessor, (null as Action<XUnitLoggerOptions>)!));
        }

        [Fact]
        public static void AddXUnit_TestOutputHelper_For_ILoggerFactory_Validates_Parameters()
        {
            // Arrange
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var logLevel = LogLevel.Information;
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper, options));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper, ConfigureAction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper, Filter));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(outputHelper, logLevel));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!, Filter));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!, logLevel));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit((null as ITestOutputHelper)!, options));
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
            var messageSink = Mock.Of<IMessageSink>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink, options));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink, ConfigureAction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink, Filter));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory)!.AddXUnit(messageSink, logLevel));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!, ConfigureAction));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!, Filter));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!, logLevel));
            Assert.Throws<ArgumentNullException>("messageSink", () => factory.AddXUnit((null as IMessageSink)!, options));
            Assert.Throws<ArgumentNullException>("options", () => factory.AddXUnit(messageSink, (null as XUnitLoggerOptions)!));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(messageSink, (null as Action<XUnitLoggerOptions>)!));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(messageSink, (null as Func<XUnitLoggerOptions>)!));
            Assert.Throws<ArgumentNullException>("filter", () => factory.AddXUnit(messageSink, (null as Func<string, LogLevel, bool>)!));
        }

        [Fact]
        public static void ToLoggerFactory_Validates_Parameters()
        {
            // Arrange
            ITestOutputHelper? outputHelper = null;
            IMessageSink? messageSink = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>("outputHelper", () => outputHelper!.ToLoggerFactory());
            Assert.Throws<ArgumentNullException>("messageSink", () => messageSink!.ToLoggerFactory());
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
            var accessor = Mock.Of<ITestOutputHelperAccessor>();

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
            var accessor = Mock.Of<IMessageSinkAccessor>();

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
            var testOutputHelper = Mock.Of<ITestOutputHelper>();

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
            var messageSink = Mock.Of<IMessageSink>();

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
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var messageSink = Mock.Of<IMessageSink>();
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
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var messageSink = Mock.Of<IMessageSink>();

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
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var messageSink = Mock.Of<IMessageSink>();
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
}
