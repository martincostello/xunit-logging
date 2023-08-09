// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an <see cref="ILoggerProvider"/> to use with xunit.
/// </summary>
public partial class XUnitLoggerProvider
{
    /// <summary>
    /// The <see cref="ITestOutputHelperAccessor"/> to use. This field is readonly.
    /// </summary>
    private readonly ITestOutputHelperAccessor? _outputHelperAccessor;

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLoggerProvider"/> class.
    /// </summary>
    /// <param name="outputHelper">The <see cref="ITestOutputHelper"/> to use.</param>
    /// <param name="options">The options to use for logging to xunit.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="outputHelper"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public XUnitLoggerProvider(ITestOutputHelper outputHelper, XUnitLoggerOptions options)
        : this(new TestOutputHelperAccessor(outputHelper), options)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="XUnitLoggerProvider"/> class.
    /// </summary>
    /// <param name="accessor">The <see cref="ITestOutputHelperAccessor"/> to use.</param>
    /// <param name="options">The options to use for logging to xunit.</param>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="accessor"/> or <paramref name="options"/> is <see langword="null"/>.
    /// </exception>
    public XUnitLoggerProvider(ITestOutputHelperAccessor accessor, XUnitLoggerOptions options)
    {
        _outputHelperAccessor = accessor ?? throw new ArgumentNullException(nameof(accessor));
        _options = options ?? throw new ArgumentNullException(nameof(options));
    }
}
