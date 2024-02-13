// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit;

/// <summary>
/// A class representing an implementation of <see cref="IExternalScopeProvider"/> to xunit logging. This class cannot be inherited.
/// </summary>
internal sealed class XUnitLogScopeProvider : IExternalScopeProvider
{
    /// <inheritdoc />
    public void ForEachScope<TState>(Action<object?, TState> callback, TState state)
    {
        var current = XUnitLogScope.Current;

        var stack = new Stack<XUnitLogScope>();
        while (current != null)
        {
            stack.Push(current);
            current = current.Parent;
        }

        while (stack.Count > 0)
        {
            var element = stack.Pop();

            callback(element.State, state);
        }
    }

    /// <inheritdoc />
    public IDisposable Push(object? state)
        => XUnitLogScope.Push(state);
}
