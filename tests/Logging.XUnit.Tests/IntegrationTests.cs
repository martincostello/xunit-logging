// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace MartinCostello.Logging.XUnit
{
    public static class IntegrationTests
    {
        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelper()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();
            var logger = BootstrapBuilder((builder) => builder.AddXUnit(mock.Object));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogInformation("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Exactly(2));
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelper_With_Configuration()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            var logger = BootstrapBuilder(
                (builder) =>
                {
                    builder.AddXUnit(
                        mock.Object,
                        (options) => options.Filter = (_, level) => level >= LogLevel.Error);
                });

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogTrace("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilderAccessor_TestOutputHelper()
        {
            // Arrange
            var mockOutputHelper = new Mock<ITestOutputHelper>();
            var outputHelper = mockOutputHelper.Object;

            var mockAccessor = new Mock<ITestOutputHelperAccessor>();

            mockAccessor
                .Setup((p) => p.OutputHelper)
                .Returns(outputHelper);

            var accessor = mockAccessor.Object;

            var logger = BootstrapBuilder((builder) => builder.AddXUnit(accessor));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogInformation("If you know the clues, it's easy to get through.");

            // Assert
            mockOutputHelper.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Exactly(2));
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilder_TestOutputHelperAccessor_With_Configuration()
        {
            // Arrange
            var mockOutputHelper = new Mock<ITestOutputHelper>();
            var outputHelper = mockOutputHelper.Object;

            var mockAccessor = new Mock<ITestOutputHelperAccessor>();

            mockAccessor
                .Setup((p) => p.OutputHelper)
                .Returns(outputHelper);

            var accessor = mockAccessor.Object;

            var logger = BootstrapBuilder(
                (builder) =>
                {
                    builder.AddXUnit(
                        mockOutputHelper.Object,
                        (options) => options.Filter = (_, level) => level >= LogLevel.Error);
                });

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogTrace("If you know the clues, it's easy to get through.");

            // Assert
            mockOutputHelper.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();
            var logger = BootstrapFactory((builder) => builder.AddXUnit(mock.Object));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogInformation("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Exactly(2));
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory_With_Filter()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();
            var logger = BootstrapFactory((builder) => builder.AddXUnit(mock.Object, (_, level) => level >= LogLevel.Error));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogWarning("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory_With_Minimum_Level()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();
            var logger = BootstrapFactory((builder) => builder.AddXUnit(mock.Object, LogLevel.Information));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogTrace("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory_With_Options()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            var options = new XUnitLoggerOptions()
            {
                Filter = (_, level) => level >= LogLevel.Error,
            };

            var logger = BootstrapFactory((builder) => builder.AddXUnit(mock.Object, options));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogWarning("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory_With_Options_Factory()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            var options = new XUnitLoggerOptions()
            {
                Filter = (_, level) => level >= LogLevel.Error,
            };

            var logger = BootstrapFactory((builder) => builder.AddXUnit(mock.Object, () => options));

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogWarning("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerFactory_With_Configure_Options()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();
            var logger = BootstrapFactory(
                (builder) =>
                {
                    builder.AddXUnit(
                        mock.Object,
                        (options) => options.Filter = (_, level) => level >= LogLevel.Error);
                });

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogWarning("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Once());
        }

        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilder()
        {
            // Arrange
            var serviceProvider = new ServiceCollection()
                .AddLogging((builder) => builder.AddXUnit())
                .BuildServiceProvider();

            var mock = new Mock<ITestOutputHelper>();

            serviceProvider.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = mock.Object;

            var logger = serviceProvider.GetRequiredService<ILogger<XUnitLogger>>();

            // Act
            logger.LogError("This is a brand new problem, a problem without any clues.");
            logger.LogInformation("If you know the clues, it's easy to get through.");

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsNotNull<string>()), Times.Exactly(2));
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
}
