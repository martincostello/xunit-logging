// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// Defines a property for accessing an <see cref="ITestOutputHelper"/>.
/// </summary>
public interface ITestOutputHelperAccessor
{
    /// <summary>
    /// Gets or sets the <see cref="ITestOutputHelper"/> to use.
    /// </summary>
    ITestOutputHelper? OutputHelper { get; set; }
}
