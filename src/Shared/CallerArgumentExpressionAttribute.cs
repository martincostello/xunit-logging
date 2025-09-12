// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

#if NETFRAMEWORK || NETSTANDARD
#pragma warning disable IDE0130
namespace System.Runtime.CompilerServices;

/// <summary>
/// Indicates that a parameter captures the expression passed for another parameter as a string.
/// </summary>
/// <param name="parameterName">The name of the parameter whose expression should be captured as a string.</param>
[AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false, Inherited = false)]
internal sealed class CallerArgumentExpressionAttribute(string parameterName) : Attribute
{
    /// <summary>
    /// Gets the name of the parameter whose expression should be captured as a string.
    /// </summary>
    public string ParameterName { get; } = parameterName;
}
#endif
