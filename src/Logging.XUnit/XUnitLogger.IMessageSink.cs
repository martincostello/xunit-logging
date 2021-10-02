// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing an <see cref="ILogger"/> to use with xunit.
    /// </summary>
    public partial class XUnitLogger
    {
        /// <summary>
        /// The <see cref="IMessageSinkAccessor"/> to use. This field is read-only.
        /// </summary>
        private readonly IMessageSinkAccessor? _messageSinkAccessor;

        /// <summary>
        /// Gets or sets the message sink message factory to use when writing to an <see cref="IMessageSink"/>.
        /// </summary>
        private Func<string, IMessageSinkMessage> _messageSinkMessageFactory;

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
        /// </summary>
        /// <param name="name">The name for messages produced by the logger.</param>
        /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
        /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="messageSink"/> is <see langword="null"/>.
        /// </exception>
        public XUnitLogger(string name, IMessageSink messageSink, XUnitLoggerOptions? options)
            : this(name, new MessageSinkAccessor(messageSink), options)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
        /// </summary>
        /// <param name="name">The name for messages produced by the logger.</param>
        /// <param name="accessor">The <see cref="IMessageSinkAccessor"/> to use.</param>
        /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="name"/> or <paramref name="accessor"/> is <see langword="null"/>.
        /// </exception>
        public XUnitLogger(string name, IMessageSinkAccessor accessor, XUnitLoggerOptions? options)
            : this(name, options)
        {
            _messageSinkAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        }

        /// <summary>
        /// Gets or sets the message sink message factory to use when writing to an <see cref="IMessageSink"/>.
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="value"/> is <see langword="null"/>.
        /// </exception>
        public Func<string, IMessageSinkMessage> MessageSinkMessageFactory
        {
            get { return _messageSinkMessageFactory; }
            set { _messageSinkMessageFactory = value ?? throw new ArgumentNullException(nameof(value)); }
        }
    }
}
