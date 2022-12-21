// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebApi
//  Author           : RzR
//  Created On       : 2022-08-07 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:00
// ***********************************************************************
//  <copyright file="WeatherForecastController.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using XResponseTimeMW.Filters;

#endregion

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries =
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        public WeatherForecastController()
        {
        }

        [HttpGet]
        [ResponseTime]
#pragma warning disable SCS0005 // Weak random number generator.
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
                .ToArray();
        }
    }
}