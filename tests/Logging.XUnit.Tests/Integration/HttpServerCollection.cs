// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

namespace MartinCostello.Logging.XUnit.Integration;

/// <summary>
/// A class representing the collection fixture for an HTTP server. This class cannot be inherited.
/// </summary>
[CollectionDefinition(Name)]
public sealed class HttpServerCollection : ICollectionFixture<HttpServerFixture>
{
    /// <summary>
    /// The name of the test fixture.
    /// </summary>
    public const string Name = "HTTP server collection";
}
