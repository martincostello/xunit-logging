// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an implementation of <see cref="ITestOutputHelperAccessor"/> that
/// uses <see cref="AmbientTestOutputHelperAccessor"/> with <see cref="TestContextTestOutputHelperAccessor"/>
/// as a fallback. This class cannot be inherited.
/// </summary>
internal sealed class CompositeTestOutputHelperAccessor : ITestOutputHelperAccessor
{
    /// <summary>
    /// The singleton instance of <see cref="CompositeTestOutputHelperAccessor"/>. This field is read-only.
    /// </summary>
    internal static readonly CompositeTestOutputHelperAccessor Instance = new();

    /// <summary>
    /// Gets or sets the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    public ITestOutputHelper? OutputHelper
    {
        get => AmbientTestOutputHelperAccessor.Instance.OutputHelper ?? TestContextTestOutputHelperAccessor.Instance.OutputHelper;
        set => AmbientTestOutputHelperAccessor.Instance.OutputHelper = value;
    }
}
