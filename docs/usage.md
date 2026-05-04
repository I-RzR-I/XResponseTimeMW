# Usage

This page walks through the common ways to use the library. The API names below are the current ones; the older `RegisterResponseTimeServices` / `UseResponseTimeMiddleware` still work but are marked obsolete and will be removed in a future major version.

## 1. Wire up the services

In `Startup.ConfigureServices` (or directly on `builder.Services` if you use the minimal hosting model):

```csharp
public void ConfigureServices(IServiceCollection services)
{
    // ...

    services.AddResponseTime();

    // ...
}
```

## 2. Add the middleware

In `Startup.Configure` (or after `var app = builder.Build();`):

```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
{
    // ...

    app.UseResponseTime();

    // ...
}
```

After this, every response carries `X-Response-Time` and `Server-Timing` headers. The value is the total time the request spent inside the pipeline.

## 3. Time a specific MVC action

Add the `[ResponseTime]` attribute on the controller class, or on a single action:

```csharp
[HttpGet]
[ResponseTime]
public IEnumerable<WeatherForecast> Get()
{
    var rng = new Random();
    
    return Enumerable.Range(1, 5).Select(index => new WeatherForecast
    {
        Date = DateTime.Now.AddDays(index),
        TemperatureC = rng.Next(-20, 55),
        Summary = Summaries[rng.Next(Summaries.Length)]
    })
    .ToArray();
}
```

The response now also has `X-Action-Response-Time` and a second metric inside `Server-Timing`.

## 4. Time a minimal API endpoint

Available on `net7.0` and later. Use `.WithResponseTime()` on the endpoint:

```csharp
app.MapGet("/weather", () => Results.Ok(forecast))
   .WithResponseTime();
```

Or apply it to a whole group so every endpoint underneath inherits it:

```csharp
var api = app.MapGroup("/api").WithResponseTime();

api.MapGet("/users",  GetUsers);
api.MapPost("/users", CreateUser);
```

The endpoint filter does not require `AddResponseTime` to be called. If you skip the registration, the filter falls back to default options and still emits the headers. Calling `AddResponseTime` is only required when you want to configure something or use the MVC `[ResponseTime]` attribute.

## 5. Configuration

All knobs live on `ResponseTimeOptions`:

```csharp
services.AddResponseTime(o =>
{
    // Header names. Set to null or empty to suppress that specific header.
    o.TotalHeaderName  = "X-Response-Time";
    o.ActionHeaderName = "X-Action-Response-Time";

    // Server-Timing is on by default. Turn it off if you only want the X-* headers.
    o.EmitServerTiming = true;
    o.ServerTimingTotalMetric = "total";
    o.ServerTimingActionMetric = "action";

    // Numeric format applied to the millisecond value in the X-* headers.
    // "0" gives "12ms", "0.0" gives "12.3ms", "0.00" gives "12.34ms".
    o.DurationFormat = "0";

    // Skip timing for selected requests. Return false to suppress all headers.
    o.Filter = ctx => !ctx.Request.Path.StartsWithSegments("/health");
});
```

## 6. What you see on the wire

A typical response with both timings looks like this:

```
HTTP/1.1 200 OK
X-Response-Time: 12ms
X-Action-Response-Time: 8ms
Server-Timing: total;dur=12.0, action;dur=8.0
Content-Type: application/json; charset=utf-8

...
```

In Chrome / Edge / Firefox dev tools, the `Server-Timing` entries appear in the *Timing* tab of the request, broken down by metric name. No browser extension required.

## 7. Notes

- Headers are written from a `Response.OnStarting` callback, so they are emitted even for streaming or short-circuiting endpoints (`Results.Stream`, `Results.File`, server-sent events, and so on).
- If the response has already started by the time the timing finishes (rare, but possible with very early flushes), the headers are quietly skipped instead of throwing.
- Stopwatch services are registered as scoped, so concurrent requests do not interfere with each other.
