// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
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
        /// <summary>
        /// The <see cref="ITestOutputHelper"/> to use. This field is readonly.
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
        /// <param name="filter">The category filter to apply to logs.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="outputHelper"/> is <see langword="null"/>.
        /// </exception>
        public XUnitLogger(string name, ITestOutputHelper outputHelper, Func<string, LogLevel, bool> filter)
        {
            Name = name ?? throw new ArgumentNullException(nameof(name));
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
            Filter = filter ?? ((category, logLevel) => true);
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
                _outputHelper.WriteLine($"[{DateTimeOffset.Now:u}] [{logLevel}:{Name}:{eventId}] {message}");
            }
        }
    }
}
