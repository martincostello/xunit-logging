// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    public static class IntegrationTests
    {
        [Fact]
        public static void Can_Configure_xunit_For_ILoggerBuilder()
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
        public static void Can_Configure_xunit_For_ILoggerBuilder_With_Configuration()
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
