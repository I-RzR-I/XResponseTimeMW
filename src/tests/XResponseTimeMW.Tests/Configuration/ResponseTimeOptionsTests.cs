// ***********************************************************************
//  Assembly         : RzR.MiddleWares.XResponseTimeMW.Tests
//  Author           : RzR
//  Created On       : 2026-05-03 22:05
// 
//  Last Modified By : RzR
//  Last Modified On : 2026-05-03 22:52
// ***********************************************************************
//  <copyright file="ResponseTimeOptionsTests.cs" company="RzR SOFT & TECH">
//   Copyright © RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using RzR.Web.Middleware.ResponseTime.Configuration;

#endregion

namespace XResponseTimeMW.Tests.Configuration
{
    [TestClass]
    public class ResponseTimeOptionsTests
    {
        [TestMethod]
        public void Defaults_Match_Legacy_Behavior_Test()
        {
            var o = new ResponseTimeOptions();

            Assert.AreEqual("X-Response-Time", o.TotalHeaderName);
            Assert.AreEqual("X-Action-Response-Time", o.ActionHeaderName);
            Assert.IsTrue(o.EmitServerTiming);
            Assert.AreEqual("total", o.ServerTimingTotalMetric);
            Assert.AreEqual("action", o.ServerTimingActionMetric);
            Assert.AreEqual("0", o.DurationFormat);
            Assert.IsNull(o.Filter);
        }

        [TestMethod]
        public void Default_Constants_Match_Documented_Values_Test()
        {
            Assert.AreEqual("X-Response-Time", ResponseTimeOptions.DefaultTotalHeaderName);
            Assert.AreEqual("X-Action-Response-Time", ResponseTimeOptions.DefaultActionHeaderName);
            Assert.AreEqual("total", ResponseTimeOptions.DefaultServerTimingTotalMetric);
            Assert.AreEqual("action", ResponseTimeOptions.DefaultServerTimingActionMetric);
            Assert.AreEqual("0", ResponseTimeOptions.DefaultDurationFormat);
        }
    }
}