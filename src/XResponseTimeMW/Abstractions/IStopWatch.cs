// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW
//  Author           : RzR
//  Created On       : 2022-08-06 21:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 12:56
// ***********************************************************************
//  <copyright file="IStopWatch.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

namespace XResponseTimeMW.Abstractions
{
    /// <summary>
    ///     Stop watch
    /// </summary>
    public interface IStopWatch
    {
        /// <summary>
        ///     Gets elapsed time from start.
        /// </summary>
        /// <value></value>
        /// <remarks></remarks>
        long ElapsedMilliseconds { get; }

        /// <summary>
        ///     Start watch timer
        /// </summary>
        /// <remarks></remarks>
        void Start();

        /// <summary>
        ///     Stop watch timer
        /// </summary>
        /// <remarks></remarks>
        void Stop();

        /// <summary>
        ///     Reset watch timer
        /// </summary>
        /// <remarks></remarks>
        void Reset();
    }
}