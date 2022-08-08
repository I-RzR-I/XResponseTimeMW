// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 22:13
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-06 22:13
// ***********************************************************************
//  <copyright file="ActionResponseTimeStopWatch.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Diagnostics;
using XResponseTimeMW.Abstractions;

#endregion

namespace XResponseTimeMW.Helpers
{
    public class ActionResponseTimeStopWatch : Stopwatch, IActionResponseTimeStopWatch
    {
        public ActionResponseTimeStopWatch() : base()
        {

        }
    }
}