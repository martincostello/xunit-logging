// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using Moq;
using Shouldly;
using Xunit;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MartinCostello.Logging.XUnit
{
    public static class XUnitLoggerTests
    {
        [Fact]
        public static void XUnitLogger_Validates_Parameters()
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterFalse,
            };

            // Act and Assert
            Assert.Throws<ArgumentNullException>("name", () => new XUnitLogger(null!, outputHelper, options));
            Assert.Throws<ArgumentNullException>("outputHelper", () => new XUnitLogger(name, (null as ITestOutputHelper)!, options));
            Assert.Throws<ArgumentNullException>("messageSink", () => new XUnitLogger(name, (null as IMessageSink)!, options));
            Assert.Throws<ArgumentNullException>("accessor", () => new XUnitLogger(name, (null as ITestOutputHelperAccessor)!, options));
            Assert.Throws<ArgumentNullException>("accessor", () => new XUnitLogger(name, (null as IMessageSinkAccessor)!, options));

            // Arrange
            var logger = new XUnitLogger(name, outputHelper, options);

            // Act and Assert
            Assert.Throws<ArgumentNullException>("value", () => logger.Filter = null!);
            Assert.Throws<ArgumentNullException>("value", () => logger.MessageSinkMessageFactory = null!);

            // Arrange
            Func<string?, LogLevel, bool> filter = (_, _) => true;

            // Act
            logger.Filter = filter;

            // Assert
            logger.Filter.ShouldBeSameAs(filter);
        }

        [Theory]
        [InlineData(Constructor.ITestOutputHelper)]
        [InlineData(Constructor.IMessageSink)]
        public static void XUnitLogger_Constructor_Initializes_Instance(Constructor constructor)
        {
            // Arrange
            string name = "MyName";
            var testOutputHelper = Mock.Of<ITestOutputHelper>();
            var messageSink = Mock.Of<IMessageSink>();
            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                MessageSinkMessageFactory = DiagnosticMessageFactory,
                IncludeScopes = true,
            };

            XUnitLogger CreateLogger(XUnitLoggerOptions? opts)
            {
                return constructor switch
                {
                    Constructor.ITestOutputHelper => new XUnitLogger(name, testOutputHelper, opts),
                    Constructor.IMessageSink => new XUnitLogger(name, messageSink, opts),
                    _ => throw new ArgumentOutOfRangeException(nameof(constructor), constructor, null),
                };
            }

            // Act
            var actual = CreateLogger(options);

            // Assert
            actual.Filter.ShouldBeSameAs(options.Filter);
            actual.MessageSinkMessageFactory.ShouldBeSameAs(options.MessageSinkMessageFactory);
            actual.IncludeScopes.ShouldBeTrue();
            actual.Name.ShouldBe(name);

            // Act
            actual = CreateLogger(null);

            // Assert
            actual.Filter.ShouldNotBeNull();
            actual.Filter(null, LogLevel.None).ShouldBeTrue();
            actual.MessageSinkMessageFactory.ShouldNotBeNull();
            actual.MessageSinkMessageFactory("message").ShouldBeOfType<DiagnosticMessage>();
            actual.IncludeScopes.ShouldBeFalse();
            actual.Name.ShouldBe(name);
        }

        [Fact]
        public static void XUnitLogger_BeginScope_Returns_Value()
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();
            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            using IDisposable actual = logger.BeginScope(true);

            // Assert
            actual.ShouldNotBeNull();
        }

        [Fact]
        public static void XUnitLogger_BeginScope_Throws_If_State_Is_Null()
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();
            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            Assert.Throws<ArgumentNullException>("state", () => logger.BeginScope(null as string));
        }

        [Theory]
        [InlineData(LogLevel.Critical, true)]
        [InlineData(LogLevel.Debug, false)]
        [InlineData(LogLevel.Error, true)]
        [InlineData(LogLevel.Information, false)]
        [InlineData(LogLevel.None, false)]
        [InlineData(LogLevel.Trace, false)]
        [InlineData(LogLevel.Warning, true)]
        public static void XUnitLogger_IsEnabled_Returns_Correct_Result(LogLevel logLevel, bool expected)
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();

            bool CustomFilter(string? categoryName, LogLevel level)
            {
                categoryName.ShouldBe(name);
                level.ShouldBe(logLevel);
                return level > LogLevel.Information;
            }

            var options = new XUnitLoggerOptions()
            {
                Filter = CustomFilter,
            };

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            bool actual = logger.IsEnabled(logLevel);

            // Assert
            actual.ShouldBe(expected);
        }

        [Fact]
        public static void XUnitLogger_Log_Throws_If_Formatter_Is_Null()
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act and Assert
            Assert.Throws<ArgumentNullException>("formatter", () => logger.Log(LogLevel.Information, new EventId(2), true, null, null!));
        }

        [Fact]
        public static void XUnitLogger_Log_Throws_If_LogLevel_Is_Invalid()
        {
            // Arrange
            string name = "MyName";
            var outputHelper = Mock.Of<ITestOutputHelper>();
            var options = new XUnitLoggerOptions();

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act and Assert
            Assert.Throws<ArgumentOutOfRangeException>("logLevel", () => logger.Log((LogLevel)int.MaxValue, 0, "state", null, Formatter));
        }

        [Fact]
        public static void XUnitLogger_Log_Does_Nothing_If_Not_Enabled()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterFalse,
            };

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            logger.Log(LogLevel.Information, new EventId(2), "state", null, Formatter);

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public static void XUnitLogger_Log_Does_Nothing_If_Null_Message_And_No_Exception()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            logger.Log(LogLevel.Information, new EventId(2), "state", null, FormatterNull);

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public static void XUnitLogger_Log_Does_Nothing_If_Empty_Message_And_No_Exception()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            logger.Log(LogLevel.Information, new EventId(2), "state", null, FormatterEmpty);

            // Assert
            mock.Verify((p) => p.WriteLine(It.IsAny<string>()), Times.Never());
        }

        [Fact]
        public static void XUnitLogger_Log_Does_Nothing_If_No_OutputHelper()
        {
            // Arrange
            string name = "MyName";
            var accessor = Mock.Of<ITestOutputHelperAccessor>();

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, accessor, options);

            // Act (no Assert)
            logger.Log(LogLevel.Information, new EventId(2), "state", null, Formatter);
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Only_Exception()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            var exception = new InvalidOperationException("Invalid");

            string expected = string.Join(
                Environment.NewLine,
                new[] { "[2018-08-19 16:12:16Z] info: MyName[2]", "System.InvalidOperationException: Invalid" });

            // Act
            logger.Log(LogLevel.Information, new EventId(2), "state", exception, FormatterNull);

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Message_And_Exception()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            var exception = new InvalidOperationException("Invalid");

            string expected = string.Join(
                Environment.NewLine,
                new[] { "[2018-08-19 16:12:16Z] warn: MyName[3]", "      Message|False|True", "System.InvalidOperationException: Invalid" });

            // Act
            logger.Log<string>(LogLevel.Warning, new EventId(3), null, exception, Formatter);

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Message_And_No_Exception()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                new[] { "[2018-08-19 16:12:16Z] fail: MyName[4]", "      Message|False|False" });

            // Act
            logger.Log<string>(LogLevel.Error, new EventId(4), null, null, Formatter);

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Theory]
        [InlineData(LogLevel.Critical, "crit")]
        [InlineData(LogLevel.Debug, "dbug")]
        [InlineData(LogLevel.Error, "fail")]
        [InlineData(LogLevel.Information, "info")]
        [InlineData(LogLevel.Trace, "trce")]
        [InlineData(LogLevel.Warning, "warn")]
        public static void XUnitLogger_Log_Logs_Messages(LogLevel logLevel, string shortLevel)
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "Your Name";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                new[] { $"[2018-08-19 16:12:16Z] {shortLevel}: Your Name[85]", "      Message|True|False" });

            // Act
            logger.Log(logLevel, new EventId(85), "Martin", null, Formatter);

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Very_Long_Messages()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
            };

            var logger = new XUnitLogger(name, outputHelper, options);

            // Act
            logger.Log(LogLevel.Information, 1, "state", null, FormatterLong);

            // Assert
            mock.Verify((p) => p.WriteLine(It.Is<string>((r) => r.Length > 1024)), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_But_There_Are_No_Scopes()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                new[] { "[2018-08-19 16:12:16Z] info: MyName[0]", "      Message|False|False" });

            // Act
            logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_And_There_Are_Scopes()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                "[2018-08-19 16:12:16Z] info: MyName[0]",
                "      => _",
                "        => __",
                "          => ___",
                "            => {OriginalFormat}: [null]",
                "      Message|False|False");

            // Act
            using (logger.BeginScope("_"))
            {
                using (logger.BeginScope("__"))
                {
                    using (logger.BeginScope("___"))
                    {
                        using (logger.BeginScope(null))
                        {
                            logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);
                        }
                    }
                }
            }

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_And_There_Is_Scope_Of_KeyValuePair()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                "[2018-08-19 16:12:16Z] info: MyName[0]",
                "      => ScopeKey: ScopeValue",
                "      Message|False|False");

            // Act
            using (logger.BeginScope(new[]
            {
                new KeyValuePair<string, object>("ScopeKey", "ScopeValue"),
            }))
            {
                logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);
            }

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_And_There_Is_Scope_Of_KeyValuePairs()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                "[2018-08-19 16:12:16Z] info: MyName[0]",
                "      => ScopeKeyOne: ScopeValueOne",
                "      => ScopeKeyTwo: ScopeValueTwo",
                "      => ScopeKeyThree: ScopeValueThree",
                "      Message|False|False");

            // Act
            using (logger.BeginScope(new[]
            {
                new KeyValuePair<string, object>("ScopeKeyOne", "ScopeValueOne"),
                new KeyValuePair<string, object>("ScopeKeyTwo", "ScopeValueTwo"),
                new KeyValuePair<string, object>("ScopeKeyThree", "ScopeValueThree"),
            }))
            {
                logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);
            }

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_And_There_Are_Scopes_Of_KeyValuePairs()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                "[2018-08-19 16:12:16Z] info: MyName[0]",
                "      => ScopeKeyOne: ScopeValueOne",
                "      => ScopeKeyTwo: ScopeValueTwo",
                "      => ScopeKeyThree: ScopeValueThree",
                "        => ScopeKeyFour: ScopeValueFour",
                "        => ScopeKeyFive: ScopeValueFive",
                "        => ScopeKeySix: ScopeValueSix",
                "      Message|False|False");

            // Act
            using (logger.BeginScope(new[]
            {
                new KeyValuePair<string, object>("ScopeKeyOne", "ScopeValueOne"),
                new KeyValuePair<string, object>("ScopeKeyTwo", "ScopeValueTwo"),
                new KeyValuePair<string, object>("ScopeKeyThree", "ScopeValueThree"),
            }))
            {
                using (logger.BeginScope(new[]
                {
                    new KeyValuePair<string, object>("ScopeKeyFour", "ScopeValueFour"),
                    new KeyValuePair<string, object>("ScopeKeyFive", "ScopeValueFive"),
                    new KeyValuePair<string, object>("ScopeKeySix", "ScopeValueSix"),
                }))
                {
                    logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);
                }
            }

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        [Fact]
        public static void XUnitLogger_Log_Logs_Message_If_Scopes_Included_And_There_Is_Scope_Of_IEnumerable()
        {
            // Arrange
            var mock = new Mock<ITestOutputHelper>();

            string name = "MyName";
            var outputHelper = mock.Object;

            var options = new XUnitLoggerOptions()
            {
                Filter = FilterTrue,
                IncludeScopes = true,
            };

            var logger = new XUnitLogger(name, outputHelper, options)
            {
                Clock = StaticClock,
            };

            string expected = string.Join(
                Environment.NewLine,
                "[2018-08-19 16:12:16Z] info: MyName[0]",
                "      => ScopeKeyOne",
                "      => ScopeKeyTwo",
                "      => ScopeKeyThree",
                "      Message|False|False");

            // Act
            using (logger.BeginScope(new[] { "ScopeKeyOne", "ScopeKeyTwo", "ScopeKeyThree" }))
            {
                logger.Log<string>(LogLevel.Information, 0, null, null, Formatter);
            }

            // Assert
            mock.Verify((p) => p.WriteLine(expected), Times.Once());
        }

        private static DateTimeOffset StaticClock() => new(2018, 08, 19, 17, 12, 16, TimeSpan.FromHours(1));

        private static IMessageSinkMessage DiagnosticMessageFactory(string message) => new DiagnosticMessage(message);

        private static bool FilterTrue(string? categoryName, LogLevel level) => true;

        private static bool FilterFalse(string? categoryName, LogLevel level) => false;

        private static string Formatter<TState>(TState? state, Exception? exception)
            where TState : class
        {
            return $"Message|{(state == null ? bool.FalseString : bool.TrueString)}|{(exception == null ? bool.FalseString : bool.TrueString)}";
        }

        private static string FormatterEmpty<TState>(TState? state, Exception? exception) => string.Empty;

        private static string FormatterLong<TState>(TState? state, Exception? exception) => new('a', 2048);

        private static string FormatterNull<TState>(TState? state, Exception? exception) => null!;
    }
}
