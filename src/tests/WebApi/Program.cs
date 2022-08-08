// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebApi
//  Author           : RzR
//  Created On       : 2022-08-07 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:00
// ***********************************************************************
//  <copyright file="Program.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;

#endregion

namespace WebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<Startup>(); });
        }
    }
}