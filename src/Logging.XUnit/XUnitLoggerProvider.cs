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
    public partial class XUnitLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// The <see cref="XUnitLoggerOptions"/> to use. This field is readonly.
        /// </summary>
        private readonly XUnitLoggerOptions _options;

        /// <summary>
        /// Finalizes an instance of the <see cref="XUnitLoggerProvider"/> class.
        /// </summary>
        ~XUnitLoggerProvider()
        {
            Dispose(false);
        }

        /// <inheritdoc />
        public virtual ILogger CreateLogger(string categoryName)
        {
            if (_outputHelperAccessor != null)
            {
                return new XUnitLogger(categoryName, _outputHelperAccessor, _options);
            }

            if (_messageSinkAccessor != null)
            {
                return new XUnitLogger(categoryName, _messageSinkAccessor, _options);
            }

            throw new InvalidOperationException($"Either {nameof(_outputHelperAccessor)} or {nameof(_messageSinkAccessor)} must not be null.");
        }

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
