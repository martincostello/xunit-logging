// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using System;
using System.Net.Http;
using System.Threading.Tasks;
using Shouldly;
using Xunit;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit.Integration
{
    [Collection(HttpServerCollection.Name)]
    public sealed class HttpApplicationTests : IDisposable
    {
        public HttpApplicationTests(HttpServerFixture fixture, ITestOutputHelper outputHelper)
        {
            Fixture = fixture;
            Fixture.OutputHelper = outputHelper;
        }

        private HttpServerFixture Fixture { get; }

        public void Dispose()
        {
            Fixture.OutputHelper = null;
        }

        [Fact]
        public async Task Http_Get_Many()
        {
            // Arrange
            using (var httpClient = Fixture.CreateClient())
            {
                // Act
                using (var response = await httpClient.GetAsync("api/values"))
                {
                    // Assert
                    response.IsSuccessStatusCode.ShouldBeTrue();
                }
            }
        }

        [Fact]
        public async Task Http_Get_Single()
        {
            // Arrange
            using (var httpClient = Fixture.CreateClient())
            {
                // Act
                using (var response = await httpClient.GetAsync("api/values/a"))
                {
                    // Assert
                    response.IsSuccessStatusCode.ShouldBeTrue();
                }
            }
        }

        [Fact]
        public async Task Http_Post()
        {
            // Arrange
            using (var httpClient = Fixture.CreateClient())
            {
                // Act
                using (var response = await httpClient.PostAsJsonAsync("api/values", "d"))
                {
                    // Assert
                    response.IsSuccessStatusCode.ShouldBeTrue();
                }
            }
        }

        [Fact]
        public async Task Http_Put()
        {
            // Arrange
            using (var httpClient = Fixture.CreateClient())
            {
                // Act
                using (var response = await httpClient.PutAsJsonAsync("api/values/d", "d"))
                {
                    // Assert
                    response.IsSuccessStatusCode.ShouldBeTrue();
                }
            }
        }

        [Fact]
        public async Task Http_Delete()
        {
            // Arrange
            using (var httpClient = Fixture.CreateClient())
            {
                // Act
                using (var response = await httpClient.DeleteAsync("api/values/d"))
                {
                    // Assert
                    response.IsSuccessStatusCode.ShouldBeTrue();
                }
            }
        }
    }
}
