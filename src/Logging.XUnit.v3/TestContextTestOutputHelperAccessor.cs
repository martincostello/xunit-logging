// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an implementation of <see cref="ITestOutputHelperAccessor"/> that
/// retrieves the <see cref="ITestOutputHelper"/> from <see cref="TestContext.Current"/>. This class cannot be inherited.
/// </summary>
internal sealed class TestContextTestOutputHelperAccessor : ITestOutputHelperAccessor
{
    /// <summary>
    /// The singleton instance of <see cref="TestContextTestOutputHelperAccessor"/>. This field is read-only.
    /// </summary>
    internal static readonly TestContextTestOutputHelperAccessor Instance = new();

    /// <summary>
    /// Gets or sets the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    /// <exception cref="NotSupportedException">
    /// An attempt to set the value of this property is made.
    /// </exception>
    public ITestOutputHelper? OutputHelper
    {
        get => TestContext.Current.TestOutputHelper;
        set => throw new NotSupportedException($"Setting this property is not supported when xunit's {nameof(TestContext)} is used as the source of the current {nameof(ITestOutputHelper)}.");
    }
}
