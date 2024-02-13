// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an <see cref="ILogger"/> to use with xunit.
/// </summary>
public partial class XUnitLogger : ILogger
{
    //// Based on https://github.com/dotnet/runtime/blob/35562ee5ac02c68d42d5b77fb0af09123d79c3ba/src/libraries/Microsoft.Extensions.Logging.Console/src/ConsoleLogger.cs

    /// <summary>
    /// The current writer to use to generate log messages.
    /// </summary>
    [ThreadStatic]
    private static StringWriter? _textWriter;

    /// <summary>
    /// The <see cref="IExternalScopeProvider"/> to use. This field is read-only.
    /// </summary>
    private readonly IExternalScopeProvider _externalScopeProvider;

    /// <summary>
    /// The <see cref="XUnitLogFormatter"/> to use. This field is read-only.
    /// </summary>
    private readonly XUnitLogFormatter _formatter;

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
        : this(name, null as IExternalScopeProvider, options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
    /// </summary>
    /// <param name="name">The name for messages produced by the logger.</param>
    /// <param name="externalScopeProvider">The <see cref="IExternalScopeProvider"/> to use, if any.</param>
    /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
    private XUnitLogger(
        string name,
        IExternalScopeProvider? externalScopeProvider,
        XUnitLoggerOptions? options)
    {
        Name = name ?? throw new ArgumentNullException(nameof(name));
        _externalScopeProvider = externalScopeProvider ?? new XUnitLogScopeProvider();
        _formatter = options?.Formatter ?? new DefaultXUnitLogFormatter(new());
        _filter = options?.Filter ?? XUnitLoggerOptions.DefaultFilter;
        _messageSinkMessageFactory = options?.MessageSinkMessageFactory ?? XUnitLoggerOptions.DefaultMessageFactory;
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
    /// Gets the name of the logger.
    /// </summary>
    public string Name { get; }

    /// <inheritdoc />
    public IDisposable? BeginScope<TState>(TState state)
         where TState : notnull
    {
#if NET8_0_OR_GREATER
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

#if NET8_0_OR_GREATER
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
            ITestOutputHelper? outputHelper = _outputHelperAccessor?.OutputHelper;
            IMessageSink? messageSink = _messageSinkAccessor?.MessageSink;

            if (outputHelper is null && messageSink is null)
            {
                return;
            }

            StringWriter? textWriter = _textWriter;
            _textWriter = null;

#pragma warning disable CA2000
            textWriter ??= new StringWriter();
#pragma warning restore CA2000

#if NETSTANDARD2_0
            _formatter.Write(logLevel, Name, eventId, state, formatter, exception, null, textWriter);
#else
            var logEntry = new LogEntry<TState>(logLevel, Name, eventId, state, exception, formatter);
            _formatter.Write(in logEntry, _externalScopeProvider, textWriter);
#endif

            var logBuilder = textWriter.GetStringBuilder();

            if (logBuilder.Length == 0)
            {
                return;
            }

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

            _textWriter = textWriter;
        }
    }
}
