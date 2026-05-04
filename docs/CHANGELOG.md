### **v2.0.0.6861** [[RzR](mailto:108324929+I-RzR-I@users.noreply.github.com)] 04-05-2026
* [DEV] - (RzR) -> Public API renamed: `AddResponseTime()` / `UseResponseTime()`. Old names (`RegisterResponseTimeServices`, `UseResponseTimeMiddleware`) kept as `[Obsolete]` shims.
* [DEV] - (RzR) -> New `ResponseTimeOptions`: `TotalHeaderName`, `ActionHeaderName`, `EmitServerTiming`, `ServerTimingTotalMetric`, `ServerTimingActionMetric`, `DurationFormat`, `Filter`, `SlowRequestThreshold`.
* [DEV] - (RzR) -> Added W3C `Server-Timing` header alongside the legacy `X-*` headers (on by default).
* [DEV] - (RzR) -> Minimal API support (net7.0+): `RouteHandlerBuilder.WithResponseTime()` and `RouteGroupBuilder.WithResponseTime()`.
* [DEV] - (RzR) -> Stopwatch services moved from singleton to scoped — fixes a race condition under concurrent requests.
* [DEV] - (RzR) -> Multi-targets `netstandard2.0;net5.0;net6.0;net7.0;net8.0;net9.0`.
* [DEV] - (RzR) -> MSTest test project added; covers middleware, MVC attribute, endpoint filter, options and concurrency.

### **v.1.0.1.1818** 
-> Small code adjustments
