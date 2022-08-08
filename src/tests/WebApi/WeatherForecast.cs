// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebApi
//  Author           : RzR
//  Created On       : 2022-08-07 12:41
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:00
// ***********************************************************************
//  <copyright file="WeatherForecast.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System;

#endregion

namespace WebApi
{
    public class WeatherForecast
    {
        public DateTime Date { get; set; }

        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int) (TemperatureC / 0.5556);

        public string Summary { get; set; }
    }
}