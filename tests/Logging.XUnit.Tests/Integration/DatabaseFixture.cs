// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit.Integration;

public sealed class DatabaseFixture : IAsyncLifetime
{
    private readonly ILogger _initializeLogger;
    private readonly ILogger _disposeLogger;
    private string? _connectionString;

    public DatabaseFixture(IMessageSink messageSink)
    {
        using var loggerFactory = new LoggerFactory();
        _initializeLogger = loggerFactory.AddXUnit(messageSink, c => c.MessageSinkMessageFactory = m => new PrintableDiagnosticMessage(m)).CreateLogger<DatabaseFixture>();
        _disposeLogger = messageSink.ToLogger<DatabaseFixture>();
    }

    public string ConnectionString => _connectionString ?? throw new InvalidOperationException("The connection string is only available after InitializeAsync has completed.");

    Task IAsyncLifetime.InitializeAsync()
    {
        _initializeLogger.LogInformation("Initializing database");
        _connectionString = "Server=localhost";
        return Task.CompletedTask;
    }

    Task IAsyncLifetime.DisposeAsync()
    {
        _disposeLogger.LogInformation("Disposing database");
        return Task.CompletedTask;
    }
}
