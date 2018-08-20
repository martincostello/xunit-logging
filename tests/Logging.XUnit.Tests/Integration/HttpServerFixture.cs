// Copyright (c) Martin Costello, 2018. All rights reserved.
// Licensed under the Apache 2.0 license. See the LICENSE file in the project root for full license information.

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using SampleApp;
using Xunit.Abstractions;

namespace MartinCostello.Logging.XUnit.Integration
{
    /// <summary>
    /// A test fixture representing an HTTP server hosting the sample application. This class cannot be inherited.
    /// </summary>
    public sealed class HttpServerFixture : WebApplicationFactory<Startup>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpServerFixture"/> class.
        /// </summary>
        public HttpServerFixture()
            : base()
        {
            // HACK Force HTTP server startup
            using (CreateDefaultClient())
            {
            }
        }

        /// <summary>
        /// Clears the current <see cref="ITestOutputHelper"/>.
        /// </summary>
        public void ClearOutputHelper()
        {
            Server.Host.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = null;
        }

        /// <summary>
        /// Sets the <see cref="ITestOutputHelper"/> to use.
        /// </summary>
        /// <param name="value">The <see cref="ITestOutputHelper"/> to use.</param>
        public void SetOutputHelper(ITestOutputHelper value)
        {
            Server.Host.Services.GetRequiredService<ITestOutputHelperAccessor>().OutputHelper = value;
        }

        /// <inheritdoc />
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureLogging((p) => p.AddXUnit());
        }
    }
}
