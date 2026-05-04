// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 21:44
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:00
// ***********************************************************************
//  <copyright file="DependencyInjection.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using RzR.Web.Middleware.ResponseTime.Abstractions;
using RzR.Web.Middleware.ResponseTime.Configuration;
using RzR.Web.Middleware.ResponseTime.Helpers;
using RzR.Web.Middleware.ResponseTime.Middleware;

#if NET7_0_OR_GREATER
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using RzR.Web.Middleware.ResponseTime.Filters;
#endif

#endregion

namespace RzR.Web.Middleware.ResponseTime
{
    /// <summary>
    ///     MW Dependency Injection
    /// </summary>
    /// <remarks></remarks>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Register response time services using the default <see cref="ResponseTimeOptions" />.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <returns>The same <see cref="IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection AddResponseTime(this IServiceCollection services)
            => services.AddResponseTime(configure: null);

        /// <summary>
        ///     Register response time services and configure <see cref="ResponseTimeOptions" />.
        /// </summary>
        /// <param name="services">Service collection.</param>
        /// <param name="configure">
        ///     Optional callback used to mutate the registered <see cref="ResponseTimeOptions" />
        ///     instance. Pass <c>null</c> (or use the parameterless overload) to keep defaults.
        /// </param>
        /// <returns>The same <see cref="IServiceCollection" /> for chaining.</returns>
        public static IServiceCollection AddResponseTime(
            this IServiceCollection services,
            Action<ResponseTimeOptions> configure)
        {
            services.AddOptions();

            if (configure != null)
                services.Configure(configure);

            services.AddScoped<IMWResponseTimeStopWatch, MWResponseTimeStopWatch>();
            services.AddScoped<IActionResponseTimeStopWatch, ActionResponseTimeStopWatch>();

            return services;
        }

        /// <summary>
        ///     Add the <see cref="ResponseTimeMiddleware"/> to the request pipeline.
        /// </summary>
        /// <param name="app">Application builder.</param>
        /// <returns>The same <see cref="IApplicationBuilder" /> for chaining.</returns>
        public static IApplicationBuilder UseResponseTime(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseTimeMiddleware>();

            return app;
        }

        /// <inheritdoc cref="AddResponseTime(IServiceCollection)" />
        [Obsolete("Use AddResponseTime() instead. This method will be removed in a future major release.")]
        public static IServiceCollection RegisterResponseTimeServices(this IServiceCollection services)
            => services.AddResponseTime();

        /// <inheritdoc cref="AddResponseTime(IServiceCollection, Action{ResponseTimeOptions})" />
        [Obsolete("Use AddResponseTime(configure) instead. This method will be removed in a future major release.")]
        public static IServiceCollection RegisterResponseTimeServices(
            this IServiceCollection services, Action<ResponseTimeOptions> configure)
            => services.AddResponseTime(configure);

        /// <inheritdoc cref="UseResponseTime(IApplicationBuilder)" />
        [Obsolete("Use UseResponseTime() instead. This method will be removed in a future major release.")]
        public static IApplicationBuilder UseResponseTimeMiddleware(this IApplicationBuilder app)
            => app.UseResponseTime();

#if NET7_0_OR_GREATER

        /// <summary>
        ///     Attach the <see cref="ResponseTimeEndpointFilter"/> to a minimal API endpoint so its
        ///     execution time is exposed via the <c>X-Action-Response-Time</c> response header.
        /// </summary>
        /// <param name="builder">The route handler builder returned by <c>MapGet</c>, <c>MapPost</c>, etc.</param>
        /// <returns>The same <see cref="RouteHandlerBuilder"/> for chaining.</returns>
        /// <remarks>
        ///     <example>
        ///         <code>
        ///         app.MapGet("/weather", () =&gt; Results.Ok(...))
        ///            .WithResponseTime();
        ///         </code>
        ///     </example>
        /// </remarks>
        public static RouteHandlerBuilder WithResponseTime(this RouteHandlerBuilder builder)
            => builder.AddEndpointFilter<ResponseTimeEndpointFilter>();

        /// <summary>
        ///     Attach the <see cref="ResponseTimeEndpointFilter"/> to every endpoint in a route group.
        /// </summary>
        /// <param name="builder">The route group builder.</param>
        /// <returns>The same <see cref="RouteGroupBuilder"/> for chaining.</returns>
        /// <remarks>
        ///     <example>
        ///         <code>
        ///         var api = app.MapGroup("/api").WithResponseTime();
        ///         </code>
        ///     </example>
        /// </remarks>
        public static RouteGroupBuilder WithResponseTime(this RouteGroupBuilder builder)
            => builder.AddEndpointFilter<ResponseTimeEndpointFilter>();

#endif
    }
}