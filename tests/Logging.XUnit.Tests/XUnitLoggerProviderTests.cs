// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Moq;

namespace MartinCostello.Logging.XUnit
{
    public static class XUnitLoggerProviderTests
    {
        [Fact]
        public static void XUnitLoggerProvider_TestOutputHelper_Constructor_Validates_Parameters()
        {
            // Arrange
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var accessor = Mock.Of<ITestOutputHelperAccessor>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("outputHelper", () => new XUnitLoggerProvider((null as ITestOutputHelper)!, options));
            Assert.Throws<ArgumentNullException>("accessor", () => new XUnitLoggerProvider((null as ITestOutputHelperAccessor)!, options));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(outputHelper, null!));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(accessor, null!));
        }

        [Fact]
        public static void XUnitLoggerProvider_MessageSink_Constructor_Validates_Parameters()
        {
            // Arrange
            var messageSink = Mock.Of<IMessageSink>();
            var accessor = Mock.Of<IMessageSinkAccessor>();
            var options = new XUnitLoggerOptions();

            // Act and Assert
            Assert.Throws<ArgumentNullException>("messageSink", () => new XUnitLoggerProvider((null as IMessageSink)!, options));
            Assert.Throws<ArgumentNullException>("accessor", () => new XUnitLoggerProvider((null as IMessageSinkAccessor)!, options));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(messageSink, null!));
            Assert.Throws<ArgumentNullException>("options", () => new XUnitLoggerProvider(accessor, null!));
        }

        [Theory]
        [InlineData(Constructor.ITestOutputHelper)]
        [InlineData(Constructor.IMessageSink)]
        public static void XUnitLoggerProvider_Creates_Logger(Constructor constructor)
        {
            // Arrange
            var testOutputHelper = Mock.Of<ITestOutputHelper>();
            var messageSink = Mock.Of<IMessageSink>();
            var options = new XUnitLoggerOptions();

            string categoryName = "MyLogger";

            using var target = constructor switch
            {
                Constructor.ITestOutputHelper => new XUnitLoggerProvider(testOutputHelper, options),
                Constructor.IMessageSink => new XUnitLoggerProvider(messageSink, options),
                _ => throw new ArgumentOutOfRangeException(nameof(constructor), constructor, null),
            };

            // Act
            ILogger actual = target.CreateLogger(categoryName);

            // Assert
            actual.ShouldNotBeNull();

            var xunit = actual.ShouldBeOfType<XUnitLogger>();
            xunit.Name.ShouldBe(categoryName);
            xunit.Filter.ShouldBeSameAs(options.Filter);
            xunit.MessageSinkMessageFactory.ShouldBeSameAs(options.MessageSinkMessageFactory);
            xunit.IncludeScopes.ShouldBeFalse();
        }
    }
}
