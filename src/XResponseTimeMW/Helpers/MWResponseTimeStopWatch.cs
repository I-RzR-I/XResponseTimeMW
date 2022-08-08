// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 21:59
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-06 22:04
// ***********************************************************************
//  <copyright file="MWResponseTimeStopWatch.cs" company="">
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

// ReSharper disable InconsistentNaming

namespace XResponseTimeMW.Helpers
{
    public class MWResponseTimeStopWatch : Stopwatch, IMWResponseTimeStopWatch
    {
        public MWResponseTimeStopWatch() : base()
        {

        }
    }
}