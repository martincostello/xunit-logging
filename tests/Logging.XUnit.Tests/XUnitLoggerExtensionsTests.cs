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
            Action<XUnitLoggerOptions> configure = (p) => { };

            // Act and Assert
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder).AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("builder", () => (null as ILoggingBuilder).AddXUnit(outputHelper, configure));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(null as ITestOutputHelper));
            Assert.Throws<ArgumentNullException>("outputHelper", () => builder.AddXUnit(null, configure));
            Assert.Throws<ArgumentNullException>("configure", () => builder.AddXUnit(outputHelper, null as Action<XUnitLoggerOptions>));
        }

        [Fact]
        public static void AddXUnit_For_ILoggerFactory_Validates_Parameters()
        {
            // Arrange
            ILoggerFactory factory = NullLoggerFactory.Instance;
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();
            Action<XUnitLoggerOptions> configureAction = (p) => { };
            Func<XUnitLoggerOptions> configureFunc = () => new XUnitLoggerOptions();
            Func<string, LogLevel, bool> filter = (c, l) => true;

            // Act and Assert
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, options));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, configureAction));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, configureFunc));
            Assert.Throws<ArgumentNullException>("factory", () => (null as ILoggerFactory).AddXUnit(outputHelper, filter));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null as ITestOutputHelper));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, options));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, configureAction));
            Assert.Throws<ArgumentNullException>("outputHelper", () => factory.AddXUnit(null, configureFunc));
            Assert.Throws<ArgumentNullException>("options", () => factory.AddXUnit(outputHelper, null as XUnitLoggerOptions));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, null as Action<XUnitLoggerOptions>));
            Assert.Throws<ArgumentNullException>("configure", () => factory.AddXUnit(outputHelper, null as Func<XUnitLoggerOptions>));
            Assert.Throws<ArgumentNullException>("filter", () => factory.AddXUnit(outputHelper, null as Func<string, LogLevel, bool>));
        }
    }
}
