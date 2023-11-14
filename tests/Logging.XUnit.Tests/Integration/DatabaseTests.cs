// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit.Integration;

public class DatabaseTests(DatabaseFixture databaseFixture) : IClassFixture<DatabaseFixture>
{
    public DatabaseFixture DatabaseFixture { get; } = databaseFixture;

    [Fact]
    public void Run_Database_Test()
    {
        DatabaseFixture.ConnectionString.ShouldNotBeEmpty();
    }
}
