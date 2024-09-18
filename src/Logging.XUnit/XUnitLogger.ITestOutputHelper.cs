// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an <see cref="ILogger"/> to use with xunit.
/// </summary>
public partial class XUnitLogger
{
    /// <summary>
    /// The <see cref="ITestOutputHelperAccessor"/> to use. This field is read-only.
    /// </summary>
    private readonly ITestOutputHelperAccessor? _outputHelperAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
    /// </summary>
    /// <param name="name">The name for messages produced by the logger.</param>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> or <paramref name="outputHelper"/> is <see langword="null"/>.
    /// </exception>
    public XUnitLogger(string name, ITestOutputHelper outputHelper, XUnitLoggerOptions? options)
        : this(name, new TestOutputHelperAccessor(outputHelper), options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLogger"/> class.
    /// </summary>
    /// <param name="name">The name for messages produced by the logger.</param>
    /// <param name="accessor">The <see cref="ITestOutputHelperAccessor"/> to use.</param>
    /// <param name="options">The <see cref="XUnitLoggerOptions"/> to use.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="name"/> or <paramref name="accessor"/> is <see langword="null"/>.
    /// </exception>
    public XUnitLogger(string name, ITestOutputHelperAccessor accessor, XUnitLoggerOptions? options)
        : this(name, options)
    {
        _outputHelperAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
    }
}
