// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing the default implementation of <see cref="ITestOutputHelperAccessor"/>. This class cannot be inherited.
/// </summary>
internal sealed class TestOutputHelperAccessor : ITestOutputHelperAccessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="TestOutputHelperAccessor"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="outputHelper"/> is <see langword="null"/>.
    /// </exception>
    internal TestOutputHelperAccessor(ITestOutputHelper outputHelper)
    {
        OutputHelper = outputHelper ?? throw new ArgumentNullException(nameof(outputHelper));
    }

    /// <summary>
    /// Gets or sets the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    public ITestOutputHelper? OutputHelper { get; set; }
}
