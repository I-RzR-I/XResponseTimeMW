// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 22:59
// ***********************************************************************
//  <copyright file="PingController.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc;
using RzR.Web.Middleware.ResponseTime.Filters;

#endregion

namespace XResponseTimeMW.Tests.Controllers
{
    [ApiController]
    [Route("ping")]
    public sealed class PingController : ControllerBase
    {
        [HttpGet]
        [ResponseTime]
        public IActionResult Get()
        {
            return Ok("pong");
        }

        [HttpGet("plain")]
        public IActionResult Plain()
        {
            return Ok("plain");
        }
    }
}