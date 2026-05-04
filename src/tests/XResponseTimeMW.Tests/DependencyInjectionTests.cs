// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:20
// ***********************************************************************
//  <copyright file="DependencyInjectionTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RzR.Web.Middleware.ResponseTime;
using RzR.Web.Middleware.ResponseTime.Abstractions;
using RzR.Web.Middleware.ResponseTime.Configuration;

#endregion

namespace XResponseTimeMW.Tests
{
    [TestClass]
    public class DependencyInjectionTests
    {
        [TestMethod]
        public void AddResponseTime_Registers_Scoped_Stopwatch_Services_Test()
        {
            var services = new ServiceCollection();

            services.AddResponseTime();

            var mw = services.Single(d => d.ServiceType == typeof(IMWResponseTimeStopWatch));
            var action = services.Single(d => d.ServiceType == typeof(IActionResponseTimeStopWatch));
            Assert.AreEqual(ServiceLifetime.Scoped, mw.Lifetime);
            Assert.AreEqual(ServiceLifetime.Scoped, action.Lifetime);
        }

        [TestMethod]
        public void AddResponseTime_Resolves_Default_Options_When_No_Callback_Supplied_Test()
        {
            var services = new ServiceCollection();
            services.AddResponseTime();

            var sp = services.BuildServiceProvider();
            var options = sp.GetRequiredService<IOptions<ResponseTimeOptions>>().Value;

            Assert.AreEqual("X-Response-Time", options.TotalHeaderName);
        }

        [TestMethod]
        public void AddResponseTime_With_Configure_Applies_Callback_Test()
        {
            var services = new ServiceCollection();

            services.AddResponseTime(o =>
            {
                o.TotalHeaderName = "X-T";
                o.EmitServerTiming = false;
            });

            var options = services.BuildServiceProvider()
                .GetRequiredService<IOptions<ResponseTimeOptions>>().Value;
            Assert.AreEqual("X-T", options.TotalHeaderName);
            Assert.IsFalse(options.EmitServerTiming);
        }

        [TestMethod]
        public void WithResponseTime_Returns_Same_RouteHandlerBuilder_For_Chaining_Test()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddResponseTime();
            var app = builder.Build();

            var handler = app.MapGet("/x", () => "ok");
            var returned = handler.WithResponseTime();

            Assert.AreSame(handler, returned);
        }

        [TestMethod]
        public void WithResponseTime_Returns_Same_RouteGroupBuilder_Cor_Chaining_Test()
        {
            var builder = WebApplication.CreateBuilder();
            builder.Services.AddResponseTime();
            var app = builder.Build();

            var group = app.MapGroup("/api");
            var returned = group.WithResponseTime();

            Assert.AreSame(group, returned);
        }

        [TestMethod]
#pragma warning disable CS0618 // intentional — verifies the obsolete shim still works
        public void Obsolete_RegisterResponseTimeServices_Delegates_To_AddResponseTime_Test()
        {
            var services = new ServiceCollection();

            services.RegisterResponseTimeServices();

            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IMWResponseTimeStopWatch)));
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IActionResponseTimeStopWatch)));
            // Options infrastructure is wired (open generic IOptions<>).
            Assert.IsTrue(services.Any(d => d.ServiceType == typeof(IOptions<>)));
        }

        [TestMethod]
        public void Obsolete_RegisterResponseTimeServices_With_Configure_Delegates_To_AddResponseTime_Test()
        {
            var services = new ServiceCollection();

            services.RegisterResponseTimeServices(o => o.ActionHeaderName = "X-A");

            var options = services.BuildServiceProvider()
                .GetRequiredService<IOptions<ResponseTimeOptions>>().Value;
            Assert.AreEqual("X-A", options.ActionHeaderName);
        }

        [TestMethod]
        public void Obsolete_UseResponseTimeMiddleware_Still_Resolves_Test()
        {
            var services = new ServiceCollection();
            services.AddResponseTime();
            var sp = services.BuildServiceProvider();
            var app = new ApplicationBuilder(sp);

            var returned = app.UseResponseTimeMiddleware();

            Assert.AreSame(app, returned);
        }
#pragma warning restore CS0618
    }
}