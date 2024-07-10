// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

//// Based on https://github.com/dotnet/runtime/blob/65067052e433eda400c5e7cc9f7b21c84640f901/src/libraries/System.Private.CoreLib/src/System/Diagnostics/CodeAnalysis/StringSyntaxAttribute.cs

#if NETSTANDARD

#pragma warning disable IDE0130
#pragma warning disable SA1600

namespace System.Diagnostics.CodeAnalysis;

[AttributeUsage(AttributeTargets.Parameter | AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
[ExcludeFromCodeCoverage]
internal sealed class StringSyntaxAttribute(string syntax, params object?[] arguments) : Attribute
{
    public const string DateTimeFormat = nameof(DateTimeFormat);

    public StringSyntaxAttribute(string syntax)
        : this(syntax, [])
    {
    }

    public string Syntax { get; } = syntax;

    public object?[] Arguments { get; } = arguments;
}
#endif
