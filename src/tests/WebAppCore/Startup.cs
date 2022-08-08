// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebAppCore
//  Author           : RzR
//  Created On       : 2022-08-07 12:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:01
// ***********************************************************************
//  <copyright file="Startup.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using XResponseTimeMW;

#endregion

namespace WebAppCore
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddRazorPages();

            services.RegisterResponseTimeServices();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();
            else
                app.UseExceptionHandler("/Error");

            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.UseResponseTimeMiddleware();

            app.UseEndpoints(endpoints => { endpoints.MapRazorPages(); });
        }
    }
}