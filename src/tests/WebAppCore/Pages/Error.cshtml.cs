// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebAppCore
//  Author           : RzR
//  Created On       : 2022-08-07 12:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:01
// ***********************************************************************
//  <copyright file="Error.cshtml.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

#endregion

namespace WebAppCore.Pages
{
    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    [IgnoreAntiforgeryToken]
    public class ErrorModel : PageModel
    {
        public ErrorModel()
        {
        }

        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);

        public void OnGet()
        {
            RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier;
        }
    }
}