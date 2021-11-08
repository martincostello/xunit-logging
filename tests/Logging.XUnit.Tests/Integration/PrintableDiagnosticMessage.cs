// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Xunit.Sdk;

namespace MartinCostello.Logging.XUnit.Integration;

/// <summary>
/// See https://github.com/xunit/xunit/pull/2148#issuecomment-839838421.
/// </summary>
internal class PrintableDiagnosticMessage : DiagnosticMessage
{
    public PrintableDiagnosticMessage(string message)
        : base(message)
    {
    }

    public override string ToString() => Message;
}
