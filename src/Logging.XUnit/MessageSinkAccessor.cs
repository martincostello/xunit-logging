// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing the default implementation of <see cref="IMessageSinkAccessor"/>. This class cannot be inherited.
    /// </summary>
    internal sealed class MessageSinkAccessor : IMessageSinkAccessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MessageSinkAccessor"/> class.
        /// </summary>
        /// <param name="messageSink">The <see cref="IMessageSink"/> to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="messageSink"/> is <see langword="null"/>.
        /// </exception>
        internal MessageSinkAccessor(IMessageSink messageSink)
        {
            MessageSink = messageSink ?? throw new ArgumentNullException(nameof(messageSink));
        }

        /// <summary>
        /// Gets or sets the current <see cref="IMessageSink"/>.
        /// </summary>
        public IMessageSink? MessageSink { get; set; }
    }
}
