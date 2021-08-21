// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;
using SampleApp;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit.Integration;

/// <summary>
/// A test fixture representing an HTTP server hosting the sample application. This class cannot be inherited.
/// </summary>
public sealed class HttpServerFixture : WebApplicationFactory<Startup>, ITestOutputHelperAccessor
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HttpServerFixture"/> class.
    /// </summary>
    public HttpServerFixture()
        : base()
    {
    }

    /// <inheritdoc />
    public ITestOutputHelper? OutputHelper { get; set; }

    /// <inheritdoc />
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureLogging((p) => p.AddXUnit(this));
    }
}
