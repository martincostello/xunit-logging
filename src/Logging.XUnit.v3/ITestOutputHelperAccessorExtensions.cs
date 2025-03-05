// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.ComponentModel;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class containing extension methods for <see cref="ITestOutputHelperAccessor"/>. This class cannot be inherited.
/// </summary>
[EditorBrowsable(EditorBrowsableState.Never)]
public static partial class ITestOutputHelperAccessorExtensions
{
    /// <summary>
    /// Captures the current <see cref="ITestOutputHelper"/> from <see cref="TestContext.Current"/>.
    /// </summary>
    /// <typeparam name="T">The type of the implementation of <see cref="ITestOutputHelperAccessor"/>.</typeparam>
    /// <param name="accessor">The <typeparamref name="T"/> to store the captured output helper in.</param>
    /// <returns>
    /// The value specified by <paramref name="accessor"/>.
    /// </returns>
    /// <remarks>
    /// This method must be called from the test class constructor or the test method itself.
    /// If this method is called from a background task or another thread, the <see cref="ITestOutputHelper"/>
    /// associated with the test will not be captured and no log output will be observed.
    /// </remarks>
    /// <exception cref="ArgumentNullException">
    /// <paramref name="accessor"/> is <see langword="null"/>.
    /// </exception>
    public static T WithOutputHelperFromTestContext<T>(this T accessor)
        where T : ITestOutputHelperAccessor
    {
#if NET
        ArgumentNullException.ThrowIfNull(accessor);
#else
        if (accessor == null)
        {
            throw new ArgumentNullException(nameof(accessor));
        }
#endif

        accessor.OutputHelper = TestContext.Current.TestOutputHelper;

        return accessor;
    }
}
