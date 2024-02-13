// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing configuration options for logging to xunit.
/// </summary>
public class XUnitLoggerOptions
{
    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLoggerOptions"/> class.
    /// </summary>
    public XUnitLoggerOptions()
    {
    }

    /// <summary>
    /// Gets or sets the category filter to apply to logs.
    /// </summary>
    public Func<string?, LogLevel, bool> Filter { get; set; } = DefaultFilter;

    /// <summary>
    /// Gets or sets the <see cref="XUnitLogFormatter"/> to use.
    /// </summary>
    public XUnitLogFormatter Formatter { get; set; } = new DefaultXUnitLogFormatter(new XUnitLogFormatterOptions());

    /// <summary>
    /// Gets or sets the message sink message factory to use when writing to a <see cref="IMessageSink"/>.
    /// </summary>
    public Func<string, IMessageSinkMessage> MessageSinkMessageFactory { get; set; } = DefaultMessageFactory;

    /// <summary>
    /// The default filter to use for logging.
    /// </summary>
    /// <param name="category">The log category.</param>
    /// <param name="logLevel">The log level.</param>
    /// <returns>
    /// <see langword="true"/> if the log entry should be written; otherwise <see langword="false"/>.
    /// </returns>
    internal static bool DefaultFilter(string? category, LogLevel logLevel) => true; // By default log everything

    /// <summary>
    /// The default message sink message factory to use.
    /// </summary>
    /// <param name="message">The message.</param>
    /// <returns>
    /// The <see cref="IMessageSinkMessage"/> to use.
    /// </returns>
    internal static IMessageSinkMessage DefaultMessageFactory(string message) => new DiagnosticMessage(message);
}
