// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing the default log formatter. This class cannot be inherited.
/// </summary>
internal sealed class DefaultXUnitLogFormatter(XUnitLogFormatterOptions options) : XUnitLogFormatter
{
    /// <summary>
    /// The padding to use for log levels.
    /// </summary>
    private const string LogLevelPadding = ": ";

    /// <summary>
    /// The padding to use for messages. This field is read-only.
    /// </summary>
    private static readonly string MessagePadding = new(' ', GetLogLevelString(LogLevel.Debug).Length + LogLevelPadding.Length);

    /// <summary>
    /// The padding to use for new lines. This field is read-only.
    /// </summary>
    private static readonly string NewLineWithMessagePadding = Environment.NewLine + MessagePadding;

    /// <summary>
    /// The current builder to use to generate log messages.
    /// </summary>
    [ThreadStatic]
    private static StringBuilder? _logBuilder;

    /// <summary>
    /// The <see cref="XUnitLogFormatterOptions"/> to use. This field is read-only.
    /// </summary>
    private readonly XUnitLogFormatterOptions _options = options;

    /// <inheritdoc />
#if NETSTANDARD2_0
    public override void Write<TState>(
        LogLevel logLevel,
        string category,
        EventId eventId,
        TState state,
        Func<TState, Exception?, string> formatter,
        Exception? exception,
        IEnumerable<TState>? scopeProvider,
        TextWriter textWriter)
    {
        Action<StringBuilder>? scopeFormatter = null;

        if (scopeProvider is not null && _options.IncludeScopes)
        {
            scopeFormatter = (builder) =>
            {
                var depth = 0;
                static string DepthPadding(int depth) => new(' ', depth * 2);

                foreach (var scope in scopeProvider)
                {
                    foreach (var property in StringifyScope(scope))
                    {
                        builder.Append(MessagePadding)
                                .Append(DepthPadding(depth))
                                .Append("=> ")
                                .Append(property)
                                .AppendLine();
                    }

                    depth++;
                }
            };
        }

        WriteMessage(
            logLevel,
            category,
            eventId.Id,
            formatter(state, exception),
            exception,
            scopeFormatter,
            _options.TimeProvider.GetUtcNow,
            _options.TimestampFormat ?? "u",
            textWriter);
    }
#else
    public override void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter)
    {
        Action<StringBuilder>? scopeFormatter = null;

        if (scopeProvider is not null && _options.IncludeScopes)
        {
            var state = logEntry.State;
            scopeFormatter = (builder) =>
            {
                var depth = 0;
                static string DepthPadding(int depth) => new(' ', depth * 2);

                scopeProvider.ForEachScope(
                    (scope, state) =>
                    {
                        foreach (var property in StringifyScope(scope))
                        {
                            builder.Append(MessagePadding)
                                   .Append(DepthPadding(depth))
                                   .Append("=> ")
                                   .Append(property)
                                   .AppendLine();
                        }

                        depth++;
                    },
                    state);
            };
        }

        WriteMessage(
            logEntry.LogLevel,
            logEntry.Category,
            logEntry.EventId.Id,
            logEntry.Formatter(logEntry.State, logEntry.Exception),
            logEntry.Exception,
            scopeFormatter,
            _options.TimeProvider.GetUtcNow,
            _options.TimestampFormat ?? "u",
            textWriter);
    }
#endif

    /// <summary>
    /// Writes a message to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <param name="logLevel">The message to write will be written on this level.</param>
    /// <param name="category">The category of the log message.</param>
    /// <param name="eventId">The Id of the event.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="exception">The exception related to this message.</param>
    /// <param name="scopeFormatter">An optional delegate to a method to use for format the scopes.</param>
    /// <param name="clock">A delegate to a method to get the current date and time.</param>
    /// <param name="timestampFormat">The format string used to format the timestamp in log messages.</param>
    /// <param name="writer">The <see cref="TextWriter"/> to write the log message to.</param>
    private static void WriteMessage(
        LogLevel logLevel,
        string category,
        int eventId,
        string? message,
        Exception? exception,
        Action<StringBuilder>? scopeFormatter,
        Func<DateTimeOffset> clock,
        string timestampFormat,
        TextWriter writer)
    {
        StringBuilder? logBuilder = _logBuilder;
        _logBuilder = null;

        logBuilder ??= new StringBuilder();

        string logLevelString = GetLogLevelString(logLevel);

        logBuilder.Append(LogLevelPadding);
        logBuilder.Append(category);
        logBuilder.Append('[');
        logBuilder.Append(eventId);
        logBuilder.Append(']');
        logBuilder.AppendLine();

        scopeFormatter?.Invoke(logBuilder);

        bool hasMessage = !string.IsNullOrEmpty(message);

        if (hasMessage)
        {
            logBuilder.Append(MessagePadding);

            int length = logBuilder.Length;
            logBuilder.Append(message);
            logBuilder.Replace(Environment.NewLine, NewLineWithMessagePadding, length, message!.Length);
        }

        if (exception != null)
        {
            if (hasMessage)
            {
                logBuilder.AppendLine();
            }

            logBuilder.Append(exception.ToString());
        }

        // Prefix the formatted message so it renders like this:
        // [{timestamp}] {logLevelString}{message}
        logBuilder.Insert(0, logLevelString);
        logBuilder.Insert(0, "] ");
        logBuilder.Insert(0, clock().ToString(timestampFormat, CultureInfo.CurrentCulture));
        logBuilder.Insert(0, '[');

        writer.Write(logBuilder.ToString());
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
        return logLevel switch
        {
            LogLevel.Critical => "crit",
            LogLevel.Debug => "dbug",
            LogLevel.Error => "fail",
            LogLevel.Information => "info",
            LogLevel.Trace => "trce",
            LogLevel.Warning => "warn",
            _ => throw new ArgumentOutOfRangeException(nameof(logLevel)),
        };
    }

    /// <summary>
    /// Returns one or more stringified properties from the state in the log scope.
    /// </summary>
    /// <param name="state">The state for the current scope to stringify.</param>
    /// <returns>An enumeration of scope properties from the current scope's state.</returns>
    private static IEnumerable<string?> StringifyScope(object? state)
    {
        if (state is IEnumerable<KeyValuePair<string, object>> pairs)
        {
            foreach (var pair in pairs)
            {
                yield return $"{pair.Key}: {pair.Value}";
            }
        }
        else if (state is IEnumerable<string> entries)
        {
            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
        else
        {
            yield return state?.ToString();
        }
    }
}
