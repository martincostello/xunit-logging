// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;
using Microsoft.Extensions.Logging;

#if XUNIT_V3
namespace Xunit;
#else
#pragma warning disable IDE0130
namespace Xunit.Abstractions;
#endif

/// <summary>
/// A class containing extension methods for the <see cref="IMessageSink"/> interface. This class cannot be inherited.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static class IMessageSinkExtensions
{
    /// <summary>
    /// Returns an <see cref="ILoggerFactory"/> that logs to the message sink.
    /// </summary>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to create the logger factory from.</param>
    /// <returns>
    /// An <see cref="ILoggerFactory"/> that writes messages to the message sink.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="messageSink"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory ToLoggerFactory(this IMessageSink messageSink)
    {
#if NET
        ArgumentNullException.ThrowIfNull(messageSink);
#else
        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }
#endif

        return new LoggerFactory().AddXUnit(messageSink);
    }

    /// <summary>
    /// Returns an <see cref="ILogger{T}"/> that logs to the message sink.
    /// </summary>
    /// <typeparam name="T">The type of the logger to create.</typeparam>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to create the logger from.</param>
    /// <returns>
    /// An <see cref="ILogger{T}"/> that writes messages to the message sink.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="messageSink"/> is <see langword="null"/>.
    /// </exception>
    public static ILogger<T> ToLogger<T>(this IMessageSink messageSink)
        => messageSink.ToLoggerFactory().CreateLogger<T>();
}
