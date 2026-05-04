// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:17
// ***********************************************************************
//  <copyright file="ResponseTimeMiddlewareTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Http;
using XResponseTimeMW.Tests.Infrastructure;

#endregion

namespace XResponseTimeMW.Tests.Middleware
{
    [TestClass]
    public class ResponseTimeMiddlewareTests
    {
        [TestMethod]
        public async Task Emits_Default_X_Response_Time_and_ServerTiming_Test()
        {
            var (host, client) = await TestHostFactory.CreateMiddlewareHostAsync();
            using var _ = host;

            var response = await client.GetAsync("/");

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.IsTrue(response.Headers.Contains("X-Response-Time"));

            var serverTiming = response.Headers.GetValues("Server-Timing").Single();
            StringAssert.StartsWith(serverTiming, "total;dur=");
        }

        [TestMethod]
        public async Task Honors_Custom_Total_Header_Name_Test()
        {
            var (host, client) = await TestHostFactory.CreateMiddlewareHostAsync(o => o.TotalHeaderName = "X-Total");
            using var _ = host;

            var response = await client.GetAsync("/");

            Assert.IsTrue(response.Headers.Contains("X-Total"));
            Assert.IsFalse(response.Headers.Contains("X-Response-Time"));
        }

        [TestMethod]
        public async Task Suppresses_All_Headers_When_Filter_Returns_False_Test()
        {
            var (host, client) = await TestHostFactory.CreateMiddlewareHostAsync(o =>
                o.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/health"));
            using var _ = host;

            var response = await client.GetAsync("/health");

            Assert.IsFalse(response.Headers.Contains("X-Response-Time"));
            Assert.IsFalse(response.Headers.Contains("Server-Timing"));
        }

        [TestMethod]
        public async Task EmitServerTiming_False_Keeps_Only_X_Header_Test()
        {
            var (host, client) = await TestHostFactory.CreateMiddlewareHostAsync(o => o.EmitServerTiming = false);
            using var _ = host;

            var response = await client.GetAsync("/");

            Assert.IsTrue(response.Headers.Contains("X-Response-Time"));
            Assert.IsFalse(response.Headers.Contains("Server-Timing"));
        }

        [TestMethod]
        public async Task Concurrent_Requests_Each_Get_Their_Own_Header_Value_Test()
        {
            var (host, client) = await TestHostFactory.CreateMiddlewareHostAsync(handler: async ctx =>
            {
                await Task.Delay(20);
                await ctx.Response.WriteAsync("ok");
            });
            using var _ = host;

            var responses = await Task.WhenAll(Enumerable.Range(0, 10).Select(_ => client.GetAsync("/")));

            foreach (var r in responses)
            {
                Assert.IsTrue(r.Headers.Contains("X-Response-Time"));

                var raw = r.Headers.GetValues("X-Response-Time").Single();
                Assert.IsTrue(raw.EndsWith("ms"), $"Header value '{raw}' should end with 'ms'.");

                var ms = long.Parse(raw.AsSpan(0, raw.Length - 2));
                Assert.IsTrue(ms >= 0, $"Negative elapsed value '{ms}' indicates a shared stopwatch.");
            }
        }
    }
}