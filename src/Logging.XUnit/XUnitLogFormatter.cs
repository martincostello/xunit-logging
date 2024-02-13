// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
#if !NETSTANDARD2_0
using Microsoft.Extensions.Logging.Abstractions;
#endif

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class that supports custom log message formatting.
/// </summary>
public abstract class XUnitLogFormatter
{
#if NETSTANDARD2_0
    /// <summary>
    /// Writes the log message to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    /// <param name="logLevel">Entry will be written on this level.</param>
    /// <param name="category">The log category of the event.</param>
    /// <param name="eventId">Id of the event.</param>
    /// <param name="state">The entry to be written. Can also be an object.</param>
    /// <param name="formatter">A delegate to create a message of <paramref name="state"/> and <paramref name="exception"/>.</param>
    /// <param name="exception">The exception related to this entry.</param>
    /// <param name="scopeProvider">The provider of scope data.</param>
    /// <param name="textWriter">The <see cref="TextWriter"/> to write the log entry to.</param>
    public abstract void Write<TState>(
        LogLevel logLevel,
        string category,
        EventId eventId,
        TState state,
        Func<TState, Exception?, string> formatter,
        Exception? exception,
        IEnumerable<TState>? scopeProvider,
        TextWriter textWriter);
#else
    /// <summary>
    /// Writes the log message to the specified <see cref="TextWriter"/>.
    /// </summary>
    /// <typeparam name="TState">The type of the object to be written.</typeparam>
    /// <param name="logEntry">The log entry.</param>
    /// <param name="scopeProvider">The provider of scope data.</param>
    /// <param name="textWriter">The <see cref="TextWriter"/> to write the log entry to.</param>
    public abstract void Write<TState>(
        in LogEntry<TState> logEntry,
        IExternalScopeProvider? scopeProvider,
        TextWriter textWriter);
#endif
}
