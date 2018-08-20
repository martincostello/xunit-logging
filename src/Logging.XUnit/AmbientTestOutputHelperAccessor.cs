// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Threading;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing an implementation of <see cref="ITestOutputHelperAccessor"/> that
    /// stores the <see cref="ITestOutputHelper"/> as an asynchronous local value. This class cannot be inherited.
    /// </summary>
    internal sealed class AmbientTestOutputHelperAccessor : ITestOutputHelperAccessor
    {
        /// <summary>
        /// A backing field for the <see cref="ITestOutputHelper"/> for the current thread.
        /// </summary>
        private static readonly AsyncLocal<ITestOutputHelper> _current = new AsyncLocal<ITestOutputHelper>();

#pragma warning disable CA1822
        /// <summary>
        /// Gets or sets the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public ITestOutputHelper OutputHelper
        {
            get { return _current.Value; }
            set { _current.Value = value; }
        }
#pragma warning restore CA1822
    }
}
