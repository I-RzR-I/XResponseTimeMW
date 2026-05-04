// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:08
// ***********************************************************************
//  <copyright file="ResponseTimeEndpointFilterTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using RzR.Web.Middleware.ResponseTime;
using RzR.Web.Middleware.ResponseTime.Configuration;

#endregion

namespace XResponseTimeMW.Tests.Filters
{
    [TestClass]
    public class ResponseTimeEndpointFilterTests
    {
        private static async Task<WebApplication> BuildAsync(Action<ResponseTimeOptions> configure = null,
            bool registerServices = true)
        {
            var builder = WebApplication.CreateBuilder();
            builder.WebHost.UseTestServer();
            if (registerServices)
            {
                if (configure == null) builder.Services.AddResponseTime();
                else builder.Services.AddResponseTime(configure);
            }

            var app = builder.Build();
            app.MapGet("/timed", () => Results.Ok("ok")).WithResponseTime();
            app.MapGet("/normal", () => Results.Ok("ok"));

            var group = app.MapGroup("/api").WithResponseTime();
            group.MapGet("/in", () => Results.Ok("ok"));

            await app.StartAsync();
            return app;
        }

        [TestMethod]
        public async Task Endpoint_With_Filter_Emits_Action_Header_Test()
        {
            await using var app = await BuildAsync();
            var client = app.GetTestClient();

            var response = await client.GetAsync("/timed");

            Assert.IsTrue(response.Headers.Contains("X-Action-Response-Time"));
            StringAssert.StartsWith(response.Headers.GetValues("Server-Timing").Single(), "action;dur=");
        }

        [TestMethod]
        public async Task Endpoint_Without_Filter_Does_Not_Emit_Action_Header_Test()
        {
            await using var app = await BuildAsync();
            var client = app.GetTestClient();

            var response = await client.GetAsync("/normal");

            Assert.IsFalse(response.Headers.Contains("X-Action-Response-Time"));
        }

        [TestMethod]
        public async Task Group_Filter_Applies_To_Every_Endpoint_In_Group_Test()
        {
            await using var app = await BuildAsync();
            var client = app.GetTestClient();

            var response = await client.GetAsync("/api/in");

            Assert.IsTrue(response.Headers.Contains("X-Action-Response-Time"));
        }

        [TestMethod]
        public async Task Endpoint_Filter_Works_Without_AddResponseTime_Via_Default_Options_Test()
        {
            await using var app = await BuildAsync(registerServices: false);
            var client = app.GetTestClient();

            var response = await client.GetAsync("/timed");

            Assert.IsTrue(response.Headers.Contains("X-Action-Response-Time"));
        }

        [TestMethod]
        public async Task Filter_predicate_suppresses_action_header_Test()
        {
            await using var app = await BuildAsync(o => o.Filter = _ => false);
            var client = app.GetTestClient();

            var response = await client.GetAsync("/timed");

            Assert.IsFalse(response.Headers.Contains("X-Action-Response-Time"));
            Assert.IsFalse(response.Headers.Contains("Server-Timing"));
        }
    }
}