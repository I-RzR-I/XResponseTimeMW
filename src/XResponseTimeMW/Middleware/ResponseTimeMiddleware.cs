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
using Microsoft.Extensions.Options;
using RzR.Web.Middleware.ResponseTime.Abstractions;
using RzR.Web.Middleware.ResponseTime.Configuration;
using RzR.Web.Middleware.ResponseTime.Internals;

// ReSharper disable ClassNeverInstantiated.Global

#endregion

namespace RzR.Web.Middleware.ResponseTime.Middleware
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Response time middleware.
    /// </summary>
    /// =================================================================================================
    public class ResponseTimeMiddleware
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Request delegate.
        /// </summary>
        /// =================================================================================================
        private readonly RequestDelegate _next;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable) options for controlling the operation.
        /// </summary>
        /// =================================================================================================
        private readonly ResponseTimeOptions _options;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Initializes a new instance of the <see cref="ResponseTimeMiddleware" /> class.
        /// </summary>
        /// <param name="next">The next request delegate in the pipeline.</param>
        /// <param name="options">
        ///     (Optional)
        ///     Options accessor. Falls back to <see cref="ResponseTimeOptions"/> defaults when the
        ///     accessor is not provided (e.g. options not registered).
        /// </param>
        /// =================================================================================================
        public ResponseTimeMiddleware(RequestDelegate next, IOptions<ResponseTimeOptions> options = null)
        {
            _next = next;
            _options = options?.Value ?? new ResponseTimeOptions();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Invoke.
        /// </summary>
        /// <param name="context">Current HTTP context.</param>
        /// <param name="watch">Time watcher.</param>
        /// <returns>
        ///     A Task.
        /// </returns>
        /// =================================================================================================
        public async Task Invoke(HttpContext context, IMWResponseTimeStopWatch watch)
        {
            if (ResponseTimeWriter.IsSuppressed(context, _options))
            {
                await _next(context);

                return;
            }

            watch.Start();

            context.Response.OnStarting(state =>
            {
                watch.Stop();
                var http = (HttpContext)state;
                ResponseTimeWriter.WriteTotal(http.Response, watch.ElapsedMilliseconds, _options);

                return Task.CompletedTask;
            }, context);

            await _next(context);
        }
    }
}