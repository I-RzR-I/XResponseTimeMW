// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2026-05-03 21:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 23:27
// ***********************************************************************
//  <copyright file="ResponseTimeOptions.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Http;

#endregion

namespace RzR.Web.Middleware.ResponseTime.Configuration
{
    /// -------------------------------------------------------------------------------------------------
    /// <summary>
    ///     Configuration options for <c>XResponseTimeMW</c>. Bound through the standard
    ///     <see cref="Microsoft.Extensions.Options.IOptions{TOptions}" /> pattern.
    /// </summary>
    /// <remarks>
    ///     Defaults reproduce the historical behavior of the library byte-for-byte:
    ///     <c>X-Response-Time</c>/<c>X-Action-Response-Time</c> headers in integer milliseconds plus
    ///     a
    ///     W3C <c>Server-Timing</c> header. Set any header name to <c>null</c> or <see cref="string.Empty" />
    ///     to suppress that specific header without disabling the others.
    /// </remarks>
    /// =================================================================================================
    public sealed class ResponseTimeOptions
    {
        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Default name of the response header carrying the total request duration.
        /// </summary>
        /// =================================================================================================
        public const string DefaultTotalHeaderName = "X-Response-Time";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Default name of the response header carrying the action / endpoint duration.
        /// </summary>
        /// =================================================================================================
        public const string DefaultActionHeaderName = "X-Action-Response-Time";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Default <c>Server-Timing</c> metric name for the total request duration.
        /// </summary>
        /// =================================================================================================
        public const string DefaultServerTimingTotalMetric = "total";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Default <c>Server-Timing</c> metric name for the action / endpoint duration.
        /// </summary>
        /// =================================================================================================
        public const string DefaultServerTimingActionMetric = "action";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     (Immutable)
        ///     Default numeric format string used for the <c>X-*</c> millisecond values.
        ///     <c>"0"</c> keeps the legacy integer rendering (<c>"12ms"</c>).
        /// </summary>
        /// =================================================================================================
        public const string DefaultDurationFormat = "0";

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Header name used by <see cref="Middleware.ResponseTimeMiddleware" /> for the total
        ///     request duration. Set to <c>null</c> or empty to suppress this header.
        /// </summary>
        /// <value>
        ///     The total number of header name.
        /// </value>
        /// =================================================================================================
        public string TotalHeaderName { get; set; } = DefaultTotalHeaderName;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Header name used by the MVC action filter and the minimal-API endpoint filter for the
        ///     action duration. Set to <c>null</c> or empty to suppress this header.
        /// </summary>
        /// <value>
        ///     The name of the action header.
        /// </value>
        /// =================================================================================================
        public string ActionHeaderName { get; set; } = DefaultActionHeaderName;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     When <c>true</c> (default) the W3C <c>Server-Timing</c> header is emitted alongside the
        ///     legacy <c>X-*</c> headers.
        /// </summary>
        /// <value>
        ///     True if emit server timing, false if not.
        /// </value>
        /// =================================================================================================
        public bool EmitServerTiming { get; set; } = true;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Metric name used inside the <c>Server-Timing</c> header for the total request duration.
        /// </summary>
        /// <value>
        ///     The server timing total metric.
        /// </value>
        /// =================================================================================================
        public string ServerTimingTotalMetric { get; set; } = DefaultServerTimingTotalMetric;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Metric name used inside the <c>Server-Timing</c> header for the action duration.
        /// </summary>
        /// <value>
        ///     The server timing action metric.
        /// </value>
        /// =================================================================================================
        public string ServerTimingActionMetric { get; set; } = DefaultServerTimingActionMetric;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Numeric format string applied to the millisecond value rendered in the <c>X-*</c>
        ///     headers (always rendered using <see cref="System.Globalization.CultureInfo.InvariantCulture" />
        ///     ). The literal <c>"ms"</c> suffix is appended automatically.
        /// </summary>
        /// <value>
        ///     The duration format.
        /// </value>
        /// =================================================================================================
        public string DurationFormat { get; set; } = DefaultDurationFormat;

        /// -------------------------------------------------------------------------------------------------
        /// <summary>
        ///     Optional predicate used to skip timing for selected requests (e.g. health checks, static
        ///     assets). Return <c>false</c> to suppress all headers for the current request. A <c>null</c>
        ///     filter (default) means every request is timed.
        /// </summary>
        /// <value>
        ///     A function delegate that yields a bool.
        /// </value>
        /// =================================================================================================
        public Func<HttpContext, bool> Filter { get; set; }
    }
}