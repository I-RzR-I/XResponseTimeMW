// ***********************************************************************
//  Assembly         : RzR.MiddleWares.UniqXTraceIdMW
//  Author           : RzR
//  Created On       : 2022-08-01 13:55
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-03 18:53
// ***********************************************************************
//  <copyright file="GeneralAssemblyInfo.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Reflection;
using System.Resources;

#endregion

#if DEBUG
[assembly: AssemblyConfiguration("Debug")]
#else
[assembly: AssemblyConfiguration("Release")]
#endif

[assembly: AssemblyCompany("RzR ®")]
[assembly: AssemblyProduct("HTTP response time")]
[assembly: AssemblyCopyright("Copyright © 2022 RzR All rights reserved.")]
[assembly: AssemblyTrademark("® RzR™")]
[assembly: AssemblyDescription("Generate and write in HTTPContext response header variables request execution time('X-Response-Time') and also provide execution time for an action('X-Action-Response-Time'). This thing may help you when is needed to analyze too mutch time-consuming applications.")]

[assembly: AssemblyMetadata("TermsOfService", "")]

[assembly: AssemblyMetadata("ContactUrl", "")]
[assembly: AssemblyMetadata("ContactName", "RzR")]
[assembly: AssemblyMetadata("ContactEmail", "ddpRzR@hotmail.com")]

[assembly: NeutralResourcesLanguage("en-US", UltimateResourceFallbackLocation.MainAssembly)]

[assembly: AssemblyVersion("1.0.0.631")]
[assembly: AssemblyFileVersion("1.0.0.631")]
[assembly: AssemblyInformationalVersion("1.0.0.631")]