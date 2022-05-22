// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using MartinCostello.Logging.XUnit;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Xunit.Abstractions;

namespace Microsoft.Extensions.Logging;

/// <summary>
/// A class containing extension methods for configuring logging to xunit. This class cannot be inherited.
/// </summary>
public static partial class XUnitLoggerExtensions
{
    /// <summary>
    /// Adds an xunit logger to the logging builder.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="accessor">The <see cref="IMessageSinkAccessor"/> to use.</param>
    /// <returns>
    /// The instance of <see cref="ILoggingBuilder"/> specified by <paramref name="builder"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="accessor"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, IMessageSinkAccessor accessor)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (accessor == null)
        {
            throw new ArgumentNullException(nameof(accessor));
        }

        return builder.AddXUnit(accessor, static (_) => { });
    }

    /// <summary>
    /// Adds an xunit logger to the logging builder.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="accessor">The <see cref="IMessageSinkAccessor"/> to use.</param>
    /// <param name="configure">A delegate to a method to use to configure the logging options.</param>
    /// <returns>
    /// The instance of <see cref="ILoggingBuilder"/> specified by <paramref name="builder"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/>, <paramref name="accessor"/> or <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, IMessageSinkAccessor accessor, Action<XUnitLoggerOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (accessor == null)
        {
            throw new ArgumentNullException(nameof(accessor));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new XUnitLoggerOptions();

        configure(options);

#pragma warning disable CA2000
        builder.AddProvider(new XUnitLoggerProvider(accessor, options));
#pragma warning restore CA2000

        builder.Services.TryAddSingleton(accessor);

        return builder;
    }

    /// <summary>
    /// Adds an xunit logger to the logging builder.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <returns>
    /// The instance of <see cref="ILoggingBuilder"/> specified by <paramref name="builder"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/> or <paramref name="messageSink"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, IMessageSink messageSink)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        return builder.AddXUnit(messageSink, static (_) => { });
    }

    /// <summary>
    /// Adds an xunit logger to the logging builder.
    /// </summary>
    /// <param name="builder">The <see cref="ILoggingBuilder"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="configure">A delegate to a method to use to configure the logging options.</param>
    /// <returns>
    /// The instance of <see cref="ILoggingBuilder"/> specified by <paramref name="builder"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="builder"/>, <paramref name="messageSink"/> or <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggingBuilder AddXUnit(this ILoggingBuilder builder, IMessageSink messageSink, Action<XUnitLoggerOptions> configure)
    {
        if (builder == null)
        {
            throw new ArgumentNullException(nameof(builder));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = new XUnitLoggerOptions();

        configure(options);

#pragma warning disable CA2000
        return builder.AddProvider(new XUnitLoggerProvider(messageSink, options));
#pragma warning restore CA2000
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="minLevel">The minimum <see cref="LogLevel"/> to be logged.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="messageSink"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink, LogLevel minLevel)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        return factory.AddXUnit(messageSink, (_, level) => level >= minLevel);
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="filter">The category filter to apply to logs.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/>, <paramref name="messageSink"/> or <paramref name="filter"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink, Func<string?, LogLevel, bool> filter)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        if (filter == null)
        {
            throw new ArgumentNullException(nameof(filter));
        }

        return factory.AddXUnit(messageSink, (options) => options.Filter = filter);
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/> or <paramref name="messageSink"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        return factory.AddXUnit(messageSink, static (_) => { });
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="options">The options to use for logging to xunit.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/>, <paramref name="messageSink"/> OR <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink, XUnitLoggerOptions options)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        if (options == null)
        {
            throw new ArgumentNullException(nameof(options));
        }

        return factory.AddXUnit(messageSink, () => options);
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="configure">A delegate to a method to use to configure the logging options.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/>, <paramref name="messageSink"/> or <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink, Action<XUnitLoggerOptions> configure)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        return factory.AddXUnit(
            messageSink,
            () =>
            {
                var options = new XUnitLoggerOptions();
                configure(options);
                return options;
            });
    }

    /// <summary>
    /// Adds an xunit logger to the factory.
    /// </summary>
    /// <param name="factory">The <see cref="ILoggerFactory"/> to use.</param>
    /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
    /// <param name="configure">A delegate to a method that returns a configured <see cref="XUnitLoggerOptions"/> to use.</param>
    /// <returns>
    /// The instance of <see cref="ILoggerFactory"/> specified by <paramref name="factory"/>.
    /// </returns>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="factory"/>, <paramref name="messageSink"/> or <paramref name="configure"/> is <see langword="null"/>.
    /// </exception>
    public static ILoggerFactory AddXUnit(this ILoggerFactory factory, IMessageSink messageSink, Func<XUnitLoggerOptions> configure)
    {
        if (factory == null)
        {
            throw new ArgumentNullException(nameof(factory));
        }

        if (messageSink == null)
        {
            throw new ArgumentNullException(nameof(messageSink));
        }

        if (configure == null)
        {
            throw new ArgumentNullException(nameof(configure));
        }

        var options = configure();

#pragma warning disable CA2000
        factory.AddProvider(new XUnitLoggerProvider(messageSink, options));
#pragma warning restore CA2000

        return factory;
    }
}
