// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System.Diagnostics.CodeAnalysis;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing options to use for format xunit log messages.
/// </summary>
public class XUnitLogFormatterOptions
{
    /// <summary>
    /// Gets or sets a value indicating whether to include scopes.
    /// </summary>
    public bool IncludeScopes { get; set; }

    /// <summary>
    /// Gets or sets the <see cref="System.TimeProvider"/> to use.
    /// </summary>
    public TimeProvider TimeProvider { get; set; } = TimeProvider.System;

    /// <summary>
    /// Gets or sets format string used to format timestamp in logging messages.
    /// Defaults to <see langword="null"/>.
    /// </summary>
    [StringSyntax(StringSyntaxAttribute.DateTimeFormat)]
    public string? TimestampFormat { get; set; }

    /// <summary>
    /// Gets or sets a value indicating whether or not the UTC timezone should be used
    /// to format timestamps in logging messages. Defaults to <see langword="false"/>.
    /// </summary>
    public bool UseUtcTimestamp { get; set; }
}
