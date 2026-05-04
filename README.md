[![NuGet Version](https://img.shields.io/nuget/v/RzR.Web.Middleware.ResponseTime.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Web.Middleware.ResponseTime/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/RzR.Web.Middleware.ResponseTime.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/RzR.Web.Middleware.ResponseTime)

<details>

  <summary>Old version</summary>
  
[![NuGet Version](https://img.shields.io/nuget/v/XResponseTimeMW.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XResponseTimeMW/)
[![Nuget Downloads](https://img.shields.io/nuget/dt/XResponseTimeMW.svg?style=flat&logo=nuget)](https://www.nuget.org/packages/XResponseTimeMW)

</details>


# RzR.Web.Middleware.ResponseTime

A small ASP.NET Core library that measures how long requests take and exposes the result on the response.

There are two things you get out of the box:

- A middleware that records the total time spent serving a request.
- A filter that records the time spent inside a single action or minimal API endpoint.

Both write their result to the response so the client (or your browser dev tools) can read it without any extra plumbing.

## Headers

| Header | Source | Meaning |
| --- | --- | --- |
| `X-Response-Time` | middleware | Total time the request spent in the pipeline, in milliseconds. |
| `X-Action-Response-Time` | `[ResponseTime]` attribute or `.WithResponseTime()` filter | Time spent inside the marked action / endpoint. |
| `Server-Timing` | both | Standard W3C header. Each contributor adds a metric (`total`, `action`). Browsers render this in the Network tab. |

The `X-*` names are kept for backward compatibility. New consumers should prefer `Server-Timing` because it is a documented standard and shows up in dev tools without a custom extension.

## Targets

`netstandard2.0`, `net5.0`, `net6.0`, `net7.0`, `net8.0`, `net9.0`. The minimal API endpoint filter is only compiled for `net7.0` and above (it relies on `IEndpointFilter`).

## Install

From NuGet:

```
Install-Package RzR.Web.Middleware.ResponseTime
```

Or pin a specific version:

```
Install-Package RzR.Web.Middleware.ResponseTime -Version x.x.x.x
```

## Quick start

Register the services and add the middleware. That is enough to start emitting `X-Response-Time` and `Server-Timing` on every response.

```csharp
services.AddResponseTime();

// ...

app.UseResponseTime();
```

For per-action timing on an MVC controller, decorate the action (or the controller) with `[ResponseTime]`:

```csharp
[HttpGet]
[ResponseTime]
public IEnumerable<WeatherForecast> Get() => ...;
```

For minimal APIs, chain `.WithResponseTime()` on the endpoint or group:

```csharp
app.MapGet("/weather", () => Results.Ok(forecast))
   .WithResponseTime();

var api = app.MapGroup("/api").WithResponseTime();
api.MapGet("/users", GetUsers);
```

## Configuration

Pass a callback to `AddResponseTime` to override defaults:

```csharp
services.AddResponseTime(o =>
{
    o.TotalHeaderName = "X-Total-Time"; // or null/empty to suppress
    o.ActionHeaderName = "X-Action-Time";
    o.EmitServerTiming = true;
    o.DurationFormat = "0.0"; // e.g. "12.3ms"
    o.Filter  = ctx => !ctx.Request.Path.StartsWithSegments("/health");
});
```

See `ResponseTimeOptions` for the full list of settings.

## Documentation

1. [USAGE](docs/usage.md)
2. [CHANGELOG](docs/CHANGELOG.md)
3. [BRANCH GUIDE](docs/branch-guide.md)
3. [MIGRATION](docs/migration.md)
