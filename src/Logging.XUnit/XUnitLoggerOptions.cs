// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit
{
    /// <summary>
    /// A class representing configuration options for logging to xunit.
    /// </summary>
    public class XUnitLoggerOptions
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="XUnitLoggerOptions"/> class.
        /// </summary>
        public XUnitLoggerOptions()
        {
        }

        /// <summary>
        /// Gets or sets the category filter to apply to logs.
        /// </summary>
        public Func<string?, LogLevel, bool> Filter { get; set; } = (c, l) => true; // By default log everything

        /// <summary>
        /// Gets or sets a value indicating whether to include scopes.
        /// </summary>
        public bool IncludeScopes { get; set; }
    }
}
