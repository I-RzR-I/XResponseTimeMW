// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 21:54
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 12:58
// ***********************************************************************
//  <copyright file="ResponseTimeMiddleware.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using XResponseTimeMW.Abstractions;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace XResponseTimeMW.Middleware
{
    /// <summary>
    ///     Response time middleware
    /// </summary>
    /// <remarks></remarks>
    public class ResponseTimeMiddleware
    {
        /// <summary>
        ///     Request delegate
        /// </summary>
        /// <remarks></remarks>
        private readonly RequestDelegate _next;

        /// <summary>
        ///     Initializes a new instance of the <see cref="XResponseTimeMW.Middleware.ResponseTimeMiddleware" /> class.
        /// </summary>
        /// <param name="next"></param>
        /// <remarks></remarks>
        public ResponseTimeMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        /// <summary>
        ///     Invoke
        /// </summary>
        /// <param name="context">Current HTTP context</param>
        /// <param name="watch">Time watcher</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public async Task Invoke(HttpContext context, IMWResponseTimeStopWatch watch)
        {
            watch.Start();

            context.Response.OnStarting(state =>
            {
                watch.Stop();
                context.Response.Headers["X-Response-Time"] = $"{watch.ElapsedMilliseconds}ms";

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}