// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2026-05-03 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 21:35
// ***********************************************************************
//  <copyright file="ServerTimingHeader.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;

#endregion

namespace RzR.Web.Middleware.ResponseTime.Internals
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Helpers for emitting the W3C <c>Server-Timing</c> response header.
    /// </summary>
    /// <remarks>
    ///     Spec: https://www.w3.org/TR/server-timing/. <br />
    ///     A <c>Server-Timing</c> response header is a comma-separated list of metrics, each of
    ///     which has the form <c>name;dur=&lt;ms&gt;[;desc=&lt;text&gt;]</c>. Multiple components on
    ///     the same request may contribute their own metric entry. This helper appends a metric
    ///     without overwriting metrics already present on the response.
    /// </remarks>
    /// =================================================================================================
    internal static class ServerTimingHeader
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     The HTTP response header name as defined by the W3C Server-Timing specification.
        /// </summary>
        /// =================================================================================================
        internal const string Name = "Server-Timing";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Append a single metric entry to the <c>Server-Timing</c> response header.
        /// </summary>
        /// <param name="response">
        ///     Response to write to. No-op if <c>null</c> or if the response has already started.
        /// </param>
        /// <param name="metricName">
        ///     Short metric token (e.g. <c>total</c>, <c>action</c>). Must be a valid HTTP token; the
        ///     caller is responsible for using only spec-allowed characters (no whitespace, comma or
        ///     semicolon).
        /// </param>
        /// <param name="elapsedMs">
        ///     Duration in milliseconds. Emitted with one decimal place so sub-ms timings are preserved.
        /// </param>
        /// <param name="description">
        ///     (Optional) Optional human-readable description; quoted per spec when present.
        /// </param>
        /// =================================================================================================
        internal static void Append(HttpResponse response, string metricName, double elapsedMs,
            string description = null)
        {
            if (response == null || response.HasStarted)
                return;

            var entry = string.IsNullOrEmpty(description)
                ? string.Format(CultureInfo.InvariantCulture, "{0};dur={1:F1}", metricName, elapsedMs)
                : string.Format(CultureInfo.InvariantCulture, "{0};dur={1:F1};desc=\"{2}\"", metricName, elapsedMs,
                    description);

            if (response.Headers.TryGetValue(Name, out var existing))
                response.Headers[Name] = StringValues.Concat(existing, entry);
            else
                response.Headers[Name] = entry;
        }
    }
}