// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing an <see cref="ILoggerProvider"/> to use with xunit.
    /// </summary>
    public class XUnitLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// The <see cref="ITestOutputHelper"/> to use. This field is readonly.
        /// </summary>
        private readonly ITestOutputHelper _outputHelper;

        /// <summary>
        /// The <see cref="XUnitLoggerOptions"/> to use. This field is readonly.
        /// </summary>
        private readonly XUnitLoggerOptions _options;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLoggerProvider"/> class.
        /// </summary>
        /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
        /// <param name="options">The options to use for logging to xunit.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="outputHelper"/> or <paramref name="options"/> is <see langword="null"/>.
        /// </exception>
        public XUnitLoggerProvider(ITestOutputHelper outputHelper, XUnitLoggerOptions options)
        {
            _outputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
            _options = options ?? throw new ArgumentNullException(nameof(options));
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="XUnitLoggerProvider"/> class.
        /// </summary>
        ~XUnitLoggerProvider()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public virtual ILogger CreateLogger(string categoryName) => new XUnitLogger(categoryName, _outputHelper, _options);

        /// <inheritdoc />
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <param name="disposing">
        /// <see langword="true" /> to release both managed and unmanaged resources;
        /// <see langword="false" /> to release only unmanaged resources.
        /// </param>
        protected virtual void Dispose(bool disposing)
        {
            // Nothing to dispose of
        }
    }
}
