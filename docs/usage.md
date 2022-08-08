# USING

For using this middleware you need to add specific pieces of code written below.
Add this two pieces (`services.RegisterResponseTimeServices()` and `app.UseResponseTimeMiddleware()`) in your `Startup.cs`.

First what is required to add:
```csharp
public void ConfigureServices(IServiceCollection services)
        {
            ...
            
            services.RegisterResponseTimeServices();
            
            ...
        }
```

Second what is required to add:
```csharp
public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ...
            
            app.UseResponseTimeMiddleware();
            
            ...
        }
```

After adding these things, to the response message (header) from the server, you can see a new variable `X-Response-Time`, which means request execution time (in milliseconds).

Also when you want to see how much time took execution for some method or class you must add the attribute `[ResponseTime]`. After adding you may see the a new header variable `X-Action-Response-Time`, which means how much time took for method execution.

An example of using a response time filter is presented below:
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