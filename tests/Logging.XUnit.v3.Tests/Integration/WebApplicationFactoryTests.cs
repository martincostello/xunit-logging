// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Logging;

namespace MartinCostello.Logging.XUnit.Integration;

public sealed class WebApplicationFactoryTests
{
    [Fact]
    public async Task Http_Get_Many()
    {
        // Arrange
        using var fixture = new WebApplicationFactoryFixture();
        using var httpClient = fixture.CreateClient();

        // Act
        using var response = await httpClient.GetAsync("api/values", TestContext.Current.CancellationToken);

        // Assert
        response.IsSuccessStatusCode.ShouldBeTrue();
    }

    private sealed class WebApplicationFactoryFixture : WebApplicationFactory<SampleApp.Program>
    {
        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
            => builder.ConfigureLogging((p) => p.AddXUnit(TestContext.Current.TestOutputHelper!));
    }
}
