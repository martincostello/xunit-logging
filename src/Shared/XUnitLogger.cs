// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Text;
using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an <see cref="ILogger"/> to use with xunit.
/// </summary>
public partial class XUnitLogger : ILogger
{
    //// Based on https://github.com/dotnet/runtime/blob/65067052e433eda400c5e7cc9f7b21c84640f901/src/libraries/Microsoft.Extensions.Logging.Console/src/ConsoleLogger.cs#L41-L66

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
    /// The format string used to format the timestamp in log messages.
    /// </summary>
    private readonly string _timestampFormat;

    /// <summary>
    /// Gets or sets an optional method to override the writing of log messages.
    /// </summary>
    private readonly Action<XUnitLogger, ITestOutputHelper?, IMessageSink?, LogLevel, int, string?, Exception?>? _writeMessageOverride;

    /// <summary>
    /// Gets or sets the filter to use.
    /// </summary>
    private Func<string?, LogLevel, bool> _filter;

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
    /// </summary>
    /// <param name="name">The name for messages produced by the logger.</param>
    /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
    private XUnitLogger(string name, XUnitLoggerOptions? options)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));

        _filter = options?.Filter ?? (static (_, _) => true);
        _messageSinkMessageFactory = options?.MessageSinkMessageFactory ?? (static (message) => new DiagnosticMessage(message));
        _timestampFormat = options?.TimestampFormat ?? "u";
        IncludeScopes = options?.IncludeScopes ?? false;
        _writeMessageOverride = options?.WriteMessageOverride;
    }

    /// <summary>
    /// Gets or sets the category filter to apply to logs.
    /// </summary>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="value"/> is <see langword="null"/>.
    /// </exception>
    public Func<string?, LogLevel, bool> Filter
    {
        get => _filter;
        set => _filter = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <summary>
    /// Gets or sets a value indicating whether to include scopes.
    /// </summary>
    public bool IncludeScopes { get; set; }

    /// <summary>
    /// Gets the name of the logger.
    /// </summary>
    public string Name { get; }

    /// <summary>
    /// Gets or sets a delegate representing the system clock.
    /// </summary>
    internal Func<DateTimeOffset> Clock { get; set; } = static () => DateTimeOffset.Now;

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
         where TState : notnull
    {
#if NET
        ArgumentNullException.ThrowIfNull(state);
#else
        if (state == null)
        {
            throw new ArgumentNullException(nameof(state));
        }
#endif

        return XUnitLogScope.Push(state);
    }

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
    public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception? exception, Func<TState, Exception?, string> formatter)
    {
        if (!IsEnabled(logLevel))
        {
            return;
        }

#if NET
        ArgumentNullException.ThrowIfNull(formatter);
#else
        if (formatter == null)
        {
            throw new ArgumentNullException(nameof(formatter));
        }
#endif

        string? message = formatter(state, exception);

        if (!string.IsNullOrEmpty(message) || exception != null)
        {
            if (_writeMessageOverride != null)
            {
                _writeMessageOverride(
                    this,
                    _outputHelperAccessor?.OutputHelper,
                    _messageSinkAccessor?.MessageSink,
                    logLevel,
                    eventId.Id,
                    message,
                    exception);
            }
            else
            {
                WriteMessage(logLevel, eventId.Id, message, exception);
            }
        }
    }

    /// <summary>
    /// Writes a message to the <see cref="ITestOutputHelper"/> or <see cref="IMessageSink"/> associated with the instance.
    /// </summary>
    /// <param name="logLevel">The message to write will be written on this level.</param>
    /// <param name="eventId">The Id of the event.</param>
    /// <param name="message">The message to write.</param>
    /// <param name="exception">The exception related to this message.</param>
    public virtual void WriteMessage(LogLevel logLevel, int eventId, string? message, Exception? exception)
    {
        ITestOutputHelper? outputHelper = _outputHelperAccessor?.OutputHelper;
        IMessageSink? messageSink = _messageSinkAccessor?.MessageSink;

        if (outputHelper is null && messageSink is null)
        {
            return;
        }

        StringBuilder? logBuilder = _logBuilder;
        _logBuilder = null;

        logBuilder ??= new StringBuilder();

        string logLevelString = GetLogLevelString(logLevel);

        logBuilder.Append(LogLevelPadding);
        logBuilder.Append(Name);
        logBuilder.Append('[');
        logBuilder.Append(eventId);
        logBuilder.Append(']');
        logBuilder.AppendLine();

        if (IncludeScopes)
        {
            GetScopeInformation(logBuilder);
        }

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

            logBuilder.Append(exception);
        }

        // Prefix the formatted message so it renders like this:
        // [{timestamp}] {logLevelString}{message}
        logBuilder.Insert(0, logLevelString);
        logBuilder.Insert(0, "] ");
        logBuilder.Insert(0, Clock().ToString(_timestampFormat, CultureInfo.CurrentCulture));
        logBuilder.Insert(0, '[');

        string line = logBuilder.ToString();

        try
        {
            outputHelper?.WriteLine(line);

            if (messageSink != null)
            {
                var sinkMessage = _messageSinkMessageFactory(line);
                messageSink.OnMessage(sinkMessage);
            }
        }
        catch (InvalidOperationException)
        {
            // Ignore exception if the application tries to log after the test ends
            // but before the ITestOutputHelper is detached, e.g. "There is no currently active test."
        }

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
    /// Gets the scope information for the current operation.
    /// </summary>
    /// <param name="builder">The <see cref="StringBuilder"/> to write the scope to.</param>
    private static void GetScopeInformation(StringBuilder builder)
    {
        var current = XUnitLogScope.Current;

        var stack = new Stack<XUnitLogScope>();
        while (current != null)
        {
            stack.Push(current);
            current = current.Parent;
        }

        var depth = 0;
        static string DepthPadding(int depth) => new(' ', depth * 2);

        while (stack.Count > 0)
        {
            var elem = stack.Pop();
            foreach (var property in StringifyScope(elem))
            {
                builder.Append(MessagePadding)
                       .Append(DepthPadding(depth))
                       .Append("=> ")
                       .Append(property)
                       .AppendLine();
            }

            depth++;
        }
    }

    /// <summary>
    /// Returns one or more stringified properties from the log scope.
    /// </summary>
    /// <param name="scope">The <see cref="XUnitLogScope"/> to stringify.</param>
    /// <returns>An enumeration of scope properties from the current scope.</returns>
    private static IEnumerable<string?> StringifyScope(XUnitLogScope scope)
    {
        if (scope.State is IEnumerable<KeyValuePair<string, object>> pairs)
        {
            foreach (var pair in pairs)
            {
                yield return $"{pair.Key}: {pair.Value}";
            }
        }
        else if (scope.State is IEnumerable<string> entries)
        {
            foreach (var entry in entries)
            {
                yield return entry;
            }
        }
        else
        {
            yield return scope.ToString();
        }
    }
}
