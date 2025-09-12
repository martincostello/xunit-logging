// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD
using System.Runtime.CompilerServices;

#pragma warning disable IDE0130
namespace System;

/// <summary>
/// Extension methods for <see cref="ArgumentNullException"/>.
/// </summary>
internal static class ArgumentNullExceptionExtensions
{
    extension(ArgumentNullException)
    {
        /// <summary>Throws an <see cref="ArgumentNullException"/> if <paramref name="argument"/> is null.</summary>
        /// <param name="argument">The reference type argument to validate as non-null.</param>
        /// <param name="paramName">The name of the parameter with which <paramref name="argument"/> corresponds.</param>
        public static void ThrowIfNull(object? argument, [CallerArgumentExpression(nameof(argument))] string? paramName = null)
        {
            if (argument is null)
            {
                throw new ArgumentNullException(paramName);
            }
        }
    }
}
#endif
