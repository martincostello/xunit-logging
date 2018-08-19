// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Moq;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    public static class XUnitLoggerExtensionsTests
    {
        [Fact]
        public static void AddXUnit_For_ILoggerBuilder_Validates_Parameters()
        {
            // Arrange
            var builder = Mock.Of<ILoggingBuilder>();
            var outputHelper = Mock.Of<ITestOutputHelper>();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder).AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder).AddXUnit(outputHelper, ConfigureAction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(null as ITestOutputHelper));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(null, ConfigureAction));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(outputHelper, null as Action<XUnitLoggerOptions>));
        }

        [Fact]
        public static void AddXUnit_For_ILoggerFactory_Validates_Parameters()
        {
            // Arrange
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var logLevel = LogLevel.Information;
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, options));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, ConfigureAction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, Filter));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, logLevel));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null as ITestOutputHelper));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, ConfigureAction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, ConfigureFunction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, Filter));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, logLevel));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, options));
            Assert.Throws<ArgumentNullException>("options", () => factory.AddXUnit(outputHelper, null as XUnitLoggerOptions));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, null as Action<XUnitLoggerOptions>));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, null as Func<XUnitLoggerOptions>));
            Assert.Throws<ArgumentNullException>("filter", () => factory.AddXUnit(outputHelper, null as Func<string, LogLevel, bool>));
        }

        [Fact]
        public static void ToLoggerFactory_Validates_Parameters()
        {
            // Arrange
            ITestOutputHelper outputHelper = null;

            // Act and Assert
            Assert.Throws<ArgumentNullException>("outputHelper", () => outputHelper.ToLoggerFactory());
        }

        private static void ConfigureAction(XUnitLoggerOptions options)
        {
        }

        private static XUnitLoggerOptions ConfigureFunction() => new XUnitLoggerOptions();

        private static bool Filter(string categoryName, LogLevel level) => true;
    }
}
