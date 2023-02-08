> **Note** This repository is developed in .netstandard2.0

[![NuGet Version](https://img.shields.io/nuget/v/XResponseTimeMW.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XResponseTimeMW/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/XResponseTimeMW.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XResponseTimeMW)

One important thing about this repository is that you have the possibility to monitor the execution time for the request sent to the server and the execution time for some method or class where is added data annotation attribute.

Once you use this repository, in server response you may see one or two new variables, meaning request execution time. Added variables (`X-Response-Time` and `X-Action-Response-Time`) will be location in `HttpContext.Response.Headers`. 

In other words in this repository is included one middleware that allows to add and return of general execution time per request (the first header variable).

And one more thing that was added is an action filter that can provide execution time for a specific method when will be added data annotation tag (the second header variable).

1. `X-Response-Time` - represent execution time for specific request;
2. `X-Action-Response-Time` - represent execution time for specific action.

**In case you wish to use it in your project, u can install the package from <a href="https://www.nuget.org/packages/XResponseTimeMW" target="_blank">nuget.org</a>** or specify what version you want:

> `Install-Package XResponseTimeMW -Version x.x.x.x`

## Content
1. [USING](docs/usage.md)
1. [CHANGELOG](docs/CHANGELOG.md)
1. [BRANCH-GUIDE](docs/branch-guide.md)