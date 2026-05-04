// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 22:59
// ***********************************************************************
//  <copyright file="ResponseTimeAttributeTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RzR.Web.Middleware.ResponseTime;
using RzR.Web.Middleware.ResponseTime.Configuration;
using XResponseTimeMW.Tests.Controllers;

#endregion

namespace XResponseTimeMW.Tests.Filters
{
    [TestClass]
    public class ResponseTimeAttributeTests
    {
        private static async Task<(IHost Host, HttpClient Client)> StartAsync(
            Action<ResponseTimeOptions> configure = null)
        {
            var host = await new HostBuilder()
                .ConfigureWebHost(web =>
                {
                    web.UseTestServer()
                        .ConfigureServices(s =>
                        {
                            s.AddControllers().AddApplicationPart(typeof(PingController).Assembly);
                            if (configure == null)
                                s.AddResponseTime();
                            else 
                                s.AddResponseTime(configure);
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseEndpoints(e => e.MapControllers());
                        });
                })
                .StartAsync();

            return (host, host.GetTestClient());
        }

        [TestMethod]
        public async Task Attribute_Emits_Action_Header_And_Server_Timing_Test()
        {
            var (host, client) = await StartAsync();
            using var _ = host;

            var response = await client.GetAsync("/ping");

            Assert.IsTrue(response.Headers.Contains("X-Action-Response-Time"));
            var serverTiming = response.Headers.GetValues("Server-Timing").Single();
            StringAssert.StartsWith(serverTiming, "action;dur=");
        }

        [TestMethod]
        public async Task Endpoint_Without_Attribute_Does_Not_Emit_Action_Header_Test()
        {
            var (host, client) = await StartAsync();
            using var _ = host;

            var response = await client.GetAsync("/ping/plain");

            Assert.IsFalse(response.Headers.Contains("X-Action-Response-Time"));
        }

        [TestMethod]
        public async Task Filter_Suppresses_Action_Header_Test()
        {
            var (host, client) = await StartAsync(o => o.Filter = _ => false);
            using var _ = host;

            var response = await client.GetAsync("/ping");

            Assert.IsFalse(response.Headers.Contains("X-Action-Response-Time"));
            Assert.IsFalse(response.Headers.Contains("Server-Timing"));
        }
    }
}