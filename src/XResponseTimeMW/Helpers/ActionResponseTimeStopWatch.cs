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
    /// <inheritdoc cref="IActionResponseTimeStopWatch"/>
    public class ActionResponseTimeStopWatch : Stopwatch, IActionResponseTimeStopWatch
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="XResponseTimeMW.Helpers.ActionResponseTimeStopWatch" /> class. 
        /// </summary>
        /// <remarks></remarks>
        public ActionResponseTimeStopWatch() : base()
        {
        }
    }
}