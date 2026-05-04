// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 22:09
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 12:53
// ***********************************************************************
//  <copyright file="ResponseTime.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;
using RzR.Web.Middleware.ResponseTime.Abstractions;
using RzR.Web.Middleware.ResponseTime.Internals;

#endregion

namespace RzR.Web.Middleware.ResponseTime.Filters
{
    /// <summary>
    ///     Response time attribute
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResponseTimeAttribute : Attribute, IActionFilter
    {
        /// <inheritdoc />
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var options = ResponseTimeWriter.ResolveOptions(context.HttpContext);
            if (ResponseTimeWriter.IsSuppressed(context.HttpContext, options))
                return;

            IStopWatch watch = GetWatch(context.HttpContext);
            watch.Reset();
            watch.Start();
        }

        /// <inheritdoc />
        public void OnActionExecuted(ActionExecutedContext context)
        {
            var options = ResponseTimeWriter.ResolveOptions(context.HttpContext);
            if (ResponseTimeWriter.IsSuppressed(context.HttpContext, options))
                return;

            IStopWatch watch = GetWatch(context.HttpContext);
            watch.Stop();

            ResponseTimeWriter.WriteAction(context.HttpContext.Response, watch.ElapsedMilliseconds, options);
        }

        /// <summary>
        ///     Get watch timer
        /// </summary>
        /// <param name="context">Current HTTP context</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IActionResponseTimeStopWatch GetWatch(HttpContext context) =>
            context.RequestServices.GetRequiredService<IActionResponseTimeStopWatch>();
    }
}