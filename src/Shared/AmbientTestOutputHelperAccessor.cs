// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an implementation of <see cref="ITestOutputHelperAccessor"/> that
/// stores the <see cref="ITestOutputHelper"/> as an asynchronous local value. This class cannot be inherited.
/// </summary>
internal sealed class AmbientTestOutputHelperAccessor : ITestOutputHelperAccessor
{
    /// <summary>
    /// The singleton instance of <see cref="AmbientTestOutputHelperAccessor"/>. This field is read-only.
    /// </summary>
    internal static readonly AmbientTestOutputHelperAccessor Instance = new();

    /// <summary>
    /// A backing field for the <see cref="ITestOutputHelper"/> for the current thread.
    /// </summary>
    private static readonly AsyncLocal<ITestOutputHelper?> _current = new();

    /// <summary>
    /// Gets or sets the current <see cref="ITestOutputHelper"/>.
    /// </summary>
    public ITestOutputHelper? OutputHelper
    {
        get { return _current.Value; }
        set { _current.Value = value; }
    }
}
