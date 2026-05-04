// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:09
// ***********************************************************************
//  <copyright file="ResponseTimeWriterTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.IO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using RzR.Web.Middleware.ResponseTime.Configuration;
using RzR.Web.Middleware.ResponseTime.Internals;

#endregion

namespace XResponseTimeMW.Tests.Internals
{
    [TestClass]
    public class ResponseTimeWriterTests
    {
        private static HttpContext NewContextWithOptions(ResponseTimeOptions options)
        {
            var services = new ServiceCollection();
            services.AddOptions();
            services.Configure<ResponseTimeOptions>(o =>
            {
                o.TotalHeaderName = options.TotalHeaderName;
                o.ActionHeaderName = options.ActionHeaderName;
                o.EmitServerTiming = options.EmitServerTiming;
                o.ServerTimingTotalMetric = options.ServerTimingTotalMetric;
                o.ServerTimingActionMetric = options.ServerTimingActionMetric;
                o.DurationFormat = options.DurationFormat;
                o.Filter = options.Filter;
            });

            var ctx = new DefaultHttpContext
            {
                RequestServices = services.BuildServiceProvider()
            };

            return ctx;
        }

        [TestMethod]
        public void ResolveOptions_Returns_Defaults_When_Options_Not_Registered_Test()
        {
            var ctx = new DefaultHttpContext
            {
                RequestServices = new ServiceCollection().BuildServiceProvider()
            };

            var resolved = ResponseTimeWriter.ResolveOptions(ctx);

            Assert.IsNotNull(resolved);
            Assert.AreEqual(ResponseTimeOptions.DefaultTotalHeaderName, resolved.TotalHeaderName);
            Assert.AreEqual(ResponseTimeOptions.DefaultActionHeaderName, resolved.ActionHeaderName);
            Assert.IsTrue(resolved.EmitServerTiming);
        }

        [TestMethod]
        public void ResolveOptions_Returns_Configured_Isnstance_Test()
        {
            var ctx = NewContextWithOptions(new ResponseTimeOptions { TotalHeaderName = "X-Total" });

            var resolved = ResponseTimeWriter.ResolveOptions(ctx);

            Assert.AreEqual("X-Total", resolved.TotalHeaderName);
        }

        [TestMethod]
        public void IsSuppressed_Returns_True_When_Filter_Returns_False_Test()
        {
            var ctx = new DefaultHttpContext();
            var options = new ResponseTimeOptions { Filter = _ => false };

            Assert.IsTrue(ResponseTimeWriter.IsSuppressed(ctx, options));
        }

        [TestMethod]
        public void IsSuppressed_Returns_False_When_No_Filter_Configured_Test()
        {
            var ctx = new DefaultHttpContext();
            var options = new ResponseTimeOptions();

            Assert.IsFalse(ResponseTimeWriter.IsSuppressed(ctx, options));
        }

        [TestMethod]
        public void WriteTotal_Writes_Default_Header_And_Server_Timing_Test()
        {
            var ctx = new DefaultHttpContext();
            var options = new ResponseTimeOptions();

            ResponseTimeWriter.WriteTotal(ctx.Response, 42, options);

            Assert.AreEqual("42ms", ctx.Response.Headers["X-Response-Time"].ToString());
            Assert.AreEqual("total;dur=42.0", ctx.Response.Headers["Server-Timing"].ToString());
        }

        [TestMethod]
        public void WriteAction_Uses_Custom_Header_And_Format_Test()
        {
            var ctx = new DefaultHttpContext();
            var options = new ResponseTimeOptions
            {
                ActionHeaderName = "X-Custom-Action",
                DurationFormat = "0.00",
                EmitServerTiming = false
            };

            ResponseTimeWriter.WriteAction(ctx.Response, 9, options);

            Assert.AreEqual("9.00ms", ctx.Response.Headers["X-Custom-Action"].ToString());
            Assert.IsFalse(ctx.Response.Headers.ContainsKey("Server-Timing"),
                "Server-Timing must be suppressed when EmitServerTiming is false.");
        }

        [TestMethod]
        public void WriteTotal_Skips_X_Header_When_Name_Is_Null_Or_Empty_But_Keeps_Server_Timing_Test()
        {
            var ctx = new DefaultHttpContext();
            var options = new ResponseTimeOptions { TotalHeaderName = null };

            ResponseTimeWriter.WriteTotal(ctx.Response, 5, options);

            Assert.IsFalse(ctx.Response.Headers.ContainsKey("X-Response-Time"));
            Assert.AreEqual("total;dur=5.0", ctx.Response.Headers["Server-Timing"].ToString());
        }

        [TestMethod]
        public void Write_Is_Noop_When_Response_Already_Started_Test()
        {
            var ctx = new DefaultHttpContext();
            var fakeBody = new MemoryStream();
            ctx.Response.Body = fakeBody;

            ResponseTimeWriter.WriteTotal(null, 1, new ResponseTimeOptions());
        }
    }
}