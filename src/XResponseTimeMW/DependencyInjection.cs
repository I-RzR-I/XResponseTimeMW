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

using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using XResponseTimeMW.Abstractions;
using XResponseTimeMW.Helpers;
using XResponseTimeMW.Middleware;

#endregion

namespace XResponseTimeMW
{
    /// <summary>
    ///     MW Dependency Injection
    /// </summary>
    /// <remarks></remarks>
    public static class DependencyInjection
    {
        /// <summary>
        ///     Register time watch services
        /// </summary>
        /// <param name="services">Service collection</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IServiceCollection RegisterResponseTimeServices(this IServiceCollection services)
        {
            services.AddSingleton<IMWResponseTimeStopWatch, MWResponseTimeStopWatch>();
            services.AddSingleton<IActionResponseTimeStopWatch, ActionResponseTimeStopWatch>();

            return services;
        }

        /// <summary>
        ///     Use response time middleware
        /// </summary>
        /// <param name="app">Application builder</param>
        /// <returns></returns>
        /// <remarks></remarks>
        public static IApplicationBuilder UseResponseTimeMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ResponseTimeMiddleware>();

            return app.UseMiddleware<ResponseTimeNowMiddleware>();
        }
    }
}