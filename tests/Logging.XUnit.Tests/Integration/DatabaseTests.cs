// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit.Integration;

public class DatabaseTests : IClassFixture<DatabaseFixture>
{
    public DatabaseTests(DatabaseFixture databaseFixture)
    {
        DatabaseFixture = databaseFixture;
    }

    public DatabaseFixture DatabaseFixture { get; }

    [Fact]
    public void Run_Database_Test()
    {
        DatabaseFixture.ConnectionString.ShouldNotBeEmpty();
    }
}
