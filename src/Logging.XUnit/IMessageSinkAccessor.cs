// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Xunit.Sdk;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// Defines a property for accessing an <see cref="IMessageSink"/>.
/// </summary>
public interface IMessageSinkAccessor
{
    /// <summary>
    /// Gets or sets the <see cref="IMessageSink"/> to use.
    /// </summary>
    IMessageSink? MessageSink { get; set; }
}
