﻿// ***********************************************************************
//  Assembly         : RzR.MiddleWares.WebAppCore
//  Author           : RzR
//  Created On       : 2022-08-07 12:26
// 
//  Last Modified By : RzR
//  Last Modified On : 2022-08-07 13:01
// ***********************************************************************
//  <copyright file="Privacy.cshtml.cs" company="">
//   Copyright (c) RzR. All rights reserved.
//  </copyright>
// 
//  <summary>
//  </summary>
// ***********************************************************************

#region U S A G E S

using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

#endregion

namespace WebAppCore.Pages
{
    public class PrivacyModel : PageModel
    {
        private readonly ILogger<PrivacyModel> _logger;

        public PrivacyModel(ILogger<PrivacyModel> logger)
        {
            _logger = logger;
        }

        public void OnGet()
        {
        }
    }
}