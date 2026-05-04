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
using RzR.Web.Middleware.ResponseTime.Abstractions;

#endregion

// ReSharper disable InconsistentNaming

namespace RzR.Web.Middleware.ResponseTime.Helpers
{
    /// <inheritdoc cref="IMWResponseTimeStopWatch" />
    public class MWResponseTimeStopWatch : Stopwatch, IMWResponseTimeStopWatch
    {
        /// <summary>
        ///     Initializes a new instance of the <see cref="MWResponseTimeStopWatch" /> class. 
        /// </summary>
        /// <remarks></remarks>
        public MWResponseTimeStopWatch() : base()
        {
        }
    }
}