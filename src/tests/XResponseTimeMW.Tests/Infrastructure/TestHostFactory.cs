// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 22:50
// ***********************************************************************
//  <copyright file="TestHostFactory.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.Hosting;
using RzR.Web.Middleware.ResponseTime;
using RzR.Web.Middleware.ResponseTime.Configuration;

#endregion

namespace XResponseTimeMW.Tests.Infrastructure
{
    internal static class TestHostFactory
    {
        public static async Task<(IHost Host, HttpClient Client)> CreateMiddlewareHostAsync(
            Action<ResponseTimeOptions> configure = null,
            RequestDelegate handler = null)
        {
            var host = await new HostBuilder()
                .ConfigureWebHost(web =>
                {
                    web.UseTestServer()
                        .ConfigureServices(s =>
                        {
                            if (configure == null)
                                s.AddResponseTime();
                            else
                                s.AddResponseTime(configure);
                        })
                        .Configure(app =>
                        {
                            app.UseResponseTime();

                            app.Run(handler ?? (ctx =>
                            {
                                ctx.Response.StatusCode = StatusCodes.Status200OK;

                                return ctx.Response.WriteAsync("ok");
                            }));
                        });
                })
                .StartAsync();

            return (host, host.GetTestClient());
        }
    }
}