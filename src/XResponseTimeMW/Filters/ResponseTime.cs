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
using XResponseTimeMW.Abstractions;

#endregion

namespace XResponseTimeMW.Filters
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class ResponseTime : Attribute, IActionFilter
    {
        /// <inheritdoc />
        public void OnActionExecuting(ActionExecutingContext context)
        {
            IStopWatch watch = GetWatch(context.HttpContext);
            watch.Reset();
            watch.Start();
        }

        /// <inheritdoc />
        public void OnActionExecuted(ActionExecutedContext context)
        {
            IStopWatch watch = GetWatch(context.HttpContext);
            watch.Stop();

            context.HttpContext.Response.Headers["X-Action-Response-Time"] = $"{watch.ElapsedMilliseconds}ms";
        }

        /// <summary>
        ///     Get watch timer
        /// </summary>
        /// <param name="context">Current HTTP context</param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static IActionResponseTimeStopWatch GetWatch(HttpContext context)
        {
            return context.RequestServices.GetService<IActionResponseTimeStopWatch>();
        }
    }
}