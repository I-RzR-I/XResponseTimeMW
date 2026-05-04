#if NET7_0_OR_GREATER

// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2026-05-03 20:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 20:35
// ***********************************************************************
//  <copyright file="ResponseTimeEndpointFilter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using RzR.Web.Middleware.ResponseTime.Internals;

#endregion

namespace RzR.Web.Middleware.ResponseTime.Filters
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Endpoint filter that measures the execution time of a minimal API endpoint and writes it
    ///     to the <c>X-Action-Response-Time</c> response header.
    /// </summary>
    /// <remarks>
    ///     Mirrors the behavior of the MVC <see cref="ResponseTime"/> action filter for route
    ///     handlers registered through endpoint routing (e.g. <c>app.MapGet(...)</c>). 
    ///     Uses a per-invocation <see cref="Stopwatch"/> so it is safe under concurrent requests
    ///     regardless of how the response time services are registered.
    /// </remarks>
    /// =================================================================================================
    public sealed class ResponseTimeEndpointFilter : IEndpointFilter
    {
        /// <inheritdoc/>
        public async ValueTask<object> InvokeAsync(EndpointFilterInvocationContext context, EndpointFilterDelegate next)
        {
            if (context == null)
                throw new ArgumentNullException(nameof(context));
            if (next == null)
                throw new ArgumentNullException(nameof(next));

            var options = ResponseTimeWriter.ResolveOptions(context.HttpContext);
            if (ResponseTimeWriter.IsSuppressed(context.HttpContext, options))
                return await next(context).ConfigureAwait(false);

            var watch = Stopwatch.StartNew();

            // Register the header write on response start so it is emitted even when the
            // endpoint short-circuits via Results.Stream / Results.File / SSE etc.
            context.HttpContext.Response.OnStarting(state =>
            {
                watch.Stop();
                var http = (HttpContext)state;
                ResponseTimeWriter.WriteAction(http.Response, watch.ElapsedMilliseconds, options);

                return Task.CompletedTask;
            }, context.HttpContext);

            return await next(context).ConfigureAwait(false);
        }
    }
}

#endif