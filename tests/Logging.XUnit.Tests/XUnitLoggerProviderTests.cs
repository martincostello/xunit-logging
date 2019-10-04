// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    public static class XUnitLoggerProviderTests
    {
        [Fact]
        public static void XUnitLoggerProvider_Constructor_Validates_Parameters()
        {
            // Arrange
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var accessor = Mock.Of<ITestOutputHelperAccessor>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("outputHelper", () => new XUnitLoggerProvider(null as ITestOutputHelper, options));
            Assert.Throws<ArgumentNullException>("accessor", () => new XUnitLoggerProvider(null as ITestOutputHelperAccessor, options));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(outputHelper, null));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(accessor, null));
        }

        [Fact]
        public static void XUnitLoggerProvider_Creates_Logger()
        {
            // Arrange
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();

            string categoryName = "MyLogger";

            using var target = new XUnitLoggerProvider(outputHelper, options);

            // Act
            ILogger actual = target.CreateLogger(categoryName);

            // Assert
            actual.ShouldNotBeNull();

            var xunit = actual.ShouldBeOfType<XUnitLogger>();
            xunit.Name.ShouldBe(categoryName);
            xunit.Filter.ShouldBeSameAs(options.Filter);
            xunit.IncludeScopes.ShouldBeFalse();
        }
    }
}
