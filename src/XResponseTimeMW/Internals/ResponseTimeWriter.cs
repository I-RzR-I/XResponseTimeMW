// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2026-05-03 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:40
// ***********************************************************************
//  <copyright file="ResponseTimeWriter.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using RzR.Web.Middleware.ResponseTime.Configuration;

#endregion

namespace RzR.Web.Middleware.ResponseTime.Internals
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Centralizes how a single duration is rendered onto the response, honoring
    ///     <see cref="ResponseTimeOptions" /> for header names, formatting and Server-Timing
    ///     emission.
    /// </summary>
    /// <remarks>
    ///     Used by the request middleware, the MVC action filter and the minimal-API endpoint filter
    ///     so that all three emit identical, configurable output.
    /// </remarks>
    /// =================================================================================================
    internal static class ResponseTimeWriter
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Resolve the effective <see cref="ResponseTimeOptions" /> for the current request, falling
        ///     back to the defaults if the options have not been registered.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>
        ///     The ResponseTimeOptions.
        /// </returns>
        /// =================================================================================================
        internal static ResponseTimeOptions ResolveOptions(HttpContext context)
        {
            var accessor = context?.RequestServices?.GetService<IOptions<ResponseTimeOptions>>();

            return accessor?.Value ?? new ResponseTimeOptions();
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Returns <c>true</c> when the configured <see cref="ResponseTimeOptions.Filter" />
        ///     opts the current request out of timing.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// <returns>
        ///     True if suppressed, false if not.
        /// </returns>
        /// =================================================================================================
        internal static bool IsSuppressed(HttpContext context, ResponseTimeOptions options)
        {
            return context != null && options?.Filter != null && !options.Filter(context);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Write the total-request timing onto the response according to <paramref name="options" />.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="elapsedMs">The elapsed in milliseconds.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        internal static void WriteTotal(HttpResponse response, long elapsedMs, ResponseTimeOptions options)
        {
            Write(response, elapsedMs, options?.TotalHeaderName, options?.ServerTimingTotalMetric, options);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Write the action / endpoint timing onto the response according to <paramref name="options" />
        ///     .
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="elapsedMs">The elapsed in milliseconds.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        internal static void WriteAction(HttpResponse response, long elapsedMs, ResponseTimeOptions options)
        {
            Write(response, elapsedMs, options?.ActionHeaderName, options?.ServerTimingActionMetric, options);
        }

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Writes.
        /// </summary>
        /// <param name="response">The response.</param>
        /// <param name="elapsedMs">The elapsed in milliseconds.</param>
        /// <param name="headerName">Name of the header.</param>
        /// <param name="serverTimingMetric">The server timing metric.</param>
        /// <param name="options">Options for controlling the operation.</param>
        /// =================================================================================================
        private static void Write(HttpResponse response, long elapsedMs, string headerName,
            string serverTimingMetric, ResponseTimeOptions options)
        {
            if (response == null || response.HasStarted || options == null)
                return;

            if (!string.IsNullOrEmpty(headerName))
            {
                var format = string.IsNullOrEmpty(options.DurationFormat)
                    ? ResponseTimeOptions.DefaultDurationFormat
                    : options.DurationFormat;

                response.Headers[headerName] =
                    elapsedMs.ToString(format, CultureInfo.InvariantCulture) + "ms";
            }

            if (options.EmitServerTiming && !string.IsNullOrEmpty(serverTimingMetric))
                ServerTimingHeader.Append(response, serverTimingMetric, elapsedMs);
        }
    }
}