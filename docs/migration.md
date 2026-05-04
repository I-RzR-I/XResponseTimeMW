# Migration guide

## From 1.x to 2.x

The wire format is unchanged by default, so most apps only need to rename two calls.

### Method renames

| 1.x | 2.x |
| --- | --- |
| `services.RegisterResponseTimeServices()` | `services.AddResponseTime()` |
| `services.RegisterResponseTimeServices(opts)` | `services.AddResponseTime(opts)` |
| `app.UseResponseTimeMiddleware()` | `app.UseResponseTime()` |

The 1.x names still compile in 2.x but produce an `[Obsolete]` warning. They will be removed in 3.x.

### Service lifetime

`IMWResponseTimeStopWatch` and `IActionResponseTimeStopWatch` are now registered as **scoped** instead of singleton. If you resolved them manually from the root provider, switch to resolving from `HttpContext.RequestServices` (or any scoped provider).

### New defaults

After upgrading, every response also carries a `Server-Timing` header. To restore the previous behaviour:

```csharp
services.AddResponseTime(o => o.EmitServerTiming = false);