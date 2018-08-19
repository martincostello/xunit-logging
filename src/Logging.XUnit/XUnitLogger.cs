// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions.Internal;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing an <see cref="ILogger"/> to use with xunit.
    /// </summary>
    public class XUnitLogger : ILogger
    {
        //// Based on https://github.com/aspnet/Logging/blob/master/src/Microsoft.Extensions.Logging.Console/ConsoleLogger.cs

        /// <summary>
        /// The padding to use for log levels.
        /// </summary>
        private const string LogLevelPadding = ": ";

        /// <summary>
        /// The padding to use for messages. This field is read-only.
        /// </summary>
        private static readonly string MessagePadding = new string(' ', GetLogLevelString(LogLevel.Debug).Length + LogLevelPadding.Length);

        /// <summary>
        /// The padding to use for new lines. This field is read-only.
        /// </summary>
        private static readonly string NewLineWithMessagePadding = Environment.NewLine + MessagePadding;

        /// <summary>
        /// The current builder to use to generate log messages.
        /// </summary>
        [ThreadStatic]
        private static StringBuilder _logBuilder;

        /// <summary>
        /// The <see cref="ITestOutputHelper"/> to use. This field is read-only.
        /// </summary>
        private readonly ITestOutputHelper _outputHelper;

        /// <summary>
        /// Gets or sets the filter to use.
        /// </summary>
        private Func<string, LogLevel, bool> _filter;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
        /// </summary>
        /// <param name="name">The name for messages produced by the logger.</param>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="outputHelper"/> is <see langword="null"/>.
        /// </exception>
        public XUnitLogger(string name, ITestOutputHelper outputHelper, XUnitLoggerOptions options)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
            Filter = options?.Filter ?? ((category, logLevel) => true);
        }

        /// <summary>
        /// Gets or sets the category filter to apply to logs.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        public Func<string, LogLevel, bool> Filter
        {
            get { return _filter; }
            set { _filter = value ?? throw new ArgumentNullException(nameof(value)); }
        }

        /// <summary>
        /// Gets the name of the logger.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets or sets a delegate representing the system clock.
        /// </summary>
        internal Func<DateTimeOffset> Clock { get; set; } = () => DateTimeOffset.Now;

        /// <inheritdoc />
        public IDisposable BeginScope<TState>(TState state) => NullScope.Instance;

        /// <inheritdoc />
        public bool IsEnabled(LogLevel logLevel)
        {
            if (logLevel == LogLevel.None)
            {
                return false;
            }

            return Filter(Name, logLevel);
        }

        /// <inheritdoc />
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            if (!IsEnabled(logLevel))
            {
                return;
            }

            if (formatter == null)
            {
                throw new ArgumentNullException(nameof(formatter));
            }

            string message = formatter(state, exception);

            if (!string.IsNullOrEmpty(message) || exception != null)
            {
                WriteMessage(logLevel, eventId.Id, message, exception);
            }
        }

        /// <summary>
        /// Writes a message to the <see cref="ITestOutputHelper"/> associated with the instance.
        /// </summary>
        /// <param name="logLevel">The message to write will be written on this level.</param>
        /// <param name="eventId">The Id of the event.</param>
        /// <param name="message">The message to write.</param>
        /// <param name="exception">The exception related to this message.</param>
        public virtual void WriteMessage(LogLevel logLevel, int eventId, string message, Exception exception)
        {
            StringBuilder logBuilder = _logBuilder;
            _logBuilder = null;

            if (logBuilder == null)
            {
                logBuilder = new StringBuilder();
            }

            string logLevelString = GetLogLevelString(logLevel);

            logBuilder.Append(LogLevelPadding);
            logBuilder.Append(Name);
            logBuilder.Append("[");
            logBuilder.Append(eventId);
            logBuilder.AppendLine("]");

            bool hasMessage = !string.IsNullOrEmpty(message);

            if (hasMessage)
            {
                logBuilder.Append(MessagePadding);

                int length = logBuilder.Length;
                logBuilder.Append(message);
                logBuilder.Replace(Environment.NewLine, NewLineWithMessagePadding, length, message.Length);
            }

            if (exception != null)
            {
                if (hasMessage)
                {
                    logBuilder.AppendLine();
                }

                logBuilder.Append(exception.ToString());
            }

            string formatted = logBuilder.ToString();
            _outputHelper.WriteLine($"[{Clock():u}] {logLevelString}{formatted}");

            logBuilder.Clear();

            if (logBuilder.Capacity > 1024)
            {
                logBuilder.Capacity = 1024;
            }

            _logBuilder = logBuilder;
        }

        /// <summary>
        /// Returns the string to use for the specified logging level.
        /// </summary>
        /// <param name="logLevel">The log level to get the representation for.</param>
        /// <returns>
        /// A <see cref="string"/> containing the text representation of <paramref name="logLevel"/>.
        /// </returns>
        private static string GetLogLevelString(LogLevel logLevel)
        {
            switch (logLevel)
            {
                case LogLevel.Critical:
                    return "crit";

                case LogLevel.Debug:
                    return "dbug";

                case LogLevel.Error:
                    return "fail";

                case LogLevel.Information:
                    return "info";

                case LogLevel.Trace:
                    return "trce";

                case LogLevel.Warning:
                    return "warn";

                default:
                    throw new ArgumentOutOfRangeException(nameof(logLevel));
            }
        }
    }
}
